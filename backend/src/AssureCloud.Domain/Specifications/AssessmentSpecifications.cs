using System;
using System.Linq.Expressions;
using Ardalis.Specification;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;

namespace AssureCloud.Domain.Specifications;

public static class AssessmentSpecifications
{
    public static Specification<Assessment> ById(Guid id)
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x => x.Id == id);
        return spec;
    }

    public static Specification<Assessment> ByOrganization(Guid organizationId)
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x => x.OrganizationId == organizationId);
        return spec;
    }

    public static Specification<Assessment> ByProgram(Guid programId)
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x => x.ProgramId == programId);
        return spec;
    }

    public static Specification<Assessment> ByStandard(Guid standardId)
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x => x.StandardId == standardId);
        return spec;
    }

    public static Specification<Assessment> ByStatus(AssessmentStatus status)
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x => x.Status == status);
        return spec;
    }

    public static Specification<Assessment> Active()
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x =>
            x.Status == AssessmentStatus.Draft ||
            x.Status == AssessmentStatus.InProgress ||
            x.Status == AssessmentStatus.OnHold);
        return spec;
    }

    public static Specification<Assessment> Completed()
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x => x.Status == AssessmentStatus.Completed);
        return spec;
    }

    public static Specification<Assessment> ByAssessor(Guid assessorId)
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x =>
            x.LeadAssessorId == assessorId ||
            x.Assignments.Any(a => a.AssessorId == assessorId && a.RemovedAt == null));
        return spec;
    }

    public static Specification<Assessment> ByReviewer(Guid reviewerId)
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x => x.ReviewerId == reviewerId);
        return spec;
    }

    public static Specification<Assessment> ScheduledBetween(DateTime start, DateTime end)
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x => x.ScheduledDate >= start && x.ScheduledDate <= end);
        return spec;
    }

    public static Specification<Assessment> Overdue()
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x =>
            x.ScheduledDate < DateTime.UtcNow &&
            (x.Status == AssessmentStatus.Draft || x.Status == AssessmentStatus.InProgress || x.Status == AssessmentStatus.OnHold));
        return spec;
    }

    public static Specification<Assessment> WithFindings()
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x => x.Findings.Any());
        return spec;
    }

    public static Specification<Assessment> WithOpenFindings()
    {
        var spec = new Specification<Assessment>();
        spec.Query.Where(x =>
            x.Findings.Any(f => f.Status == FindingStatus.Open || f.Status == FindingStatus.UnderReview));
        return spec;
    }
}