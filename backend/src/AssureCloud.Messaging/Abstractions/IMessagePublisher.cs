using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Messaging.Abstractions;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string topicName, CancellationToken cancellationToken = default) where T : class;
    Task PublishBatchAsync<T>(IEnumerable<T> messages, string topicName, CancellationToken cancellationToken = default) where T : class;
}