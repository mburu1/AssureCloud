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
        var spec = new Specification<Audit>();
        spec.Query.Where(x => x.Id == id);
        return spec;
    }

    public static Specification<Audit> ByOrganization(Guid organizationId)
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x => x.OrganizationId == organizationId);
        return spec;
    }

    public static Specification<Audit> ByAssessment(Guid assessmentId)
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x => x.AssessmentId == assessmentId);
        return spec;
    }

    public static Specification<Audit> ByStatus(AuditStatus status)
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x => x.Status == status);
        return spec;
    }

    public static Specification<Audit> ByType(AuditType type)
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x => x.Type == type);
        return spec;
    }

    public static Specification<Audit> Active()
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x =>
            x.Status == AuditStatus.Planned ||
            x.Status == AuditStatus.InProgress ||
            x.Status == AuditStatus.OnHold);
        return spec;
    }

    public static Specification<Audit> Completed()
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x =>
            x.Status == AuditStatus.Completed ||
            x.Status == AuditStatus.Cancelled);
        return spec;
    }

    public static Specification<Audit> ByLeadAuditor(Guid auditorId)
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x => x.LeadAuditorId == auditorId);
        return spec;
    }

    public static Specification<Audit> ByAuditor(Guid auditorId)
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x =>
            x.LeadAuditorId == auditorId ||
            x.Assignments.Any(a => a.AuditorId == auditorId && a.RemovedAt == null));
        return spec;
    }

    public static Specification<Audit> PlannedBetween(DateTime start, DateTime end)
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x => x.PlannedStartDate >= start && x.PlannedEndDate <= end);
        return spec;
    }

    public static Specification<Audit> Overdue()
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x =>
            x.PlannedEndDate < DateTime.UtcNow &&
            (x.Status == AuditStatus.Planned || x.Status == AuditStatus.InProgress || x.Status == AuditStatus.OnHold));
        return spec;
    }

    public static Specification<Audit> WithFindings()
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x => x.Findings.Any());
        return spec;
    }

    public static Specification<Audit> WithOpenFindings()
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x =>
            x.Findings.Any(f => f.Status == FindingStatus.Open || f.Status == FindingStatus.UnderReview));
        return spec;
    }

    public static Specification<Audit> WithOverdueCorrectiveActions()
    {
        var spec = new Specification<Audit>();
        spec.Query.Where(x =>
            x.CorrectiveActions.Any(ca =>
                ca.Status != CorrectiveActionStatus.Completed &&
                ca.Status != CorrectiveActionStatus.Verified &&
                ca.DueDate < DateTime.UtcNow));
        return spec;
    }
}