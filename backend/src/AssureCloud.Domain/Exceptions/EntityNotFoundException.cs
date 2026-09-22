namespace AssureCloud.Domain.Exceptions;

using System;

public class EntityNotFoundException : DomainException
{
    public Guid EntityId { get; }
    public string EntityType { get; }

    public EntityNotFoundException(Guid entityId, string entityType)
        : base($"{entityType} with id '{entityId}' was not found.")
    {
        EntityId = entityId;
        EntityType = entityType;
    }

    public EntityNotFoundException(string message) : base(message) { }
}
