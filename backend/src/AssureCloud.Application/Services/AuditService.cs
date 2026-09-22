using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Application.Commands.Audits;
using AssureCloud.Application.DTOs;
using AssureCloud.Application.Queries.Audits;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AssureCloud.Application.Services.Audits;

public class AuditService :
    ICommandHandler<CreateAuditCommand, AuditDto>,
    ICommandHandler<UpdateAuditCommand, AuditDto>,
    ICommandHandler<DeleteAuditCommand>,
    ICommandHandler<UpdateAuditStatusCommand>,
    ICommandHandler<SetAuditLeadAuditorCommand>,
    ICommandHandler<SetAuditOverallScoreCommand>,
    ICommandHandler<SetAuditReportUrlCommand>,
    ICommandHandler<AssignAuditorCommand, AuditAssignmentDto>,
    ICommandHandler<RemoveAuditorCommand>,
    ICommandHandler<CreateAuditFindingCommand, AuditFindingDto>,
    ICommandHandler<UpdateAuditFindingCommand, AuditFindingDto>,
    ICommandHandler<DeleteAuditFindingCommand>,
    ICommandHandler<UpdateAuditFindingStatusCommand>,
    ICommandHandler<ResolveAuditFindingCommand>,
    ICommandHandler<ReopenAuditFindingCommand>,
    ICommandHandler<CreateCorrectiveActionCommand, CorrectiveActionDto>,
    ICommandHandler<UpdateCorrectiveActionCommand, CorrectiveActionDto>,
    ICommandHandler<CompleteCorrectiveActionCommand>,
    ICommandHandler<VerifyCorrectiveActionCommand>,
    ICommandHandler<RejectCorrectiveActionCommand>,
    ICommandHandler<ExtendCorrectiveActionDueDateCommand>,
    IQueryHandler<GetAuditByIdQuery, AuditDto?>,
    IQueryHandler<GetAuditsQuery, PaginatedResult<AuditListDto>>,
    IQueryHandler<GetAuditSummaryQuery, AuditSummaryDto?>,
    IQueryHandler<GetAuditFindingsQuery, IReadOnlyList<AuditFindingDto>>,
    IQueryHandler<GetAuditFindingByIdQuery, AuditFindingDto?>,
    IQueryHandler<GetCorrectiveActionsByAuditQuery, IReadOnlyList<CorrectiveActionDto>>,
    IQueryHandler<GetCorrectiveActionByIdQuery, CorrectiveActionDto?>,
    IQueryHandler<GetOverdueCorrectiveActionsQuery, IReadOnlyList<CorrectiveActionDto>>,
    IQueryHandler<GetAuditAssignmentsQuery, IReadOnlyList<AuditAssignmentDto>>,
    IQueryHandler<GetAuditsByOrganizationQuery, IReadOnlyList<AuditListDto>>,
    IQueryHandler<GetAuditsByAuditorQuery, IReadOnlyList<AuditListDto>>,
    IQueryHandler<GetOverdueAuditsQuery, IReadOnlyList<AuditListDto>>,
    IQueryHandler<GetAuditsWithOverdueCorrectiveActionsQuery, IReadOnlyList<AuditListDto>>
{
    private readonly IRepository<Audit> _auditRepository;
    private readonly IRepository<AuditFinding> _findingRepository;
    private readonly IRepository<CorrectiveAction> _correctiveActionRepository;
    private readonly IRepository<AuditAssignment> _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AuditService(
        IRepository<Audit> auditRepository,
        IRepository<AuditFinding> findingRepository,
        IRepository<CorrectiveAction> correctiveActionRepository,
        IRepository<AuditAssignment> assignmentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _auditRepository = auditRepository;
        _findingRepository = findingRepository;
        _correctiveActionRepository = correctiveActionRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuditDto> HandleAsync(CreateAuditCommand command, CancellationToken ct = default)
    {
        var audit = new Audit(
            command.OrganizationId,
            Enum.Parse<AuditType>(command.Type),
            command.PlannedStartDate,
            command.PlannedEndDate,
            command.AssessmentId,
            command.Title,
            command.Description);

        if (!string.IsNullOrEmpty(command.Scope))
        {
            audit.UpdateDetails(audit.Title, audit.Description, audit.Type, audit.PlannedStartDate, audit.PlannedEndDate, command.Scope, command.Criteria);
        }

        await _auditRepository.AddAsync(audit, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<AuditDto>(audit);
    }

    public async Task<AuditDto> HandleAsync(UpdateAuditCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Audit));

        audit.UpdateDetails(
            command.Title,
            command.Description,
            command.Type != null ? Enum.Parse<AuditType>(command.Type) : null,
            command.PlannedStartDate,
            command.PlannedEndDate,
            command.Scope,
            command.Criteria);

        await _auditRepository.UpdateAsync(audit, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<AuditDto>(audit);
    }

    public async Task HandleAsync(DeleteAuditCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Audit));

        audit.MarkAsDeleted();
        await _auditRepository.UpdateAsync(audit, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateAuditStatusCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Audit));

        if (Enum.TryParse<AuditStatus>(command.Status, out var status))
        {
            audit.UpdateStatus(status);
            await _auditRepository.UpdateAsync(audit, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task HandleAsync(SetAuditLeadAuditorCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Audit));

        audit.SetLeadAuditor(command.AuditorId);
        await _auditRepository.UpdateAsync(audit, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetAuditOverallScoreCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Audit));

        audit.SetOverallScore(command.Score);
        await _auditRepository.UpdateAsync(audit, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetAuditReportUrlCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Audit));

        audit.SetReportUrl(command.Url);
        await _auditRepository.UpdateAsync(audit, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<AuditAssignmentDto> HandleAsync(AssignAuditorCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.AuditId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AuditId, nameof(Audit));

        var role = Enum.Parse<AuditRole>(command.Role);
        var assignment = audit.AssignAuditor(command.AuditorId, role);

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<AuditAssignmentDto>(assignment);
    }

    public async Task HandleAsync(RemoveAuditorCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.AuditId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AuditId, nameof(Audit));

        audit.RemoveAuditor(command.AuditorId);
        await _auditRepository.UpdateAsync(audit, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<AuditFindingDto> HandleAsync(CreateAuditFindingCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.AuditId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AuditId, nameof(Audit));

        var finding = audit.AddFinding(
            command.Title,
            command.Description,
            Enum.Parse<FindingSeverity>(command.Severity),
            Enum.Parse<FindingCategory>(command.Category),
            command.CriterionId,
            command.Requirement);

        if (!string.IsNullOrEmpty(command.RootCause))
        {
            finding.UpdateDetails(finding.Title, finding.Description, finding.Severity, finding.Category, command.RootCause, command.Impact, command.Recommendation);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<AuditFindingDto>(finding);
    }

    public async Task<AuditFindingDto> HandleAsync(UpdateAuditFindingCommand command, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(AuditFinding));

        finding.UpdateDetails(
            command.Title,
            command.Description,
            Enum.Parse<FindingSeverity>(command.Severity),
            Enum.Parse<FindingCategory>(command.Category),
            command.RootCause,
            command.Impact,
            command.Recommendation);

        await _findingRepository.UpdateAsync(finding, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<AuditFindingDto>(finding);
    }

    public async Task HandleAsync(DeleteAuditFindingCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.AuditId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AuditId, nameof(Audit));

        audit.RemoveFinding(command.FindingId);
        await _auditRepository.UpdateAsync(audit, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateAuditFindingStatusCommand command, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(AuditFinding));

        if (Enum.TryParse<FindingStatus>(command.Status, out var status))
        {
            finding.UpdateStatus(status);
            await _findingRepository.UpdateAsync(finding, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task HandleAsync(ResolveAuditFindingCommand command, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(AuditFinding));

        finding.Resolve(command.UserId, command.ResolutionNotes);
        await _findingRepository.UpdateAsync(finding, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(ReopenAuditFindingCommand command, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(AuditFinding));

        finding.Reopen();
        await _findingRepository.UpdateAsync(finding, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<CorrectiveActionDto> HandleAsync(CreateCorrectiveActionCommand command, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(command.AuditId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AuditId, nameof(Audit));

        var correctiveAction = audit.AddCorrectiveAction(
            command.Title,
            command.Description,
            command.FindingId,
            command.ResponsiblePartyId,
            command.DueDate,
            command.RootCause);

        if (!string.IsNullOrEmpty(command.ActionPlan))
        {
            correctiveAction.UpdateDetails(
                correctiveAction.Title,
                correctiveAction.Description,
                correctiveAction.RootCause,
                command.ActionPlan,
                command.VerificationMethod,
                command.Priority,
                null);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<CorrectiveActionDto>(correctiveAction);
    }

    public async Task<CorrectiveActionDto> HandleAsync(UpdateCorrectiveActionCommand command, CancellationToken ct = default)
    {
        var correctiveAction = await _correctiveActionRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(CorrectiveAction));

        correctiveAction.UpdateDetails(
            command.Title,
            command.Description,
            command.RootCause,
            command.ActionPlan,
            command.VerificationMethod,
            command.Priority,
            command.DueDate);

        await _correctiveActionRepository.UpdateAsync(correctiveAction, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<CorrectiveActionDto>(correctiveAction);
    }

    public async Task HandleAsync(CompleteCorrectiveActionCommand command, CancellationToken ct = default)
    {
        var correctiveAction = await _correctiveActionRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(CorrectiveAction));

        correctiveAction.Complete(command.VerificationNotes);
        await _correctiveActionRepository.UpdateAsync(correctiveAction, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(VerifyCorrectiveActionCommand command, CancellationToken ct = default)
    {
        var correctiveAction = await _correctiveActionRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(CorrectiveAction));

        correctiveAction.Verify(command.UserId, command.VerificationNotes);
        await _correctiveActionRepository.UpdateAsync(correctiveAction, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(RejectCorrectiveActionCommand command, CancellationToken ct = default)
    {
        var correctiveAction = await _correctiveActionRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(CorrectiveAction));

        correctiveAction.Reject(command.UserId, command.VerificationNotes);
        await _correctiveActionRepository.UpdateAsync(correctiveAction, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(ExtendCorrectiveActionDueDateCommand command, CancellationToken ct = default)
    {
        var correctiveAction = await _correctiveActionRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(CorrectiveAction));

        correctiveAction.ExtendDueDate(command.NewDueDate, command.Reason);
        await _correctiveActionRepository.UpdateAsync(correctiveAction, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<AuditDto?> HandleAsync(GetAuditByIdQuery query, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(query.Id, ct);
        return audit != null ? _mapper.Map<AuditDto>(audit) : null;
    }

    public async Task<PaginatedResult<AuditListDto>> HandleAsync(GetAuditsQuery query, CancellationToken ct = default)
    {
        var audits = await _auditRepository.ListAsync(ct);

        var filtered = audits.AsQueryable();

        if (query.OrganizationId.HasValue)
        {
            filtered = filtered.Where(a => a.OrganizationId == query.OrganizationId.Value);
        }

        if (query.AssessmentId.HasValue)
        {
            filtered = filtered.Where(a => a.AssessmentId == query.AssessmentId.Value);
        }

        if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<AuditStatus>(query.Status, out var status))
        {
            filtered = filtered.Where(a => a.Status == status);
        }

        if (!string.IsNullOrEmpty(query.Type) && Enum.TryParse<AuditType>(query.Type, out var type))
        {
            filtered = filtered.Where(a => a.Type == type);
        }

        if (query.LeadAuditorId.HasValue)
        {
            filtered = filtered.Where(a => a.LeadAuditorId == query.LeadAuditorId.Value);
        }

        if (query.AuditorId.HasValue)
        {
            filtered = filtered.Where(a =>
                a.LeadAuditorId == query.AuditorId.Value ||
                a.Assignments.Any(asg => asg.AuditorId == query.AuditorId.Value && asg.RemovedAt == null));
        }

        if (query.PlannedFrom.HasValue)
        {
            filtered = filtered.Where(a => a.PlannedStartDate >= query.PlannedFrom.Value);
        }

        if (query.PlannedTo.HasValue)
        {
            filtered = filtered.Where(a => a.PlannedEndDate <= query.PlannedTo.Value);
        }

        var totalCount = filtered.Count();
        var items = filtered
            .OrderByDescending(a => a.PlannedStartDate)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<AuditListDto>>(items);

        return new PaginatedResult<AuditListDto>
        {
            Items = dtos,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<AuditSummaryDto?> HandleAsync(GetAuditSummaryQuery query, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(query.Id, ct);
        if (audit == null) return null;

        var dto = _mapper.Map<AuditSummaryDto>(audit);
        dto.TotalFindings = audit.Findings.Count;
        dto.OpenFindings = audit.Findings.Count(f => f.IsOpen);
        dto.CriticalFindings = audit.Findings.Count(f => f.Severity == FindingSeverity.Critical);
        dto.MajorFindings = audit.Findings.Count(f => f.Category == FindingCategory.MajorNonConformity);
        dto.MinorFindings = audit.Findings.Count(f => f.Category == FindingCategory.MinorNonConformity);
        dto.TotalCorrectiveActions = audit.CorrectiveActions.Count;
        dto.CompletedCorrectiveActions = audit.CorrectiveActions.Count(ca => ca.Status == CorrectiveActionStatus.Completed || ca.Status == CorrectiveActionStatus.Verified);
        dto.OverdueCorrectiveActions = audit.CorrectiveActions.Count(ca => ca.IsOverdue);

        return dto;
    }

    public async Task<IReadOnlyList<AuditFindingDto>> HandleAsync(GetAuditFindingsQuery query, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(query.AuditId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.AuditId, nameof(Audit));

        return _mapper.Map<IReadOnlyList<AuditFindingDto>>(audit.Findings);
    }

    public async Task<AuditFindingDto?> HandleAsync(GetAuditFindingByIdQuery query, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(query.FindingId, ct);
        return finding != null ? _mapper.Map<AuditFindingDto>(finding) : null;
    }

    public async Task<IReadOnlyList<CorrectiveActionDto>> HandleAsync(GetCorrectiveActionsByAuditQuery query, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(query.AuditId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.AuditId, nameof(Audit));

        return _mapper.Map<IReadOnlyList<CorrectiveActionDto>>(audit.CorrectiveActions);
    }

    public async Task<CorrectiveActionDto?> HandleAsync(GetCorrectiveActionByIdQuery query, CancellationToken ct = default)
    {
        var correctiveAction = await _correctiveActionRepository.GetByIdAsync(query.CorrectiveActionId, ct);
        return correctiveAction != null ? _mapper.Map<CorrectiveActionDto>(correctiveAction) : null;
    }

    public async Task<IReadOnlyList<CorrectiveActionDto>> HandleAsync(GetOverdueCorrectiveActionsQuery query, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(query.AuditId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.AuditId, nameof(Audit));

        var overdue = audit.CorrectiveActions.Where(ca => ca.IsOverdue).ToList();
        return _mapper.Map<IReadOnlyList<CorrectiveActionDto>>(overdue);
    }

    public async Task<IReadOnlyList<AuditAssignmentDto>> HandleAsync(GetAuditAssignmentsQuery query, CancellationToken ct = default)
    {
        var audit = await _auditRepository.GetByIdAsync(query.AuditId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.AuditId, nameof(Audit));

        return _mapper.Map<IReadOnlyList<AuditAssignmentDto>>(audit.Assignments);
    }

    public async Task<IReadOnlyList<AuditListDto>> HandleAsync(GetAuditsByOrganizationQuery query, CancellationToken ct = default)
    {
        var audits = await _auditRepository.ListAsync(ct);
        var filtered = audits.Where(a => a.OrganizationId == query.OrganizationId).ToList();
        return _mapper.Map<IReadOnlyList<AuditListDto>>(filtered);
    }

    public async Task<IReadOnlyList<AuditListDto>> HandleAsync(GetAuditsByAuditorQuery query, CancellationToken ct = default)
    {
        var audits = await _auditRepository.ListAsync(ct);
        var filtered = audits.Where(a =>
            a.LeadAuditorId == query.AuditorId ||
            a.Assignments.Any(asg => asg.AuditorId == query.AuditorId && asg.RemovedAt == null)).ToList();
        return _mapper.Map<IReadOnlyList<AuditListDto>>(filtered);
    }

    public async Task<IReadOnlyList<AuditListDto>> HandleAsync(GetOverdueAuditsQuery query, CancellationToken ct = default)
    {
        var audits = await _auditRepository.ListAsync(ct);
        var now = DateTime.UtcNow;
        var filtered = audits.Where(a =>
            a.PlannedEndDate < now &&
            (a.Status == AuditStatus.Planned || a.Status == AuditStatus.InProgress || a.Status == AuditStatus.OnHold)).ToList();
        return _mapper.Map<IReadOnlyList<AuditListDto>>(filtered);
    }

    public async Task<IReadOnlyList<AuditListDto>> HandleAsync(GetAuditsWithOverdueCorrectiveActionsQuery query, CancellationToken ct = default)
    {
        var audits = await _auditRepository.ListAsync(ct);
        var filtered = audits.Where(a =>
            a.CorrectiveActions.Any(ca =>
                ca.Status != CorrectiveActionStatus.Completed &&
                ca.Status != CorrectiveActionStatus.Verified &&
                ca.DueDate < DateTime.UtcNow)).ToList();
        return _mapper.Map<IReadOnlyList<AuditListDto>>(filtered);
    }
}