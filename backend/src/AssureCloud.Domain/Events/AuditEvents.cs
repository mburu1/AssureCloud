using System;

namespace AssureCloud.Domain.Events;

public sealed class AuditCreatedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public Guid AuditId { get; }

    public AuditCreatedEvent(Guid organizationId, Guid auditId)
    {
        OrganizationId = organizationId;
        AuditId = auditId;
    }
}

public sealed class AuditStatusChangedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public string OldStatus { get; }
    public string NewStatus { get; }

    public AuditStatusChangedEvent(Guid auditId, string oldStatus, string newStatus)
    {
        AuditId = auditId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}

public sealed class AuditCompletedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public decimal? Score { get; }

    public AuditCompletedEvent(Guid auditId, decimal? score)
    {
        AuditId = auditId;
        Score = score;
    }
}

public sealed class AuditorAssignedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public Guid AuditorId { get; }
    public string Role { get; }

    public AuditorAssignedEvent(Guid auditId, Guid auditorId, string role)
    {
        AuditId = auditId;
        AuditorId = auditorId;
        Role = role;
    }
}

public sealed class AuditorRemovedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public Guid AuditorId { get; }

    public AuditorRemovedEvent(Guid auditId, Guid auditorId)
    {
        AuditId = auditId;
        AuditorId = auditorId;
    }
}

public sealed class AuditFindingCreatedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public Guid FindingId { get; }

    public AuditFindingCreatedEvent(Guid auditId, Guid findingId)
    {
        AuditId = auditId;
        FindingId = findingId;
    }
}

public sealed class AuditFindingRemovedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public Guid FindingId { get; }

    public AuditFindingRemovedEvent(Guid auditId, Guid findingId)
    {
        AuditId = auditId;
        FindingId = findingId;
    }
}

public sealed class AuditFindingResolvedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public Guid FindingId { get; }

    public AuditFindingResolvedEvent(Guid auditId, Guid findingId)
    {
        AuditId = auditId;
        FindingId = findingId;
    }
}

public sealed class CorrectiveActionCreatedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public Guid CorrectiveActionId { get; }

    public CorrectiveActionCreatedEvent(Guid auditId, Guid correctiveActionId)
    {
        AuditId = auditId;
        CorrectiveActionId = correctiveActionId;
    }
}

public sealed class CorrectiveActionCompletedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public Guid CorrectiveActionId { get; }

    public CorrectiveActionCompletedEvent(Guid auditId, Guid correctiveActionId)
    {
        AuditId = auditId;
        CorrectiveActionId = correctiveActionId;
    }
}

public sealed class CorrectiveActionVerifiedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public Guid CorrectiveActionId { get; }

    public CorrectiveActionVerifiedEvent(Guid auditId, Guid correctiveActionId)
    {
        AuditId = auditId;
        CorrectiveActionId = correctiveActionId;
    }
}

public sealed class AuditReportGeneratedEvent : DomainEvent
{
    public Guid AuditId { get; }
    public string ReportUrl { get; }

    public AuditReportGeneratedEvent(Guid auditId, string reportUrl)
    {
        AuditId = auditId;
        ReportUrl = reportUrl;
    }
}