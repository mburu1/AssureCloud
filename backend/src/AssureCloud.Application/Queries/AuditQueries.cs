using System;
using AssureCloud.Application.Queries;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Queries.Audits;

public record GetAuditByIdQuery(Guid Id) : IQuery<AuditDto?>;

public record GetAuditsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    Guid? OrganizationId = null,
    Guid? AssessmentId = null,
    string? Status = null,
    string? Type = null,
    Guid? LeadAuditorId = null,
    Guid? AuditorId = null,
    DateTime? PlannedFrom = null,
    DateTime? PlannedTo = null) : IQuery<PaginatedResult<AuditListDto>>;

public record GetAuditSummaryQuery(Guid Id) : IQuery<AuditSummaryDto?>;

public record GetAuditFindingsQuery(Guid AuditId) : IQuery<IReadOnlyList<AuditFindingDto>>;

public record GetAuditFindingByIdQuery(Guid AuditId, Guid FindingId) : IQuery<AuditFindingDto?>;

public record GetCorrectiveActionsByAuditQuery(Guid AuditId) : IQuery<IReadOnlyList<CorrectiveActionDto>>;

public record GetCorrectiveActionByIdQuery(Guid AuditId, Guid CorrectiveActionId) : IQuery<CorrectiveActionDto?>;

public record GetOverdueCorrectiveActionsQuery(Guid AuditId) : IQuery<IReadOnlyList<CorrectiveActionDto>>;

public record GetAuditAssignmentsQuery(Guid AuditId) : IQuery<IReadOnlyList<AuditAssignmentDto>>;

public record GetAuditsByOrganizationQuery(Guid OrganizationId) : IQuery<IReadOnlyList<AuditListDto>>;

public record GetAuditsByAuditorQuery(Guid AuditorId) : IQuery<IReadOnlyList<AuditListDto>>;

public record GetOverdueAuditsQuery() : IQuery<IReadOnlyList<AuditListDto>>;

public record GetAuditsWithOverdueCorrectiveActionsQuery() : IQuery<IReadOnlyList<AuditListDto>>;