using System;
using AssureCloud.Application.Commands;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Commands.Reports;

public record CreateReportCommand(
    Guid OrganizationId,
    string Title,
    string Type,
    Guid? ProgramId = null,
    Guid? AssessmentId = null,
    Guid? AuditId = null,
    Guid? CertificationId = null,
    string? Description = null,
    DateTime? PeriodStart = null,
    DateTime? PeriodEnd = null,
    string? Parameters = null) : ICommand<ReportDto>;

public record UpdateReportCommand(
    Guid Id,
    string Title,
    string? Description = null,
    string? Parameters = null) : ICommand<ReportDto>;

public record DeleteReportCommand(Guid Id) : ICommand;

public record UpdateReportStatusCommand(Guid Id, string Status) : ICommand;

public record GenerateReportCommand(Guid Id, string? Parameters = null) : ICommand;

public record SetReportGeneratedCommand(Guid Id, Guid UserId, string FileUrl, string? MimeType = null, long? FileSize = null) : ICommand;

public record SetReportFailedCommand(Guid Id, string Error) : ICommand;

public record SetReportPeriodCommand(Guid Id, DateTime PeriodStart, DateTime PeriodEnd) : ICommand;