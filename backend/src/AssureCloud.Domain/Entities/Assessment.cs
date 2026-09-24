using System;
using System.Collections.Generic;
using AssureCloud.Domain.Events;
using AssureCloud.Domain.Enums;

namespace AssureCloud.Domain.Entities;

public class Assessment : BaseEntity
{
    private readonly List<AssessmentResponse> _responses = new();
    private readonly List<Evidence> _evidence = new();
    private readonly List<Finding> _findings = new();
    private readonly List<AssessmentAssignment> _assignments = new();

    public Guid ProgramId { get; private set; }
    public Guid OrganizationId { get; private set; }
    public Guid StandardId { get; private set; }
    public string? Title { get; private set; }
    public string? Description { get; private set; }
    public DateTime ScheduledDate { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? CompletedDate { get; private set; }
    public AssessmentStatus Status { get; private set; } = AssessmentStatus.Draft;
    public decimal? Score { get; private set; }
    public string? AssessorNotes { get; private set; }
    public string? ReviewerNotes { get; private set; }
    public Guid? LeadAssessorId { get; private set; }
    public Guid? ReviewerId { get; private set; }

    public IReadOnlyCollection<AssessmentResponse> Responses => _responses.AsReadOnly();
    public IReadOnlyCollection<Evidence> Evidence => _evidence.AsReadOnly();
    public IReadOnlyCollection<Finding> Findings => _findings.AsReadOnly();
    public IReadOnlyCollection<AssessmentAssignment> Assignments => _assignments.AsReadOnly();

    private Assessment() { }

    public Assessment(
        Guid programId,
        Guid organizationId,
        Guid standardId,
        DateTime scheduledDate,
        string? notes = null)
    {
        ProgramId = programId;
        OrganizationId = organizationId;
        StandardId = standardId;
        ScheduledDate = scheduledDate;
        AssessorNotes = notes;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string? title, string? description, DateTime? scheduledDate, string? assessorNotes)
    {
        Title = title;
        Description = description;
        if (scheduledDate.HasValue) ScheduledDate = scheduledDate.Value;
        AssessorNotes = assessorNotes;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(AssessmentStatus status)
    {
        var oldStatus = Status;
        Status = status;

        if (status == AssessmentStatus.InProgress && StartDate == null)
        {
            StartDate = DateTime.UtcNow;
        }
        else if (status == AssessmentStatus.Completed && CompletedDate == null)
        {
            CompletedDate = DateTime.UtcNow;
        }

        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new AssessmentStatusChangedEvent(Id, oldStatus.ToString(), status.ToString()));
    }

    public void SetScore(decimal score)
    {
        Score = score;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetLeadAssessor(Guid assessorId)
    {
        LeadAssessorId = assessorId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetReviewer(Guid reviewerId)
    {
        ReviewerId = reviewerId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetReviewerNotes(string notes)
    {
        ReviewerNotes = notes;
        UpdatedAt = DateTime.UtcNow;
    }

    public AssessmentAssignment AssignAssessor(Guid assessorId, AssessmentRole role)
    {
        var assignment = new AssessmentAssignment(Id, assessorId, role);
        _assignments.Add(assignment);
        AddDomainEvent(new AssessorAssignedEvent(Id, assessorId, role.ToString()));
        return assignment;
    }

    public void RemoveAssessor(Guid assessorId)
    {
        var assignment = _assignments.FirstOrDefault(a => a.AssessorId == assessorId);
        if (assignment != null)
        {
            _assignments.Remove(assignment);
            AddDomainEvent(new AssessorRemovedEvent(Id, assessorId));
        }
    }

    public AssessmentResponse AddResponse(Guid criterionId, decimal score, string? comments = null, bool isManual = false)
    {
        var response = new AssessmentResponse(Id, criterionId, score, comments, isManual);
        _responses.Add(response);
        return response;
    }

    public Evidence AddEvidence(string title, string? description, string fileUrl, string? mimeType = null, long? fileSize = null)
    {
        var evidence = new Evidence(Id, title, description, fileUrl, mimeType, fileSize);
        _evidence.Add(evidence);
        AddDomainEvent(new EvidenceAddedEvent(Id, evidence.Id));
        return evidence;
    }

    public void RemoveEvidence(Guid evidenceId)
    {
        var evidence = _evidence.FirstOrDefault(e => e.Id == evidenceId);
        if (evidence != null)
        {
            _evidence.Remove(evidence);
            AddDomainEvent(new EvidenceRemovedEvent(Id, evidenceId));
        }
    }

    public Finding AddFinding(string title, string description, FindingSeverity severity, FindingCategory category, Guid? criterionId = null)
    {
        var finding = new Finding(Id, title, description, severity, category, criterionId);
        _findings.Add(finding);
        AddDomainEvent(new FindingCreatedEvent(Id, finding.Id));
        return finding;
    }

    public void RemoveFinding(Guid findingId)
    {
        var finding = _findings.FirstOrDefault(f => f.Id == findingId);
        if (finding != null)
        {
            _findings.Remove(finding);
            AddDomainEvent(new FindingRemovedEvent(Id, findingId));
        }
    }

    public bool IsCompleted => Status == AssessmentStatus.Completed || Status == AssessmentStatus.Cancelled;
    public bool IsInProgress => Status == AssessmentStatus.InProgress;
    public bool CanStart => Status == AssessmentStatus.Draft || Status == AssessmentStatus.OnHold;
}

public class AssessmentAssignment : BaseEntity
{
    public Guid AssessmentId { get; private set; }
    public Guid AssessorId { get; private set; }
    public AssessmentRole? Role { get; private set; }
    public DateTime AssignedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? RemovedAt { get; private set; }

    private AssessmentAssignment() { }

    public AssessmentAssignment(Guid assessmentId, Guid assessorId, AssessmentRole role)
    {
        AssessmentId = assessmentId;
        AssessorId = assessorId;
        Role = role;
        CreatedAt = DateTime.UtcNow;
    }

    public void Remove()
    {
        RemovedAt = DateTime.UtcNow;
    }
}

public class AssessmentRole : Enumeration
{
    public static readonly AssessmentRole LeadAssessor = new(0, "LeadAssessor");
    public static readonly AssessmentRole Assessor = new(1, "Assessor");
    public static readonly AssessmentRole Observer = new(2, "Observer");
    public static readonly AssessmentRole TechnicalExpert = new(3, "TechnicalExpert");

    private AssessmentRole(int value, string name) : base(value, name) { }
}