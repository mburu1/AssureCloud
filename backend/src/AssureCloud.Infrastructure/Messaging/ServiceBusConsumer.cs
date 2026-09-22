using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace AssureCloud.Infrastructure.Messaging;

public interface IMessageConsumer
{
    Task StartAsync(CancellationToken cancellationToken = default);
    Task StopAsync(CancellationToken cancellationToken = default);
}

public class ServiceBusConsumer : IMessageConsumer
{
    private readonly ServiceBusProcessor _processor;
    private readonly ConcurrentDictionary<string, Func<ServiceBusReceivedMessage, ServiceBusMessageActions, Task>> _handlers;

    public ServiceBusConsumer(ServiceBusClient client, string queueOrTopicName, string? subscriptionName = null)
    {
        var processorOptions = new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 16,
            AutoCompleteMessages = false
        };

        _processor = string.IsNullOrEmpty(subscriptionName)
            ? client.CreateReceiver(queueOrTopicName, processorOptions)
            : client.CreateProcessor(queueOrTopicName, subscriptionName, processorOptions);

        _handlers = new ConcurrentDictionary<string, Func<ServiceBusReceivedMessage, ServiceBusMessageActions, Task>>();
    }

    public void RegisterHandler<T>(Func<T, ServiceBusMessageActions, Task> handler, string? messageType = null)
    {
        var type = messageType ?? typeof(T).FullName!;
        _handlers.TryAdd(type, async (message, actions) =>
        {
            var body = message.Body.ToString();
            var obj = System.Text.Json.JsonSerializer.Deserialize<T>(body);
            if (obj != null)
            {
                await handler(obj, actions);
            }
        });
    }

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return _processor.StartProcessingAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _processor.StopProcessingAsync(cancellationToken);
    }
}
