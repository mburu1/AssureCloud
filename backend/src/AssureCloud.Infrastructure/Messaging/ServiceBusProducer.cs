using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace AssureCloud.Infrastructure.Messaging;

public class ServiceBusProducer : IMessageProducer, IDisposable, IAsyncDisposable
{
    private readonly ServiceBusSender _sender;
    private readonly JsonSerializerOptions _options;

    public ServiceBusProducer(ServiceBusClient client, string queueOrTopicName)
    {
        _sender = client.CreateSender(queueOrTopicName);
        _options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    public async Task PublishAsync<T>(T message, string subject, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(message, _options);
        using var messageEnvelope = new ServiceBusMessage(json)
        {
            ContentType = typeof(T).FullName,
            Subject = subject,
        };
        messageEnvelope.ApplicationInputs["message-type"] = typeof(T).FullName!;
        await _sender.SendMessageAsync(messageEnvelope, ct);
    }

    public async Task SendMessageAsync<T>(T message, string queueName, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(message, _options);
        using var messageEnvelope = new ServiceBusMessage(json)
        {
            ContentType = typeof(T).FullName,
        };
        await _sender.SendMessageAsync(messageEnvelope, ct);
    }

    public void Dispose()
    {
        _sender?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _sender?.DisposeAsync();
    }
}
