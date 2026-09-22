namespace AssureCloud.Domain;

public abstract class DomainEvent
{
    public Guid EventId { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset Timestamp { get; protected set; } = DateTimeOffset.UtcNow;
}
