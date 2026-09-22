using System;
using AssureCloud.Domain.Events;

namespace AssureCloud.Domain.Entities;

public class Finding : BaseEntity
{
    public Guid AssessmentId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public FindingSeverity Severity { get; private set; }
    public FindingCategory Category { get; private set; }
    public Guid? CriterionId { get; private set; }
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

    private Finding() { }

    public Finding(
        Guid assessmentId,
        string title,
        string description,
        FindingSeverity severity,
        FindingCategory category,
        Guid? criterionId = null)
    {
        AssessmentId = assessmentId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Severity = severity;
        Category = category;
        CriterionId = criterionId;
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
        AddDomainEvent(new FindingResolvedEvent(AssessmentId, Id));
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

public class FindingSeverity : Enumeration
{
    public static readonly FindingSeverity Low = new(0, "Low");
    public static readonly FindingSeverity Medium = new(1, "Medium");
    public static readonly FindingSeverity High = new(2, "High");
    public static readonly FindingSeverity Critical = new(3, "Critical");
    public static readonly FindingSeverity Informational = new(4, "Informational");

    private FindingSeverity(int value, string name) : base(value, name) { }
}

public class FindingCategory : Enumeration
{
    public static readonly FindingCategory NonConformity = new(0, "NonConformity");
    public static readonly FindingCategory Observation = new(1, "Observation");
    public static readonly FindingCategory OpportunityForImprovement = new(2, "OpportunityForImprovement");
    public static readonly FindingCategory PositiveFinding = new(3, "PositiveFinding");
    public static readonly FindingCategory MinorNonConformity = new(4, "MinorNonConformity");
    public static readonly FindingCategory MajorNonConformity = new(5, "MajorNonConformity");

    private FindingCategory(int value, string name) : base(value, name) { }
}

public class FindingStatus : Enumeration
{
    public static readonly FindingStatus Open = new(0, "Open");
    public static readonly FindingStatus UnderReview = new(1, "UnderReview");
    public static readonly FindingStatus Resolved = new(2, "Resolved");
    public static readonly FindingStatus Closed = new(3, "Closed");
    public static readonly FindingStatus Deferred = new(4, "Deferred");
    public static readonly FindingStatus Rejected = new(5, "Rejected");

    private FindingStatus(int value, string name) : base(value, name) { }
}