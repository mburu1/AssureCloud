using System;

namespace AssureCloud.Messaging.Messages;

public record ReportCreatedMessage
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ReportNumber { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Format { get; init; } = string.Empty;
    public Guid OrganizationId { get; init; }
    public Guid? ProgramId { get; init; }
    public Guid? AssessmentId { get; init; }
    public Guid? AuditId { get; init; }
    public Guid? CertificationId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record ReportGeneratedMessage
{
    public Guid Id { get; init; }
    public string ReportNumber { get; init; } = string.Empty;
    public string FilePath { get; init; } = string.Empty;
    public long FileSize { get; init; }
    public string MimeType { get; init; } = string.Empty;
    public DateTimeOffset GeneratedAt { get; init; }
    public string GeneratedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record ReportReviewedMessage
{
    public Guid Id { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? ReviewComments { get; init; }
    public DateTimeOffset ReviewedAt { get; init; }
    public string ReviewedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record ReportApprovedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset ApprovedAt { get; init; }
    public string ApprovedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record ReportDeletedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset DeletedAt { get; init; }
    public string DeletedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}