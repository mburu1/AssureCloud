using System;

namespace AssureCloud.Messaging.Messages;

public record AssessmentCreatedMessage
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public Guid ProgramId { get; init; }
    public Guid OrganizationId { get; init; }
    public Guid? AssessorId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset ScheduledStartDate { get; init; }
    public DateTimeOffset ScheduledEndDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AssessmentStartedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset StartedAt { get; init; }
    public string StartedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AssessmentCompletedMessage
{
    public Guid Id { get; init; }
    public decimal OverallScore { get; init; }
    public decimal MaxPossibleScore { get; init; }
    public DateTimeOffset CompletedAt { get; init; }
    public string CompletedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AssessmentDeletedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset DeletedAt { get; init; }
    public string DeletedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AssessmentResponseSubmittedMessage
{
    public Guid AssessmentId { get; init; }
    public Guid ResponseId { get; init; }
    public Guid CriterionId { get; init; }
    public Guid RespondentId { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal? Score { get; init; }
    public DateTimeOffset SubmittedAt { get; init; }
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record EvidenceUploadedMessage
{
    public Guid AssessmentId { get; init; }
    public Guid EvidenceId { get; init; }
    public Guid? CriterionId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public long FileSize { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset UploadedAt { get; init; }
    public string UploadedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record FindingCreatedMessage
{
    public Guid AssessmentId { get; init; }
    public Guid FindingId { get; init; }
    public Guid? CriterionId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Severity { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record AssessmentAssignedMessage
{
    public Guid AssessmentId { get; init; }
    public Guid AssignmentId { get; init; }
    public Guid UserId { get; init; }
    public string Role { get; init; } = string.Empty;
    public DateTimeOffset AssignedAt { get; init; }
    public string AssignedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}