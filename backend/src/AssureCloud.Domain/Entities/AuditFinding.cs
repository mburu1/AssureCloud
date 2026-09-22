using System;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class AuditFinding : BaseEntity
{
    public Guid AuditId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public FindingSeverity Severity { get; private set; }
    public FindingCategory Category { get; private set; }
    public Guid? CriterionId { get; private set; }
    public string? Requirement { get; private set; }
    public FindingStatus Status { get; private set; } = FindingStatus.Open;
    public string? RootCause { get; private set; }
    public string? Impact { get; private set; }
    public string? Recommendation { get; private set; }
    public Guid? IdentifiedById { get; private set; }
    public DateTime? IdentifiedAt { get; private set; }
    public Guid? ResolvedById { get; private set; }
    public DateTime? ResolvedAt { get; private set; }
    public string? ResolutionNotes { get; private set; }
    public CorrectiveAction? CorrectiveAction { get; private set; }

    private AuditFinding() { }

    public AuditFinding(
        Guid auditId,
        string title,
        string description,
        FindingSeverity severity,
        FindingCategory category,
        Guid? criterionId = null,
        string? requirement = null)
    {
        AuditId = auditId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Severity = severity;
        Category = category;
        CriterionId = criterionId;
        Requirement = requirement;
        IdentifiedAt = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string title,
        string description,
        FindingSeverity severity,
        FindingCategory category,
        string? rootCause = null,
        string? impact = null,
        string? recommendation = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Severity = severity;
        Category = category;
        RootCause = rootCause;
        Impact = impact;
        Recommendation = recommendation;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(FindingStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetIdentifiedBy(Guid userId)
    {
        IdentifiedById = userId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Resolve(Guid userId, string? resolutionNotes = null)
    {
        ResolvedById = userId;
        ResolvedAt = DateTime.UtcNow;
        ResolutionNotes = resolutionNotes;
        Status = FindingStatus.Resolved;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AuditFindingResolvedEvent(AuditId, Id));
    }

    public void Reopen()
    {
        ResolvedById = null;
        ResolvedAt = null;
        ResolutionNotes = null;
        Status = FindingStatus.Open;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignCorrectiveAction(CorrectiveAction correctiveAction)
    {
        CorrectiveAction = correctiveAction;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsOpen => Status == FindingStatus.Open || Status == FindingStatus.UnderReview;
    public bool IsResolved => Status == FindingStatus.Resolved || Status == FindingStatus.Closed;
}