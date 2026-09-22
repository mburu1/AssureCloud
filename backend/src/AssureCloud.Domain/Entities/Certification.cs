using System;
using System.Collections.Generic;
using AssureCloud.Domain.Events;
using AssureCloud.Domain.Enums;

namespace AssureCloud.Domain.Entities;

public class Certification : BaseEntity
{
    private readonly List<CertificationDecision> _decisions = new();
    private readonly List<CertificationScope> _scopes = new();

    public Guid OrganizationId { get; private set; }
    public Guid ProgramId { get; private set; }
    public Guid? AssessmentId { get; private set; }
    public string? CertificateNumber { get; private set; }
    public string? Title { get; private set; }
    public CertificationStatus Status { get; private set; } = CertificationStatus.NotStarted;
    public DateTime? ApplicationDate { get; private set; }
    public DateTime? IssueDate { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public DateTime? SuspensionDate { get; private set; }
    public DateTime? RevocationDate { get; private set; }
    public string? SuspensionReason { get; private set; }
    public string? RevocationReason { get; private set; }
    public Guid? IssuedById { get; private set; }
    public string? CertificateUrl { get; private set; }
    public int SurveillanceIntervalMonths { get; private set; } = 12;
    public DateTime? NextSurveillanceDue { get; private set; }

    public IReadOnlyCollection<CertificationDecision> Decisions => _decisions.AsReadOnly();
    public IReadOnlyCollection<CertificationScope> Scopes => _scopes.AsReadOnly();

    private Certification() { }

    public Certification(
        Guid organizationId,
        Guid programId,
        Guid? assessmentId = null,
        string? title = null)
    {
        OrganizationId = organizationId;
        ProgramId = programId;
        AssessmentId = assessmentId;
        Title = title;
        ApplicationDate = DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string? title, string? certificateNumber, int? surveillanceIntervalMonths)
    {
        Title = title;
        CertificateNumber = certificateNumber;
        if (surveillanceIntervalMonths.HasValue) SurveillanceIntervalMonths = surveillanceIntervalMonths.Value;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(CertificationStatus status)
    {
        var oldStatus = Status;
        Status = status;

        if (status == CertificationStatus.Issued && IssueDate == null)
        {
            IssueDate = DateTime.UtcNow;
            ExpirationDate = IssueDate.Value.AddYears(3);
            NextSurveillanceDue = IssueDate.Value.AddMonths(SurveillanceIntervalMonths);
        }
        else if (status == CertificationStatus.Suspended && SuspensionDate == null)
        {
            SuspensionDate = DateTime.UtcNow;
        }
        else if (status == CertificationStatus.Revoked && RevocationDate == null)
        {
            RevocationDate = DateTime.UtcNow;
        }

        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CertificationStatusChangedEvent(Id, oldStatus.ToString(), status.ToString()));
    }

    public void SetCertificateNumber(string certificateNumber)
    {
        CertificateNumber = certificateNumber ?? throw new ArgumentNullException(nameof(certificateNumber));
        UpdatedAt = DateTime.UtcNow;
    }

    public void Issue(Guid issuedById, string? certificateUrl = null)
    {
        IssueDate = DateTime.UtcNow;
        ExpirationDate = IssueDate.Value.AddYears(3);
        NextSurveillanceDue = IssueDate.Value.AddMonths(SurveillanceIntervalMonths);
        IssuedById = issuedById;
        CertificateUrl = certificateUrl;
        Status = CertificationStatus.Issued;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CertificationIssuedEvent(Id, CertificateNumber!));
    }

    public void Suspend(string reason)
    {
        SuspensionDate = DateTime.UtcNow;
        SuspensionReason = reason;
        Status = CertificationStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CertificationSuspendedEvent(Id, reason));
    }

    public void Revoke(string reason)
    {
        RevocationDate = DateTime.UtcNow;
        RevocationReason = reason;
        Status = CertificationStatus.Revoked;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CertificationRevokedEvent(Id, reason));
    }

    public void Renew(Guid renewedById, DateTime newExpirationDate)
    {
        ExpirationDate = newExpirationDate;
        NextSurveillanceDue = DateTime.UtcNow.AddMonths(SurveillanceIntervalMonths);
        Status = CertificationStatus.Issued;
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new CertificationRenewedEvent(Id, newExpirationDate));
    }

    public CertificationDecision AddDecision(
        string decision,
        string rationale,
        Guid decidedById,
        CertificationDecisionType type)
    {
        var certDecision = new CertificationDecision(Id, decision, rationale, decidedById, type);
        _decisions.Add(certDecision);
        AddDomainEvent(new CertificationDecisionAddedEvent(Id, certDecision.Id));
        return certDecision;
    }

    public CertificationScope AddScope(string name, string? description, Guid standardId)
    {
        var scope = new CertificationScope(Id, name, description, standardId);
        _scopes.Add(scope);
        return scope;
    }

    public void RemoveScope(Guid scopeId)
    {
        var scope = _scopes.FirstOrDefault(s => s.Id == scopeId);
        if (scope != null)
        {
            _scopes.Remove(scope);
        }
    }

    public void UpdateSurveillanceDue(DateTime dueDate)
    {
        NextSurveillanceDue = dueDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsActive => Status == CertificationStatus.Issued;
    public bool IsExpired => ExpirationDate.HasValue && DateTime.UtcNow > ExpirationDate.Value;
    public bool IsExpiringSoon => ExpirationDate.HasValue && DateTime.UtcNow.AddDays(90) > ExpirationDate.Value;
    public bool IsSuspended => Status == CertificationStatus.Suspended;
}

public class CertificationDecision : BaseEntity
{
    public Guid CertificationId { get; private set; }
    public string Decision { get; private set; } = string.Empty;
    public string Rationale { get; private set; } = string.Empty;
    public Guid DecidedById { get; private set; }
    public DateTime DecidedAt { get; private set; } = DateTime.UtcNow;
    public CertificationDecisionType Type { get; private set; }

    private CertificationDecision() { }

    public CertificationDecision(
        Guid certificationId,
        string decision,
        string rationale,
        Guid decidedById,
        CertificationDecisionType type)
    {
        CertificationId = certificationId;
        Decision = decision ?? throw new ArgumentNullException(nameof(decision));
        Rationale = rationale ?? throw new ArgumentNullException(nameof(rationale));
        DecidedById = decidedById;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }
}

public class CertificationScope : BaseEntity
{
    public Guid CertificationId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid StandardId { get; private set; }

    private CertificationScope() { }

    public CertificationScope(
        Guid certificationId,
        string name,
        string? description,
        Guid standardId)
    {
        CertificationId = certificationId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        StandardId = standardId;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }
}

public class CertificationDecisionType : Enumeration
{
    public static readonly CertificationDecisionType InitialCertification = new(0, "InitialCertification");
    public static readonly CertificationDecisionType Surveillance = new(1, "Surveillance");
    public static readonly CertificationDecisionType Recertification = new(2, "Recertification");
    public static readonly CertificationDecisionType Suspension = new(3, "Suspension");
    public static readonly CertificationDecisionType Revocation = new(4, "Revocation");
    public static readonly CertificationDecisionType ScopeExtension = new(5, "ScopeExtension");
    public static readonly CertificationDecisionType ScopeReduction = new(6, "ScopeReduction");

    private CertificationDecisionType(int value, string name) : base(value, name) { }
}