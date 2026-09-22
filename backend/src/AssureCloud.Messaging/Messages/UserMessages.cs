using System;

namespace AssureCloud.Messaging.Messages;

public record UserCreatedMessage
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty
    public Guid OrganizationId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record UserUpdatedMessage
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; init; }
    public string UpdatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record UserDeletedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset DeletedAt { get; init; }
    public string DeletedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record UserActivatedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset ActivatedAt { get; init; }
    public string ActivatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record UserDeactivatedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset DeactivatedAt { get; init; }
    public string DeactivatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record UserRoleAssignedMessage
{
    public Guid UserId { get; init; }
    public string Role { get; init; } = string.Empty;
    public DateTimeOffset AssignedAt { get; init; }
    public string AssignedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record UserRoleRemovedMessage
{
    public Guid UserId { get; init; }
    public string Role { get; init; } = string.Empty;
    public DateTimeOffset RemovedAt { get; init; }
    public string RemovedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}