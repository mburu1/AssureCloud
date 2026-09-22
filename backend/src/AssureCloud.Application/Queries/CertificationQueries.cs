using System;
using AssureCloud.Application.Queries;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Queries.Certifications;

public record GetCertificationByIdQuery(Guid Id) : IQuery<CertificationDto?>;

public record GetCertificationsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    Guid? OrganizationId = null,
    Guid? ProgramId = null,
    string? Status = null,
    bool? ExpiringSoon = null) : IQuery<PaginatedResult<CertificationListDto>>;

public record GetCertificationSummaryQuery(Guid Id) : IQuery<CertificationSummaryDto?>;

public record GetCertificationDecisionsQuery(Guid CertificationId) : IQuery<IReadOnlyList<CertificationDecisionDto>>;

public record GetCertificationDecisionByIdQuery(Guid CertificationId, Guid DecisionId) : IQuery<CertificationDecisionDto?>;

public record GetCertificationScopesQuery(Guid CertificationId) : IQuery<IReadOnlyList<CertificationScopeDto>>;

public record GetCertificationScopeByIdQuery(Guid CertificationId, Guid ScopeId) : IQuery<CertificationScopeDto?>;

public record GetCertificationsByOrganizationQuery(Guid OrganizationId) : IQuery<IReadOnlyList<CertificationListDto>>;

public record GetActiveCertificationsQuery() : IQuery<IReadOnlyList<CertificationListDto>>;

public record GetExpiringCertificationsQuery(int Days = 90) : IQuery<IReadOnlyList<CertificationListDto>>;

public record GetExpiredCertificationsQuery() : IQuery<IReadOnlyList<CertificationListDto>>;

public record GetSurveillanceDueCertificationsQuery() : IQuery<IReadOnlyList<CertificationListDto>>;

public record GetSuspendedCertificationsQuery() : IQuery<IReadOnlyList<CertificationListDto>>;

public record GetRevokedCertificationsQuery() : IQuery<IReadOnlyList<CertificationListDto>>;

public record GetCertificationByCertificateNumberQuery(string CertificateNumber) : IQuery<CertificationDto?>;