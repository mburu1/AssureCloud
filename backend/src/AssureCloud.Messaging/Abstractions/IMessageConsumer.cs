using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Messaging.Abstractions;

public interface IMessageConsumer<T> where T : class
{
    Task ConsumeAsync(T message, CancellationToken cancellationToken);
}

public interface IMessageHandler
{
    string TopicName { get; }
    string SubscriptionName { get; }
    Task HandleAsync(object message, CancellationToken cancellationToken);
}