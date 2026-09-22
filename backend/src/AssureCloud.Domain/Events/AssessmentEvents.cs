using System;

namespace AssureCloud.Domain.Events;

public sealed class AssessmentCreatedEvent : DomainEvent
{
    public Guid ProgramId { get; }
    public Guid AssessmentId { get; }

    public AssessmentCreatedEvent(Guid programId, Guid assessmentId)
    {
        ProgramId = programId;
        AssessmentId = assessmentId;
    }
}

public sealed class AssessmentStatusChangedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public string OldStatus { get; }
    public string NewStatus { get; }

    public AssessmentStatusChangedEvent(Guid assessmentId, string oldStatus, string newStatus)
    {
        AssessmentId = assessmentId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}

public sealed class AssessmentCompletedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public decimal Score { get; }

    public AssessmentCompletedEvent(Guid assessmentId, decimal score)
    {
        AssessmentId = assessmentId;
        Score = score;
    }
}

public sealed class AssessorAssignedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public Guid AssessorId { get; }
    public string Role { get; }

    public AssessorAssignedEvent(Guid assessmentId, Guid assessorId, string role)
    {
        AssessmentId = assessmentId;
        AssessorId = assessorId;
        Role = role;
    }
}

public sealed class AssessorRemovedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public Guid AssessorId { get; }

    public AssessorRemovedEvent(Guid assessmentId, Guid assessorId)
    {
        AssessmentId = assessmentId;
        AssessorId = assessorId;
    }
}

public sealed class EvidenceAddedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public Guid EvidenceId { get; }

    public EvidenceAddedEvent(Guid assessmentId, Guid evidenceId)
    {
        AssessmentId = assessmentId;
        EvidenceId = evidenceId;
    }
}

public sealed class EvidenceRemovedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public Guid EvidenceId { get; }

    public EvidenceRemovedEvent(Guid assessmentId, Guid evidenceId)
    {
        AssessmentId = assessmentId;
        EvidenceId = evidenceId;
    }
}

public sealed class FindingCreatedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public Guid FindingId { get; }

    public FindingCreatedEvent(Guid assessmentId, Guid findingId)
    {
        AssessmentId = assessmentId;
        FindingId = findingId;
    }
}

public sealed class FindingRemovedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public Guid FindingId { get; }

    public FindingRemovedEvent(Guid assessmentId, Guid findingId)
    {
        AssessmentId = assessmentId;
        FindingId = findingId;
    }
}

public sealed class FindingResolvedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public Guid FindingId { get; }

    public FindingResolvedEvent(Guid assessmentId, Guid findingId)
    {
        AssessmentId = assessmentId;
        FindingId = findingId;
    }
}

public sealed class AssessmentSubmittedEvent : DomainEvent
{
    public Guid AssessmentId { get; }
    public Guid OrganizationId { get; }
    public Guid SubmittedById { get; }

    public AssessmentSubmittedEvent(Guid assessmentId, Guid organizationId, Guid submittedById)
    {
        AssessmentId = assessmentId;
        OrganizationId = organizationId;
        SubmittedById = submittedById;
    }
}