using System;

namespace AssureCloud.Messaging.Messages;

public record CertificationCreatedMessage
{
    public Guid Id { get; init; }
    public string CertificateNumber { get; init; } = string.Empty;
    public Guid ProgramId { get; init; }
    public Guid OrganizationId { get; init; }
    public Guid? AuditId { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset IssueDate { get; init; }
    public DateTimeOffset ExpiryDate { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CertificationIssuedMessage
{
    public Guid Id { get; init; }
    public string CertificateNumber { get; init; } = string.Empty;
    public DateTimeOffset IssuedAt { get; init; }
    public string IssuedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CertificationSuspendedMessage
{
    public Guid Id { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTimeOffset SuspendedAt { get; init; }
    public string SuspendedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CertificationRevokedMessage
{
    public Guid Id { get; init; }
    public string Reason { get; init; } = string.Empty;
    public DateTimeOffset RevokedAt { get; init; }
    public string RevokedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CertificationRenewedMessage
{
    public Guid Id { get; init; }
    public Guid NewCertificationId { get; init; }
    public string NewCertificateNumber { get; init; } = string.Empty;
    public DateTimeOffset RenewedAt { get; init; }
    public string RenewedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CertificationDeletedMessage
{
    public Guid Id { get; init; }
    public DateTimeOffset DeletedAt { get; init; }
    public string DeletedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CertificationDecisionAddedMessage
{
    public Guid CertificationId { get; init; }
    public Guid DecisionId { get; init; }
    public string Decision { get; init; } = string.Empty;
    public string? Rationale { get; init; }
    public DateTimeOffset DecisionDate { get; init; }
    public string DecidedBy { get; init; } = string.Empty;
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}

public record CertificationScopeAddedMessage
{
    public Guid CertificationId { get; init; }
    public Guid ScopeId { get; init; }
    public string Description { get; init; } = string.Empty;
    public string? StandardClause { get; init; }
    public string? Location { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();
}