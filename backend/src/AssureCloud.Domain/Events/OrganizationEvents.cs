using System;

namespace AssureCloud.Domain.Events;

public sealed class OrganizationCreatedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public string Name { get; }

    public OrganizationCreatedEvent(Guid organizationId, string name)
    {
        OrganizationId = organizationId;
        Name = name;
    }
}

public sealed class OrganizationUpdatedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public string Name { get; }

    public OrganizationUpdatedEvent(Guid organizationId, string name)
    {
        OrganizationId = organizationId;
        Name = name;
    }
}

public sealed class OrganizationDeletedEvent : DomainEvent
{
    public Guid OrganizationId { get; }

    public OrganizationDeletedEvent(Guid organizationId)
    {
        OrganizationId = organizationId;
    }
}

public sealed class OrganizationStatusChangedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public string OldStatus { get; }
    public string NewStatus { get; }

    public OrganizationStatusChangedEvent(Guid organizationId, string oldStatus, string newStatus)
    {
        OrganizationId = organizationId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}

public sealed class OrganizationLocationAddedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public Guid LocationId { get; }

    public OrganizationLocationAddedEvent(Guid organizationId, Guid locationId)
    {
        OrganizationId = organizationId;
        LocationId = locationId;
    }
}

public sealed class OrganizationLocationRemovedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public Guid LocationId { get; }

    public OrganizationLocationRemovedEvent(Guid organizationId, Guid locationId)
    {
        OrganizationId = organizationId;
        LocationId = locationId;
    }
}

public sealed class SupplierAddedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public Guid SupplierId { get; }

    public SupplierAddedEvent(Guid organizationId, Guid supplierId)
    {
        OrganizationId = organizationId;
        SupplierId = supplierId;
    }
}

public sealed class SupplierRemovedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public Guid SupplierId { get; }

    public SupplierRemovedEvent(Guid organizationId, Guid supplierId)
    {
        OrganizationId = organizationId;
        SupplierId = supplierId;
    }
}

public sealed class UserAddedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public Guid UserId { get; }

    public UserAddedEvent(Guid organizationId, Guid userId)
    {
        OrganizationId = organizationId;
        UserId = userId;
    }
}

public sealed class UserRemovedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public Guid UserId { get; }

    public UserRemovedEvent(Guid organizationId, Guid userId)
    {
        OrganizationId = organizationId;
        UserId = userId;
    }
}