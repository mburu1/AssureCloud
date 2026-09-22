using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Messaging.Contracts;

public interface IMessage
{
    Guid Id { get; }
    DateTimeOffset Timestamp { get; }
    string CorrelationId { get; }
}

public abstract class Message : IMessage
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset Timestamp { get; protected set; } = DateTimeOffset.UtcNow;
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString();
}
