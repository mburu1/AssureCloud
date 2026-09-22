using System;
using AssureCloud.Application.Queries;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Queries.Assessments;

public record GetAssessmentByIdQuery(Guid Id) : IQuery<AssessmentDto?>;

public record GetAssessmentsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    Guid? OrganizationId = null,
    Guid? ProgramId = null,
    Guid? StandardId = null,
    string? Status = null,
    Guid? AssessorId = null,
    Guid? ReviewerId = null,
    DateTime? ScheduledFrom = null,
    DateTime? ScheduledTo = null) : IQuery<PaginatedResult<AssessmentListDto>>;

public record GetAssessmentSummaryQuery(Guid Id) : IQuery<AssessmentSummaryDto?>;

public record GetAssessmentResponsesQuery(Guid AssessmentId) : IQuery<IReadOnlyList<AssessmentResponseDto>>;

public record GetAssessmentResponseByIdQuery(Guid AssessmentId, Guid ResponseId) : IQuery<AssessmentResponseDto?>;

public record GetAssessmentEvidenceQuery(Guid AssessmentId) : IQuery<IReadOnlyList<EvidenceDto>>;

public record GetEvidenceByIdQuery(Guid AssessmentId, Guid EvidenceId) : IQuery<EvidenceDto?>;

public record GetAssessmentFindingsQuery(Guid AssessmentId) : IQuery<IReadOnlyList<FindingDto>>;

public record GetFindingByIdQuery(Guid AssessmentId, Guid FindingId) : IQuery<FindingDto?>;

public record GetAssessmentAssignmentsQuery(Guid AssessmentId) : IQuery<IReadOnlyList<AssessmentAssignmentDto>>;

public record GetAssessmentsByOrganizationQuery(Guid OrganizationId) : IQuery<IReadOnlyList<AssessmentListDto>>;

public record GetAssessmentsByAssessorQuery(Guid AssessorId) : IQuery<IReadOnlyList<AssessmentListDto>>;

public record GetOverdueAssessmentsQuery() : IQuery<IReadOnlyList<AssessmentListDto>>;