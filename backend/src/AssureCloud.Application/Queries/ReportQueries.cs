using System;
using AssureCloud.Application.Queries;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Queries.Reports;

public record GetReportByIdQuery(Guid Id) : IQuery<ReportDto?>;

public record GetReportsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    Guid? OrganizationId = null,
    Guid? ProgramId = null,
    Guid? AssessmentId = null,
    Guid? AuditId = null,
    Guid? CertificationId = null,
    string? Type = null,
    string? Status = null) : IQuery<PaginatedResult<ReportListDto>>;

public record GetReportSummaryQuery(Guid Id) : IQuery<ReportSummaryDto?>;

public record GetReportsByOrganizationQuery(Guid OrganizationId) : IQuery<IReadOnlyList<ReportListDto>>;

public record GetReportsByTypeQuery(string Type) : IQuery<IReadOnlyList<ReportListDto>>;

public record GetReportsByStatusQuery(string Status) : IQuery<IReadOnlyList<ReportListDto>>;