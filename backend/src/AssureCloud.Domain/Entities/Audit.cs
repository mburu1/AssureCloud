using System;
using System.Collections.Generic;
using AssureCloud.Domain.Events;
using AssureCloud.Domain.Enums;

namespace AssureCloud.Domain.Entities;

public class Audit : BaseEntity
{
    private readonly List<AuditFinding> _findings = new();
    private readonly List<AuditAssignment> _assignments = new();
    private readonly List<CorrectiveAction> _correctiveActions = new();

    public Guid OrganizationId { get; private set; }
    public Guid? AssessmentId { get; private set; }
    public string? Title { get; private set; }
    public string? Description { get; private set; }
    public AuditType? Type { get; private set; }
    public AuditStatus Status { get; private set; } = AuditStatus.Planned;
    public DateTime PlannedStartDate { get; private set; }
    public DateTime PlannedEndDate { get; private set; }
    public DateTime? ActualStartDate { get; private set; }
    public DateTime? ActualEndDate { get; private set; }
    public Guid? LeadAuditorId { get; private set; }
    public string? Scope { get; private set; }
    public string? Criteria { get; private set; }
    public string? ReportUrl { get; private set; }
    public decimal? OverallScore { get; private set; }

    public IReadOnlyCollection<AuditFinding> Findings => _findings.AsReadOnly();
    public IReadOnlyCollection<AuditAssignment> Assignments => _assignments.AsReadOnly();
    public IReadOnlyCollection<CorrectiveAction> CorrectiveActions => _correctiveActions.AsReadOnly();

    private Audit() { }

    public Audit(
        Guid organizationId,
        AuditType type,
        DateTime plannedStartDate,
        DateTime plannedEndDate,
        Guid? assessmentId = null,
        string? title = null,
        string? description = null)
    {
        OrganizationId = organizationId;
        Type = type;
        PlannedStartDate = plannedStartDate;
        PlannedEndDate = plannedEndDate;
        AssessmentId = assessmentId;
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(
        string? title,
        string? description,
        AuditType? type = null,
        DateTime? plannedStartDate = null,
        DateTime? plannedEndDate = null,
        string? scope = null,
        string? criteria = null)
    {
        Title = title;
        Description = description;
        if (type != null) Type = type;
        if (plannedStartDate.HasValue) PlannedStartDate = plannedStartDate.Value;
        if (plannedEndDate.HasValue) PlannedEndDate = plannedEndDate.Value;
        Scope = scope;
        Criteria = criteria;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(AuditStatus status)
    {
        var oldStatus = Status;
        Status = status;

        if (status == AuditStatus.InProgress && ActualStartDate == null)
        {
            ActualStartDate = DateTime.UtcNow;
        }
        else if ((status == AuditStatus.Completed || status == AuditStatus.Cancelled) && ActualEndDate == null)
        {
            ActualEndDate = DateTime.UtcNow;
        }

        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AuditStatusChangedEvent(Id, oldStatus.ToString(), status.ToString()));
    }

    public void SetLeadAuditor(Guid auditorId)
    {
        LeadAuditorId = auditorId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetOverallScore(decimal score)
    {
        OverallScore = score;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetReportUrl(string url)
    {
        ReportUrl = url;
        UpdatedAt = DateTime.UtcNow;
    }

    public AuditAssignment AssignAuditor(Guid auditorId, AuditRole role)
    {
        var assignment = new AuditAssignment(Id, auditorId, role);
        _assignments.Add(assignment);
        AddDomainEvent(new AuditorAssignedEvent(Id, auditorId, role.ToString()));
        return assignment;
    }

    public void RemoveAuditor(Guid auditorId)
    {
        var assignment = _assignments.FirstOrDefault(a => a.AuditorId == auditorId);
        if (assignment != null)
        {
            _assignments.Remove(assignment);
            AddDomainEvent(new AuditorRemovedEvent(Id, auditorId));
        }
    }

    public AuditFinding AddFinding(
        string title,
        string description,
        FindingSeverity severity,
        FindingCategory category,
        Guid? criterionId = null,
        string? requirement = null)
    {
        var finding = new AuditFinding(Id, title, description, severity, category, criterionId, requirement);
        _findings.Add(finding);
        AddDomainEvent(new AuditFindingCreatedEvent(Id, finding.Id));
        return finding;
    }

    public void RemoveFinding(Guid findingId)
    {
        var finding = _findings.FirstOrDefault(f => f.Id == findingId);
        if (finding != null)
        {
            _findings.Remove(finding);
            AddDomainEvent(new AuditFindingRemovedEvent(Id, findingId));
        }
    }

    public CorrectiveAction AddCorrectiveAction(
        string title,
        string description,
        Guid findingId,
        Guid responsiblePartyId,
        DateTime dueDate,
        string? rootCause = null)
    {
        var correctiveAction = new CorrectiveAction(Id, findingId, title, description, responsiblePartyId, dueDate, rootCause);
        _correctiveActions.Add(correctiveAction);
        AddDomainEvent(new CorrectiveActionCreatedEvent(Id, correctiveAction.Id));
        return correctiveAction;
    }

    public bool IsCompleted => Status == AuditStatus.Completed || Status == AuditStatus.Cancelled;
    public bool IsInProgress => Status == AuditStatus.InProgress;
    public bool IsOverdue => Status != AuditStatus.Completed && Status != AuditStatus.Cancelled && DateTime.UtcNow > PlannedEndDate;
}

public class AuditAssignment : BaseEntity
{
    public Guid AuditId { get; private set; }
    public Guid AuditorId { get; private set; }
    public AuditRole? Role { get; private set; }
    public DateTime AssignedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? RemovedAt { get; private set; }

    private AuditAssignment() { }

    public AuditAssignment(Guid auditId, Guid auditorId, AuditRole role)
    {
        AuditId = auditId;
        AuditorId = auditorId;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public void Remove()
    {
        RemovedAt = DateTime.UtcNow;
    }
}

public class AuditRole : Enumeration
{
    public static readonly AuditRole LeadAuditor = new(0, "LeadAuditor");
    public static readonly AuditRole Auditor = new(1, "Auditor");
    public static readonly AuditRole TechnicalExpert = new(2, "TechnicalExpert");
    public static readonly AuditRole Observer = new(3, "Observer");
    public static readonly AuditRole Trainee = new(4, "Trainee");

    private AuditRole(int value, string name) : base(value, name) { }
}

public class AuditType : Enumeration
{
    public static readonly AuditType Internal = new(0, "Internal");
    public static readonly AuditType External = new(1, "External");
    public static readonly AuditType Supplier = new(2, "Supplier");
    public static readonly AuditType Surveillance = new(3, "Surveillance");
    public static readonly AuditType Recertification = new(4, "Recertification");
    public static readonly AuditType Special = new(5, "Special");
    public static readonly AuditType FollowUp = new(6, "FollowUp");

    private AuditType(int value, string name) : base(value, name) { }
}