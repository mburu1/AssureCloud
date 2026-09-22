using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using AssureCloud.Messaging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace AssureCloud.Messaging.Services;

public class ServiceBusMessagePublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly ServiceBusClient _client;
    private readonly MessagingOptions _options;
    private readonly ILogger<ServiceBusMessagePublisher> _logger;
    private readonly Dictionary<string, ServiceBusSender> _senders = new();

    public ServiceBusMessagePublisher(
        ServiceBusClient client,
        IOptions<MessagingOptions> options,
        ILogger<ServiceBusMessagePublisher> logger)
    {
        _client = client;
        _options = options.Value;
        _logger = logger;
    }

    private ServiceBusSender GetOrCreateSender(string topicName)
    {
        if (!_senders.TryGetValue(topicName, out var sender))
        {
            sender = _client.CreateSender(topicName);
            _senders[topicName] = sender;
        }
        return sender;
    }

    public async Task PublishAsync<T>(T message, string topicName, CancellationToken cancellationToken = default) where T : class
    {
        var sender = GetOrCreateSender(topicName);

        var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var serviceBusMessage = new ServiceBusMessage(json)
        {
            ContentType = "application/json",
            MessageId = Guid.NewGuid().ToString(),
            CorrelationId = GetCorrelationId(message),
            Subject = typeof(T).Name
        };

        // Add custom properties
        serviceBusMessage.ApplicationProperties["MessageType"] = typeof(T).Name;
        serviceBusMessage.ApplicationProperties["Timestamp"] = DateTimeOffset.UtcNow.ToString("O");

        await sender.SendMessageAsync(serviceBusMessage, cancellationToken);

        _logger.LogDebug("Published message of type {MessageType} to topic {TopicName}", typeof(T).Name, topicName);
    }

    public async Task PublishBatchAsync<T>(IEnumerable<T> messages, string topicName, CancellationToken cancellationToken = default) where T : class
    {
        var sender = GetOrCreateSender(topicName);
        var messageBatch = await sender.CreateMessageBatchAsync(cancellationToken);

        foreach (var message in messages)
        {
            var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var serviceBusMessage = new ServiceBusMessage(json)
            {
                ContentType = "application/json",
                MessageId = Guid.NewGuid().ToString(),
                CorrelationId = GetCorrelationId(message),
                Subject = typeof(T).Name
            };

            serviceBusMessage.ApplicationProperties["MessageType"] = typeof(T).Name;
            serviceBusMessage.ApplicationProperties["Timestamp"] = DateTimeOffset.UtcNow.ToString("O");

            if (!messageBatch.TryAddMessage(serviceBusMessage))
            {
                // Batch is full, send current batch and create new one
                await sender.SendMessagesAsync(messageBatch, cancellationToken);
                messageBatch = await sender.CreateMessageBatchAsync(cancellationToken);

                if (!messageBatch.TryAddMessage(serviceBusMessage))
                {
                    _logger.LogWarning("Message too large for batch: {MessageType}", typeof(T).Name);
                }
            }
        }

        if (messageBatch.Count > 0)
        {
            await sender.SendMessagesAsync(messageBatch, cancellationToken);
        }

        _logger.LogDebug("Published batch of messages of type {MessageType} to topic {TopicName}", typeof(T).Name, topicName);
    }

    private static string? GetCorrelationId<T>(T message) where T : class
    {
        // Try to get correlation ID from message properties
        var correlationIdProperty = typeof(T).GetProperty("CorrelationId");
        if (correlationIdProperty != null)
        {
            return correlationIdProperty.GetValue(message)?.ToString();
        }

        var idProperty = typeof(T).GetProperty("Id");
        if (idProperty != null)
        {
            return idProperty.GetValue(message)?.ToString();
        }

        return null;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var sender in _senders.Values)
        {
            await sender.DisposeAsync();
        }
        _senders.Clear();
        await _client.DisposeAsync();
    }
}