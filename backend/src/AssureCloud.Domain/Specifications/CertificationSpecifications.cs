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
        return new Specification<Certification>(x => x.Id == id);
    }

    public static Specification<Certification> ByOrganization(Guid organizationId)
    {
        return new Specification<Certification>(x => x.OrganizationId == organizationId);
    }

    public static Specification<Certification> ByProgram(Guid programId)
    {
        return new Specification<Certification>(x => x.ProgramId == programId);
    }

    public static Specification<Certification> ByStatus(CertificationStatus status)
    {
        return new Specification<Certification>(x => x.Status == status);
    }

    public static Specification<Certification> Active()
    {
        return new Specification<Certification>(x => x.Status == CertificationStatus.Issued);
    }

    public static Specification<Certification> Expired()
    {
        return new Specification<Certification>(x =>
            x.ExpirationDate.HasValue && x.ExpirationDate.Value < DateTime.UtcNow);
    }

    public static Specification<Certification> ExpiringSoon(int days = 90)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(days);
        return new Specification<Certification>(x =>
            x.ExpirationDate.HasValue &&
            x.ExpirationDate.Value <= cutoffDate &&
            x.ExpirationDate.Value >= DateTime.UtcNow &&
            x.Status == CertificationStatus.Issued);
    }

    public static Specification<Certification> Suspended()
    {
        return new Specification<Certification>(x => x.Status == CertificationStatus.Suspended);
    }

    public static Specification<Certification> Revoked()
    {
        return new Specification<Certification>(x => x.Status == CertificationStatus.Revoked);
    }

    public static Specification<Certification> ByCertificateNumber(string certificateNumber)
    {
        return new Specification<Certification>(x => x.CertificateNumber == certificateNumber);
    }

    public static Specification<Certification> SurveillanceDue()
    {
        return new Specification<Certification>(x =>
            x.NextSurveillanceDue.HasValue &&
            x.NextSurveillanceDue.Value <= DateTime.UtcNow &&
            x.Status == CertificationStatus.Issued);
    }

    public static Specification<Certification> WithDecisions()
    {
        return new Specification<Certification>(x => x.Decisions.Any());
    }
}