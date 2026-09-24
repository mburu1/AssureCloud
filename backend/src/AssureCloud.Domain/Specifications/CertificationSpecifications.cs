using System;
using System.Linq.Expressions;
using Ardalis.Specification;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;

namespace AssureCloud.Domain.Specifications;

public static class CertificationSpecifications
{
    public static Specification<Certification> ById(Guid id)
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x => x.Id == id);
        return spec;
    }

    public static Specification<Certification> ByOrganization(Guid organizationId)
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x => x.OrganizationId == organizationId);
        return spec;
    }

    public static Specification<Certification> ByProgram(Guid programId)
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x => x.ProgramId == programId);
        return spec;
    }

    public static Specification<Certification> ByStatus(CertificationStatus status)
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x => x.Status == status);
        return spec;
    }

    public static Specification<Certification> Active()
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x => x.Status == CertificationStatus.Issued);
        return spec;
    }

    public static Specification<Certification> Expired()
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x =>
            x.ExpirationDate.HasValue && x.ExpirationDate.Value < DateTime.UtcNow);
        return spec;
    }

    public static Specification<Certification> ExpiringSoon(int days = 90)
    {
        var spec = new Specification<Certification>();
        var cutoffDate = DateTime.UtcNow.AddDays(days);
        spec.Query.Where(x =>
            x.ExpirationDate.HasValue &&
            x.ExpirationDate.Value <= cutoffDate &&
            x.ExpirationDate.Value >= DateTime.UtcNow &&
            x.Status == CertificationStatus.Issued);
        return spec;
    }

    public static Specification<Certification> Suspended()
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x => x.Status == CertificationStatus.Suspended);
        return spec;
    }

    public static Specification<Certification> Revoked()
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x => x.Status == CertificationStatus.Revoked);
        return spec;
    }

    public static Specification<Certification> ByCertificateNumber(string certificateNumber)
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x => x.CertificateNumber == certificateNumber);
        return spec;
    }

    public static Specification<Certification> SurveillanceDue()
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x =>
            x.NextSurveillanceDue.HasValue &&
            x.NextSurveillanceDue.Value <= DateTime.UtcNow &&
            x.Status == CertificationStatus.Issued);
        return spec;
    }

    public static Specification<Certification> WithDecisions()
    {
        var spec = new Specification<Certification>();
        spec.Query.Where(x => x.Decisions.Any());
        return spec;
    }
}