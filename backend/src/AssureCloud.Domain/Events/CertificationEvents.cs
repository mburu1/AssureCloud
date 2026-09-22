using System;

namespace AssureCloud.Domain.Events;

public sealed class CertificationCreatedEvent : DomainEvent
{
    public Guid OrganizationId { get; }
    public Guid ProgramId { get; }
    public Guid CertificationId { get; }

    public CertificationCreatedEvent(Guid organizationId, Guid programId, Guid certificationId)
    {
        OrganizationId = organizationId;
        ProgramId = programId;
        CertificationId = certificationId;
    }
}

public sealed class CertificationStatusChangedEvent : DomainEvent
{
    public Guid CertificationId { get; }
    public string OldStatus { get; }
    public string NewStatus { get; }

    public CertificationStatusChangedEvent(Guid certificationId, string oldStatus, string newStatus)
    {
        CertificationId = certificationId;
        OldStatus = oldStatus;
        NewStatus = newStatus;
    }
}

public sealed class CertificationIssuedEvent : DomainEvent
{
    public Guid CertificationId { get; }
    public string CertificateNumber { get; }

    public CertificationIssuedEvent(Guid certificationId, string certificateNumber)
    {
        CertificationId = certificationId;
        CertificateNumber = certificateNumber;
    }
}

public sealed class CertificationSuspendedEvent : DomainEvent
{
    public Guid CertificationId { get; }
    public string Reason { get; }

    public CertificationSuspendedEvent(Guid certificationId, string reason)
    {
        CertificationId = certificationId;
        Reason = reason;
    }
}

public sealed class CertificationRevokedEvent : DomainEvent
{
    public Guid CertificationId { get; }
    public string Reason { get; }

    public CertificationRevokedEvent(Guid certificationId, string reason)
    {
        CertificationId = certificationId;
        Reason = reason;
    }
}

public sealed class CertificationRenewedEvent : DomainEvent
{
    public Guid CertificationId { get; }
    public DateTime NewExpirationDate { get; }

    public CertificationRenewedEvent(Guid certificationId, DateTime newExpirationDate)
    {
        CertificationId = certificationId;
        NewExpirationDate = newExpirationDate;
    }
}

public sealed class CertificationDecisionAddedEvent : DomainEvent
{
    public Guid CertificationId { get; }
    public Guid DecisionId { get; }

    public CertificationDecisionAddedEvent(Guid certificationId, Guid decisionId)
    {
        CertificationId = certificationId;
        DecisionId = decisionId;
    }
}

public sealed class CertificationExpiringSoonEvent : DomainEvent
{
    public Guid CertificationId { get; }
    public DateTime ExpirationDate { get; }
    public int DaysUntilExpiration { get; }

    public CertificationExpiringSoonEvent(Guid certificationId, DateTime expirationDate, int daysUntilExpiration)
    {
        CertificationId = certificationId;
        ExpirationDate = expirationDate;
        DaysUntilExpiration = daysUntilExpiration;
    }
}

public sealed class CertificationExpiredEvent : DomainEvent
{
    public Guid CertificationId { get; }
    public DateTime ExpirationDate { get; }

    public CertificationExpiredEvent(Guid certificationId, DateTime expirationDate)
    {
        CertificationId = certificationId;
        ExpirationDate = expirationDate;
    }
}