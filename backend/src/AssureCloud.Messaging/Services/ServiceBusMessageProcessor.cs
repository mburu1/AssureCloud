using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using AssureCloud.Messaging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AssureCloud.Messaging.Services;

public class ServiceBusMessageProcessor : IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly MessagingOptions _options;
    private readonly ILogger<ServiceBusMessageProcessor> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, ServiceBusProcessor> _processors = new();

    public ServiceBusMessageProcessor(
        ServiceBusClient client,
        IOptions<MessagingOptions> options,
        ILogger<ServiceBusMessageProcessor> logger,
        IServiceProvider serviceProvider)
    {
        _client = client;
        _options = options.Value;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartProcessingAsync(CancellationToken cancellationToken = default)
    {
        var topicNames = new[]
        {
            _options.OrganizationTopicName,
            _options.ProgramTopicName,
            _options.AssessmentTopicName,
            _options.AuditTopicName,
            _options.CertificationTopicName,
            _options.ReportTopicName,
            _options.UserTopicName
        };

        foreach (var topicName in topicNames)
        {
            var subscriptionName = $"assurecloud-{Environment.MachineName.ToLowerInvariant()}";

            var processor = _client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 10,
                PrefetchCount = 100,
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            });

            processor.ProcessMessageAsync += ProcessMessageAsync;
            processor.ProcessErrorAsync += ProcessErrorAsync;

            _processors[topicName] = processor;
            await processor.StartProcessingAsync(cancellationToken);

            _logger.LogInformation("Started processing messages for topic: {TopicName}", topicName);
        }
    }

    public async Task StopProcessingAsync(CancellationToken cancellationToken = default)
    {
        foreach (var processor in _processors.Values)
        {
            await processor.StopProcessingAsync(cancellationToken);
            await processor.DisposeAsync();
        }
        _processors.Clear();
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var messageType = message.ApplicationProperties.TryGetValue("MessageType", out var typeObj) ? typeObj?.ToString() : "Unknown";

        _logger.LogDebug("Received message of type {MessageType} from topic {TopicName}", messageType, args.FullyQualifiedNamespace);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<IMessageHandler>();

            foreach (var handler in handlers)
            {
                if (handler.TopicName.Equals(args.FullyQualifiedNamespace.Split('/').LastOrDefault()?.Split('.').FirstOrDefault(), StringComparison.OrdinalIgnoreCase) ||
                    handler.TopicName.Equals(message.Subject, StringComparison.OrdinalIgnoreCase))
                {
                    var messageObject = DeserializeMessage(message, messageType);
                    if (messageObject != null)
                    {
                        await handler.HandleAsync(messageObject, args.CancellationToken);
                    }
                }
            }

            await args.CompleteMessageAsync(message, args.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message: {MessageId}", message.MessageId);

            if (message.DeliveryCount >= 3)
            {
                await args.DeadLetterMessageAsync(message, ex.Message, ex.StackTrace, args.CancellationToken);
            }
            else
            {
                await args.AbandonMessageAsync(message, args.CancellationToken);
            }
        }
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error in message processor for {ErrorSource}: {ExceptionMessage}",
            args.ErrorSource, args.Exception.Message);
        return Task.CompletedTask;
    }

    private static object? DeserializeMessage(ServiceBusReceivedMessage message, string messageType)
    {
        try
        {
            var body = message.Body.ToString();
            var type = Type.GetType($"AssureCloud.Messaging.Messages.{messageType}, AssureCloud.Messaging");

            if (type != null)
            {
                return JsonSerializer.Deserialize(body, type, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await StopProcessingAsync();
        await _client.DisposeAsync();
    }
}