using System;
using System.Linq.Expressions;
using Ardalis.Specification;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;

namespace AssureCloud.Domain.Specifications;

public static class AuditSpecifications
{
    public static Specification<Audit> ById(Guid id)
    {
        return new Specification<Audit>(x => x.Id == id);
    }

    public static Specification<Audit> ByOrganization(Guid organizationId)
    {
        return new Specification<Audit>(x => x.OrganizationId == organizationId);
    }

    public static Specification<Audit> ByAssessment(Guid assessmentId)
    {
        return new Specification<Audit>(x => x.AssessmentId == assessmentId);
    }

    public static Specification<Audit> ByStatus(AuditStatus status)
    {
        return new Specification<Audit>(x => x.Status == status);
    }

    public static Specification<Audit> ByType(AuditType type)
    {
        return new Specification<Audit>(x => x.Type == type);
    }

    public static Specification<Audit> Active()
    {
        return new Specification<Audit>(x =>
            x.Status == AuditStatus.Planned ||
            x.Status == AuditStatus.InProgress ||
            x.Status == AuditStatus.OnHold);
    }

    public static Specification<Audit> Completed()
    {
        return new Specification<Audit>(x =>
            x.Status == AuditStatus.Completed ||
            x.Status == AuditStatus.Cancelled);
    }

    public static Specification<Audit> ByLeadAuditor(Guid auditorId)
    {
        return new Specification<Audit>(x => x.LeadAuditorId == auditorId);
    }

    public static Specification<Audit> ByAuditor(Guid auditorId)
    {
        return new Specification<Audit>(x =>
            x.LeadAuditorId == auditorId ||
            x.Assignments.Any(a => a.AuditorId == auditorId && a.RemovedAt == null));
    }

    public static Specification<Audit> PlannedBetween(DateTime start, DateTime end)
    {
        return new Specification<Audit>(x => x.PlannedStartDate >= start && x.PlannedEndDate <= end);
    }

    public static Specification<Audit> Overdue()
    {
        return new Specification<Audit>(x =>
            x.PlannedEndDate < DateTime.UtcNow &&
            (x.Status == AuditStatus.Planned || x.Status == AuditStatus.InProgress || x.Status == AuditStatus.OnHold));
    }

    public static Specification<Audit> WithFindings()
    {
        return new Specification<Audit>(x => x.Findings.Any());
    }

    public static Specification<Audit> WithOpenFindings()
    {
        return new Specification<Audit>(x =>
            x.Findings.Any(f => f.Status == FindingStatus.Open || f.Status == FindingStatus.UnderReview));
    }

    public static Specification<Audit> WithOverdueCorrectiveActions()
    {
        return new Specification<Audit>(x =>
            x.CorrectiveActions.Any(ca =>
                ca.Status != CorrectiveActionStatus.Completed &&
                ca.Status != CorrectiveActionStatus.Verified &&
                ca.DueDate < DateTime.UtcNow));
    }
}