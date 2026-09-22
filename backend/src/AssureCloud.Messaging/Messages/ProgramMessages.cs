using System;

namespace AssureCloud.Messaging.Messages;

public record ProgramCreatedMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public Guid OrganizationId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record ProgramUpdatedMessage
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; init; }
    public string UpdatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record ProgramDeletedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset DeletedAt { get; init; }
    public string DeletedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record StandardAddedMessage
{
    public Guid ProgramId { get; init; }
    public Guid StandardId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record RequirementAddedMessage
{
    public Guid StandardId { get; init; }
    public Guid RequirementId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public decimal Weight { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CriterionAddedMessage
{
    public Guid RequirementId { get; init; }
    public Guid CriterionId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string ResponseType { get; init; } = string.Empty;
    public decimal Weight { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record ControlAddedMessage
{
    public Guid CriterionId { get; init; }
    public Guid ControlId { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Frequency { get; init; } = string.Empty;
    public string AutomationLevel { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}