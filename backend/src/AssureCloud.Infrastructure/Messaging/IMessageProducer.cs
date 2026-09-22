using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Infrastructure.Messaging;

public interface IMessageProducer
{
    Task PublishAsync<T>(T message, string subject, CancellationToken ct = default);
    Task SendMessageAsync<T>(T message, string queueName, CancellationToken ct = default);
}
