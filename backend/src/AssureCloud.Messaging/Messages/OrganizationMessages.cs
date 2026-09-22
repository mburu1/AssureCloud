using System;

namespace AssureCloud.Messaging.Messages;

public record OrganizationCreatedMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid? ParentOrganizationId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record OrganizationUpdatedMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; init; }
    public string UpdatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record OrganizationDeletedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset DeletedAt { get; init; }
    public string DeletedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record OrganizationLocationAddedMessage
{
    public Guid OrganizationId { get; init; }
    public Guid LocationId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public bool IsPrimary { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record SupplierAddedMessage
{
    public Guid OrganizationId { get; init; }
    public Guid SupplierId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}