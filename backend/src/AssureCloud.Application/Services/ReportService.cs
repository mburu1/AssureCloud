using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Application.Commands.Reports;
using AssureCloud.Application.DTOs;
using AssureCloud.Application.Queries.Reports;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AssureCloud.Application.Services.Reports;

public class ReportService :
    ICommandHandler<CreateReportCommand, ReportDto>,
    ICommandHandler<UpdateReportCommand, ReportDto>,
    ICommandHandler<DeleteReportCommand>,
    ICommandHandler<UpdateReportStatusCommand>,
    ICommandHandler<GenerateReportCommand>,
    ICommandHandler<SetReportGeneratedCommand>,
    ICommandHandler<SetReportFailedCommand>,
    ICommandHandler<SetReportPeriodCommand>,
    IQueryHandler<GetReportByIdQuery, ReportDto?>,
    IQueryHandler<GetReportsQuery, PaginatedResult<ReportListDto>>,
    IQueryHandler<GetReportSummaryQuery, ReportSummaryDto?>,
    IQueryHandler<GetReportsByOrganizationQuery, IReadOnlyList<ReportListDto>>,
    IQueryHandler<GetReportsByTypeQuery, IReadOnlyList<ReportListDto>>,
    IQueryHandler<GetReportsByStatusQuery, IReadOnlyList<ReportListDto>>
{
    private readonly IRepository<Report> _reportRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ReportService(
        IRepository<Report> reportRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _reportRepository = reportRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReportDto> HandleAsync(CreateReportCommand command, CancellationToken ct = default)
    {
        var report = new Report(
            command.OrganizationId,
            command.Title,
            Enum.Parse<ReportType>(command.Type),
            command.ProgramId,
            command.AssessmentId,
            command.AuditId,
            command.CertificationId,
            command.Description);

        if (command.PeriodStart.HasValue || command.PeriodEnd.HasValue)
        {
            report.SetPeriod(command.PeriodStart ?? DateTime.MinValue, command.PeriodEnd ?? DateTime.MaxValue);
        }

        if (!string.IsNullOrEmpty(command.Parameters))
        {
            report.UpdateDetails(report.Title, report.Description, command.Parameters);
        }

        await _reportRepository.AddAsync(report, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<ReportDto>(report);
    }

    public async Task<ReportDto> HandleAsync(UpdateReportCommand command, CancellationToken ct = default)
    {
        var report = await _reportRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Report));

        report.UpdateDetails(command.Title, command.Description, command.Parameters);
        await _reportRepository.UpdateAsync(report, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<ReportDto>(report);
    }

    public async Task HandleAsync(DeleteReportCommand command, CancellationToken ct = default)
    {
        var report = await _reportRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Report));

        report.MarkAsDeleted();
        await _reportRepository.UpdateAsync(report, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateReportStatusCommand command, CancellationToken ct = default)
    {
        var report = await _reportRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Report));

        if (Enum.TryParse<ReportStatus>(command.Status, out var status))
        {
            report.UpdateStatus(status);
            await _reportRepository.UpdateAsync(report, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task HandleAsync(GenerateReportCommand command, CancellationToken ct = default)
    {
        var report = await _reportRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Report));

        report.UpdateStatus(ReportStatus.Generating);
        await _reportRepository.UpdateAsync(report, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetReportGeneratedCommand command, CancellationToken ct = default)
    {
        var report = await _reportRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Report));

        report.SetGenerated(command.UserId, command.FileUrl, command.MimeType, command.FileSize);
        await _reportRepository.UpdateAsync(report, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetReportFailedCommand command, CancellationToken ct = default)
    {
        var report = await _reportRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Report));

        report.UpdateStatus(ReportStatus.Failed);
        await _reportRepository.UpdateAsync(report, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetReportPeriodCommand command, CancellationToken ct = default)
    {
        var report = await _reportRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Report));

        report.SetPeriod(command.PeriodStart, command.PeriodEnd);
        await _reportRepository.UpdateAsync(report, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<ReportDto?> HandleAsync(GetReportByIdQuery query, CancellationToken ct = default)
    {
        var report = await _reportRepository.GetByIdAsync(query.Id, ct);
        return report != null ? _mapper.Map<ReportDto>(report) : null;
    }

    public async Task<PaginatedResult<ReportListDto>> HandleAsync(GetReportsQuery query, CancellationToken ct = default)
    {
        var reports = await _reportRepository.ListAsync(ct);

        var filtered = reports.AsQueryable();

        if (query.OrganizationId.HasValue)
        {
            filtered = filtered.Where(r => r.OrganizationId == query.OrganizationId.Value);
        }

        if (query.ProgramId.HasValue)
        {
            filtered = filtered.Where(r => r.ProgramId == query.ProgramId.Value);
        }

        if (query.AssessmentId.HasValue)
        {
            filtered = filtered.Where(r => r.AssessmentId == query.AssessmentId.Value);
        }

        if (query.AuditId.HasValue)
        {
            filtered = filtered.Where(r => r.AuditId == query.AuditId.Value);
        }

        if (query.CertificationId.HasValue)
        {
            filtered = filtered.Where(r => r.CertificationId == query.CertificationId.Value);
        }

        if (!string.IsNullOrEmpty(query.Type) && Enum.TryParse<ReportType>(query.Type, out var type))
        {
            filtered = filtered.Where(r => r.Type == type);
        }

        if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<ReportStatus>(query.Status, out var status))
        {
            filtered = filtered.Where(r => r.Status == status);
        }

        var totalCount = filtered.Count();
        var items = filtered
            .OrderByDescending(r => r.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<ReportListDto>>(items);

        return new PaginatedResult<ReportListDto>
        {
            Items = dtos,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<ReportSummaryDto?> HandleAsync(GetReportSummaryQuery query, CancellationToken ct = default)
    {
        var report = await _reportRepository.GetByIdAsync(query.Id, ct);
        return report != null ? _mapper.Map<ReportSummaryDto>(report) : null;
    }

    public async Task<IReadOnlyList<ReportListDto>> HandleAsync(GetReportsByOrganizationQuery query, CancellationToken ct = default)
    {
        var reports = await _reportRepository.ListAsync(ct);
        var filtered = reports.Where(r => r.OrganizationId == query.OrganizationId).ToList();
        return _mapper.Map<IReadOnlyList<ReportListDto>>(filtered);
    }

    public async Task<IReadOnlyList<ReportListDto>> HandleAsync(GetReportsByTypeQuery query, CancellationToken ct = default)
    {
        var reports = await _reportRepository.ListAsync(ct);
        if (Enum.TryParse<ReportType>(query.Type, out var type))
        {
            var filtered = reports.Where(r => r.Type == type).ToList();
            return _mapper.Map<IReadOnlyList<ReportListDto>>(filtered);
        }
        return Array.Empty<ReportListDto>();
    }

    public async Task<IReadOnlyList<ReportListDto>> HandleAsync(GetReportsByStatusQuery query, CancellationToken ct = default)
    {
        var reports = await _reportRepository.ListAsync(ct);
        if (Enum.TryParse<ReportStatus>(query.Status, out var status))
        {
            var filtered = reports.Where(r => r.Status == status).ToList();
            return _mapper.Map<IReadOnlyList<ReportListDto>>(filtered);
        }
        return Array.Empty<ReportListDto>();
    }
}