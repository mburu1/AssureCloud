using System;

namespace AssureCloud.Messaging.Messages;

public record AuditCreatedMessage
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public Guid ProgramId { get; init; }
    public Guid OrganizationId { get; init; }
    public Guid? LeadAuditorId { get; init; }
    public string Status { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public DateTimeOffset ScheduledStartDate { get; init; }
    public DateTimeOffset ScheduledEndDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AuditStartedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset StartedAt { get; init; }
    public string StartedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AuditCompletedMessage
{
    public Guid Id { get; init; }
    public string? OverallRating { get; init; }
    public DateTimeOffset CompletedAt { get; init; }
    public string CompletedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AuditDeletedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset DeletedAt { get; init; }
    public string DeletedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AuditFindingCreatedMessage
{
    public Guid AuditId { get; init; }
    public Guid FindingId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Severity { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string? RequirementReference { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CorrectiveActionCreatedMessage
{
    public Guid AuditId { get; init; }
    public Guid CorrectiveActionId { get; init; }
    public Guid? FindingId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Priority { get; init; } = string.Empty;
    public DateTimeOffset DueDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CorrectiveActionVerifiedMessage
{
    public Guid CorrectiveActionId { get; init; }
    public bool IsEffective { get; init; }
    public string? EffectivenessReview { get; init; }
    public DateTimeOffset VerifiedAt { get; init; }
    public string VerifiedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AuditAssignedMessage
{
    public Guid AuditId { get; init; }
    public Guid AssignmentId { get; init; }
    public Guid UserId { get; init; }
    public string Role { get; init; } = string.Empty;
    public DateTimeOffset AssignedAt { get; init; }
    public string AssignedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}