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
        return new Specification<Assessment>(x => x.Id == id);
    }

    public static Specification<Assessment> ByOrganization(Guid organizationId)
    {
        return new Specification<Assessment>(x => x.OrganizationId == organizationId);
    }

    public static Specification<Assessment> ByProgram(Guid programId)
    {
        return new Specification<Assessment>(x => x.ProgramId == programId);
    }

    public static Specification<Assessment> ByStandard(Guid standardId)
    {
        return new Specification<Assessment>(x => x.StandardId == standardId);
    }

    public static Specification<Assessment> ByStatus(AssessmentStatus status)
    {
        return new Specification<Assessment>(x => x.Status == status);
    }

    public static Specification<Assessment> Active()
    {
        return new Specification<Assessment>(x =>
            x.Status == AssessmentStatus.Draft ||
            x.Status == AssessmentStatus.InProgress ||
            x.Status == AssessmentStatus.OnHold);
    }

    public static Specification<Assessment> Completed()
    {
        return new Specification<Assessment>(x => x.Status == AssessmentStatus.Completed);
    }

    public static Specification<Assessment> ByAssessor(Guid assessorId)
    {
        return new Specification<Assessment>(x =>
            x.LeadAssessorId == assessorId ||
            x.Assignments.Any(a => a.AssessorId == assessorId && a.RemovedAt == null));
    }

    public static Specification<Assessment> ByReviewer(Guid reviewerId)
    {
        return new Specification<Assessment>(x => x.ReviewerId == reviewerId);
    }

    public static Specification<Assessment> ScheduledBetween(DateTime start, DateTime end)
    {
        return new Specification<Assessment>(x => x.ScheduledDate >= start && x.ScheduledDate <= end);
    }

    public static Specification<Assessment> Overdue()
    {
        return new Specification<Assessment>(x =>
            x.ScheduledDate < DateTime.UtcNow &&
            (x.Status == AssessmentStatus.Draft || x.Status == AssessmentStatus.InProgress || x.Status == AssessmentStatus.OnHold));
    }

    public static Specification<Assessment> WithFindings()
    {
        return new Specification<Assessment>(x => x.Findings.Any());
    }

    public static Specification<Assessment> WithOpenFindings()
    {
        return new Specification<Assessment>(x =>
            x.Findings.Any(f => f.Status == FindingStatus.Open || f.Status == FindingStatus.UnderReview));
    }
}