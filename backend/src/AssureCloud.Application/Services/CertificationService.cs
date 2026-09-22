using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Application.Commands.Certifications;
using AssureCloud.Application.DTOs;
using AssureCloud.Application.Queries.Certifications;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AssureCloud.Application.Services.Certifications;

public class CertificationService :
    ICommandHandler<CreateCertificationCommand, CertificationDto>,
    ICommandHandler<UpdateCertificationCommand, CertificationDto>,
    ICommandHandler<DeleteCertificationCommand>,
    ICommandHandler<UpdateCertificationStatusCommand>,
    ICommandHandler<IssueCertificationCommand>,
    ICommandHandler<SuspendCertificationCommand>,
    ICommandHandler<RevokeCertificationCommand>,
    ICommandHandler<RenewCertificationCommand>,
    ICommandHandler<SetCertificationCertificateNumberCommand>,
    ICommandHandler<AddCertificationDecisionCommand, CertificationDecisionDto>,
    ICommandHandler<AddCertificationScopeCommand, CertificationScopeDto>,
    ICommandHandler<UpdateCertificationScopeCommand, CertificationScopeDto>,
    ICommandHandler<DeleteCertificationScopeCommand>,
    ICommandHandler<UpdateSurveillanceDueCommand>,
    IQueryHandler<GetCertificationByIdQuery, CertificationDto?>,
    IQueryHandler<GetCertificationsQuery, PaginatedResult<CertificationListDto>>,
    IQueryHandler<GetCertificationSummaryQuery, CertificationSummaryDto?>,
    IQueryHandler<GetCertificationDecisionsQuery, IReadOnlyList<CertificationDecisionDto>>,
    IQueryHandler<GetCertificationDecisionByIdQuery, CertificationDecisionDto?>,
    IQueryHandler<GetCertificationScopesQuery, IReadOnlyList<CertificationScopeDto>>,
    IQueryHandler<GetCertificationScopeByIdQuery, CertificationScopeDto?>,
    IQueryHandler<GetCertificationsByOrganizationQuery, IReadOnlyList<CertificationListDto>>,
    IQueryHandler<GetActiveCertificationsQuery, IReadOnlyList<CertificationListDto>>,
    IQueryHandler<GetExpiringCertificationsQuery, IReadOnlyList<CertificationListDto>>,
    IQueryHandler<GetExpiredCertificationsQuery, IReadOnlyList<CertificationListDto>>,
    IQueryHandler<GetSurveillanceDueCertificationsQuery, IReadOnlyList<CertificationListDto>>,
    IQueryHandler<GetSuspendedCertificationsQuery, IReadOnlyList<CertificationListDto>>,
    IQueryHandler<GetRevokedCertificationsQuery, IReadOnlyList<CertificationListDto>>,
    IQueryHandler<GetCertificationByCertificateNumberQuery, CertificationDto?>
{
    private readonly IRepository<Certification> _certificationRepository;
    private readonly IRepository<CertificationDecision> _decisionRepository;
    private readonly IRepository<CertificationScope> _scopeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CertificationService(
        IRepository<Certification> certificationRepository,
        IRepository<CertificationDecision> decisionRepository,
        IRepository<CertificationScope> scopeRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _certificationRepository = certificationRepository;
        _decisionRepository = decisionRepository;
        _scopeRepository = scopeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CertificationDto> HandleAsync(CreateCertificationCommand command, CancellationToken ct = default)
    {
        var certification = new Certification(
            command.OrganizationId,
            command.ProgramId,
            command.AssessmentId,
            command.Title);

        if (!string.IsNullOrEmpty(command.CertificateNumber))
        {
            certification.SetCertificateNumber(command.CertificateNumber);
        }

        if (command.SurveillanceIntervalMonths > 0)
        {
            certification.SurveillanceIntervalMonths = command.SurveillanceIntervalMonths;
        }

        await _certificationRepository.AddAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<CertificationDto>(certification);
    }

    public async Task<CertificationDto> HandleAsync(UpdateCertificationCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Certification));

        certification.UpdateDetails(command.Title, command.CertificateNumber, command.SurveillanceIntervalMonths);
        await _certificationRepository.UpdateAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<CertificationDto>(certification);
    }

    public async Task HandleAsync(DeleteCertificationCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Certification));

        certification.MarkAsDeleted();
        await _certificationRepository.UpdateAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateCertificationStatusCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Certification));

        if (Enum.TryParse<CertificationStatus>(command.Status, out var status))
        {
            certification.UpdateStatus(status);
            await _certificationRepository.UpdateAsync(certification, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task HandleAsync(IssueCertificationCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Certification));

        certification.Issue(command.IssuedById, command.CertificateUrl);
        await _certificationRepository.UpdateAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SuspendCertificationCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Certification));

        certification.Suspend(command.Reason);
        await _certificationRepository.UpdateAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(RevokeCertificationCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Certification));

        certification.Revoke(command.Reason);
        await _certificationRepository.UpdateAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(RenewCertificationCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Certification));

        certification.Renew(command.RenewedById, command.NewExpirationDate);
        await _certificationRepository.UpdateAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetCertificationCertificateNumberCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Certification));

        certification.SetCertificateNumber(command.CertificateNumber);
        await _certificationRepository.UpdateAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<CertificationDecisionDto> HandleAsync(AddCertificationDecisionCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.CertificationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.CertificationId, nameof(Certification));

        var decision = certification.AddDecision(
            command.Decision,
            command.Rationale,
            command.DecidedById,
            Enum.Parse<CertificationDecisionType>(command.Type));

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<CertificationDecisionDto>(decision);
    }

    public async Task<CertificationScopeDto> HandleAsync(AddCertificationScopeCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.CertificationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.CertificationId, nameof(Certification));

        var scope = certification.AddScope(command.Name, command.Description, command.StandardId);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<CertificationScopeDto>(scope);
    }

    public async Task<CertificationScopeDto> HandleAsync(UpdateCertificationScopeCommand command, CancellationToken ct = default)
    {
        var scope = await _scopeRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(CertificationScope));

        scope.UpdateDetails(command.Name, command.Description);
        await _scopeRepository.UpdateAsync(scope, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<CertificationScopeDto>(scope);
    }

    public async Task HandleAsync(DeleteCertificationScopeCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.CertificationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.CertificationId, nameof(Certification));

        certification.RemoveScope(command.ScopeId);
        await _certificationRepository.UpdateAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateSurveillanceDueCommand command, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Certification));

        certification.UpdateSurveillanceDue(command.DueDate);
        await _certificationRepository.UpdateAsync(certification, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<CertificationDto?> HandleAsync(GetCertificationByIdQuery query, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(query.Id, ct);
        return certification != null ? _mapper.Map<CertificationDto>(certification) : null;
    }

    public async Task<PaginatedResult<CertificationListDto>> HandleAsync(GetCertificationsQuery query, CancellationToken ct = default)
    {
        var certifications = await _certificationRepository.ListAsync(ct);

        var filtered = certifications.AsQueryable();

        if (query.OrganizationId.HasValue)
        {
            filtered = filtered.Where(c => c.OrganizationId == query.OrganizationId.Value);
        }

        if (query.ProgramId.HasValue)
        {
            filtered = filtered.Where(c => c.ProgramId == query.ProgramId.Value);
        }

        if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<CertificationStatus>(query.Status, out var status))
        {
            filtered = filtered.Where(c => c.Status == status);
        }

        if (query.ExpiringSoon == true)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(90);
            filtered = filtered.Where(c =>
                c.ExpirationDate.HasValue &&
                c.ExpirationDate.Value <= cutoffDate &&
                c.ExpirationDate.Value >= DateTime.UtcNow &&
                c.Status == CertificationStatus.Issued);
        }

        var totalCount = filtered.Count();
        var items = filtered
            .OrderByDescending(c => c.CreatedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<CertificationListDto>>(items);

        return new PaginatedResult<CertificationListDto>
        {
            Items = dtos,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<CertificationSummaryDto?> HandleAsync(GetCertificationSummaryQuery query, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(query.Id, ct);
        if (certification == null) return null;

        var dto = _mapper.Map<CertificationSummaryDto>(certification);
        dto.IsActive = certification.IsActive;
        dto.IsExpired = certification.IsExpired;
        dto.IsExpiringSoon = certification.IsExpiringSoon;
        dto.IsSuspended = certification.IsSuspended;
        dto.DaysUntilExpiration = certification.ExpirationDate.HasValue
            ? (int)(certification.ExpirationDate.Value - DateTime.UtcNow).TotalDays
            : 0;
        dto.DaysUntilSurveillance = certification.NextSurveillanceDue.HasValue
            ? (int)(certification.NextSurveillanceDue.Value - DateTime.UtcNow).TotalDays
            : 0;

        return dto;
    }

    public async Task<IReadOnlyList<CertificationDecisionDto>> HandleAsync(GetCertificationDecisionsQuery query, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(query.CertificationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.CertificationId, nameof(Certification));

        return _mapper.Map<IReadOnlyList<CertificationDecisionDto>>(certification.Decisions);
    }

    public async Task<CertificationDecisionDto?> HandleAsync(GetCertificationDecisionByIdQuery query, CancellationToken ct = default)
    {
        var decision = await _decisionRepository.GetByIdAsync(query.DecisionId, ct);
        return decision != null ? _mapper.Map<CertificationDecisionDto>(decision) : null;
    }

    public async Task<IReadOnlyList<CertificationScopeDto>> HandleAsync(GetCertificationScopesQuery query, CancellationToken ct = default)
    {
        var certification = await _certificationRepository.GetByIdAsync(query.CertificationId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.CertificationId, nameof(Certification));

        return _mapper.Map<IReadOnlyList<CertificationScopeDto>>(certification.Scopes);
    }

    public async Task<CertificationScopeDto?> HandleAsync(GetCertificationScopeByIdQuery query, CancellationToken ct = default)
    {
        var scope = await _scopeRepository.GetByIdAsync(query.ScopeId, ct);
        return scope != null ? _mapper.Map<CertificationScopeDto>(scope) : null;
    }

    public async Task<IReadOnlyList<CertificationListDto>> HandleAsync(GetCertificationsByOrganizationQuery query, CancellationToken ct = default)
    {
        var certifications = await _certificationRepository.ListAsync(ct);
        var filtered = certifications.Where(c => c.OrganizationId == query.OrganizationId).ToList();
        return _mapper.Map<IReadOnlyList<CertificationListDto>>(filtered);
    }

    public async Task<IReadOnlyList<CertificationListDto>> HandleAsync(GetActiveCertificationsQuery query, CancellationToken ct = default)
    {
        var certifications = await _certificationRepository.ListAsync(ct);
        var filtered = certifications.Where(c => c.Status == CertificationStatus.Issued).ToList();
        return _mapper.Map<IReadOnlyList<CertificationListDto>>(filtered);
    }

    public async Task<IReadOnlyList<CertificationListDto>> HandleAsync(GetExpiringCertificationsQuery query, CancellationToken ct = default)
    {
        var certifications = await _certificationRepository.ListAsync(ct);
        var cutoffDate = DateTime.UtcNow.AddDays(query.Days);
        var filtered = certifications.Where(c =>
            c.ExpirationDate.HasValue &&
            c.ExpirationDate.Value <= cutoffDate &&
            c.ExpirationDate.Value >= DateTime.UtcNow &&
            c.Status == CertificationStatus.Issued).ToList();
        return _mapper.Map<IReadOnlyList<CertificationListDto>>(filtered);
    }

    public async Task<IReadOnlyList<CertificationListDto>> HandleAsync(GetExpiredCertificationsQuery query, CancellationToken ct = default)
    {
        var certifications = await _certificationRepository.ListAsync(ct);
        var filtered = certifications.Where(c =>
            c.ExpirationDate.HasValue &&
            c.ExpirationDate.Value < DateTime.UtcNow).ToList();
        return _mapper.Map<IReadOnlyList<CertificationListDto>>(filtered);
    }

    public async Task<IReadOnlyList<CertificationListDto>> HandleAsync(GetSurveillanceDueCertificationsQuery query, CancellationToken ct = default)
    {
        var certifications = await _certificationRepository.ListAsync(ct);
        var filtered = certifications.Where(c =>
            c.NextSurveillanceDue.HasValue &&
            c.NextSurveillanceDue.Value <= DateTime.UtcNow &&
            c.Status == CertificationStatus.Issued).ToList();
        return _mapper.Map<IReadOnlyList<CertificationListDto>>(filtered);
    }

    public async Task<IReadOnlyList<CertificationListDto>> HandleAsync(GetSuspendedCertificationsQuery query, CancellationToken ct = default)
    {
        var certifications = await _certificationRepository.ListAsync(ct);
        var filtered = certifications.Where(c => c.Status == CertificationStatus.Suspended).ToList();
        return _mapper.Map<IReadOnlyList<CertificationListDto>>(filtered);
    }

    public async Task<IReadOnlyList<CertificationListDto>> HandleAsync(GetRevokedCertificationsQuery query, CancellationToken ct = default)
    {
        var certifications = await _certificationRepository.ListAsync(ct);
        var filtered = certifications.Where(c => c.Status == CertificationStatus.Revoked).ToList();
        return _mapper.Map<IReadOnlyList<CertificationListDto>>(filtered);
    }

    public async Task<CertificationDto?> HandleAsync(GetCertificationByCertificateNumberQuery query, CancellationToken ct = default)
    {
        var certifications = await _certificationRepository.ListAsync(ct);
        var certification = certifications.FirstOrDefault(c => c.CertificateNumber == query.CertificateNumber);
        return certification != null ? _mapper.Map<CertificationDto>(certification) : null;
    }
}