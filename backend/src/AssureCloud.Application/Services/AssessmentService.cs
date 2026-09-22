using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Application.Commands.Assessments;
using AssureCloud.Application.DTOs;
using AssureCloud.Application.Queries.Assessments;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AssureCloud.Application.Services.Assessments;

public class AssessmentService :
    ICommandHandler<CreateAssessmentCommand, AssessmentDto>,
    ICommandHandler<UpdateAssessmentCommand, AssessmentDto>,
    ICommandHandler<DeleteAssessmentCommand>,
    ICommandHandler<UpdateAssessmentStatusCommand>,
    ICommandHandler<SetAssessmentScoreCommand>,
    ICommandHandler<SetAssessmentLeadAssessorCommand>,
    ICommandHandler<SetAssessmentReviewerCommand>,
    ICommandHandler<SetAssessmentReviewerNotesCommand>,
    ICommandHandler<AssignAssessorCommand, AssessmentAssignmentDto>,
    ICommandHandler<RemoveAssessorCommand>,
    ICommandHandler<SubmitAssessmentCommand>,
    ICommandHandler<AddAssessmentResponseCommand, AssessmentResponseDto>,
    ICommandHandler<UpdateAssessmentResponseCommand, AssessmentResponseDto>,
    ICommandHandler<AddEvidenceCommand, EvidenceDto>,
    ICommandHandler<UpdateEvidenceCommand, EvidenceDto>,
    ICommandHandler<DeleteEvidenceCommand>,
    ICommandHandler<SubmitEvidenceCommand>,
    ICommandHandler<VerifyEvidenceCommand>,
    ICommandHandler<RejectEvidenceCommand>,
    ICommandHandler<CreateFindingCommand, FindingDto>,
    ICommandHandler<UpdateFindingCommand, FindingDto>,
    ICommandHandler<DeleteFindingCommand>,
    ICommandHandler<UpdateFindingStatusCommand>,
    ICommandHandler<ResolveFindingCommand>,
    ICommandHandler<ReopenFindingCommand>,
    IQueryHandler<GetAssessmentByIdQuery, AssessmentDto?>,
    IQueryHandler<GetAssessmentsQuery, PaginatedResult<AssessmentListDto>>,
    IQueryHandler<GetAssessmentSummaryQuery, AssessmentSummaryDto?>,
    IQueryHandler<GetAssessmentResponsesQuery, IReadOnlyList<AssessmentResponseDto>>,
    IQueryHandler<GetAssessmentResponseByIdQuery, AssessmentResponseDto?>,
    IQueryHandler<GetAssessmentEvidenceQuery, IReadOnlyList<EvidenceDto>>,
    IQueryHandler<GetEvidenceByIdQuery, EvidenceDto?>,
    IQueryHandler<GetAssessmentFindingsQuery, IReadOnlyList<FindingDto>>,
    IQueryHandler<GetFindingByIdQuery, FindingDto?>,
    IQueryHandler<GetAssessmentAssignmentsQuery, IReadOnlyList<AssessmentAssignmentDto>>,
    IQueryHandler<GetAssessmentsByOrganizationQuery, IReadOnlyList<AssessmentListDto>>,
    IQueryHandler<GetAssessmentsByAssessorQuery, IReadOnlyList<AssessmentListDto>>,
    IQueryHandler<GetOverdueAssessmentsQuery, IReadOnlyList<AssessmentListDto>>
{
    private readonly IRepository<Assessment> _assessmentRepository;
    private readonly IRepository<AssessmentResponse> _responseRepository;
    private readonly IRepository<Evidence> _evidenceRepository;
    private readonly IRepository<Finding> _findingRepository;
    private readonly IRepository<AssessmentAssignment> _assignmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AssessmentService(
        IRepository<Assessment> assessmentRepository,
        IRepository<AssessmentResponse> responseRepository,
        IRepository<Evidence> evidenceRepository,
        IRepository<Finding> findingRepository,
        IRepository<AssessmentAssignment> assignmentRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _assessmentRepository = assessmentRepository;
        _responseRepository = responseRepository;
        _evidenceRepository = evidenceRepository;
        _findingRepository = findingRepository;
        _assignmentRepository = assignmentRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AssessmentDto> HandleAsync(CreateAssessmentCommand command, CancellationToken ct = default)
    {
        var assessment = new Assessment(
            command.ProgramId,
            command.OrganizationId,
            command.StandardId,
            command.ScheduledDate,
            command.Notes);

        if (!string.IsNullOrEmpty(command.Title))
        {
            assessment.UpdateDetails(command.Title, command.Description, null, null);
        }

        await _assessmentRepository.AddAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<AssessmentDto>(assessment);
    }

    public async Task<AssessmentDto> HandleAsync(UpdateAssessmentCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Assessment));

        assessment.UpdateDetails(command.Title, command.Description, command.ScheduledDate, command.AssessorNotes);
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<AssessmentDto>(assessment);
    }

    public async Task HandleAsync(DeleteAssessmentCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Assessment));

        assessment.MarkAsDeleted();
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateAssessmentStatusCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Assessment));

        if (Enum.TryParse<AssessmentStatus>(command.Status, out var status))
        {
            assessment.UpdateStatus(status);
            await _assessmentRepository.UpdateAsync(assessment, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task HandleAsync(SetAssessmentScoreCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Assessment));

        assessment.SetScore(command.Score);
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetAssessmentLeadAssessorCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Assessment));

        assessment.SetLeadAssessor(command.AssessorId);
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetAssessmentReviewerCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Assessment));

        assessment.SetReviewer(command.ReviewerId);
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetAssessmentReviewerNotesCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Assessment));

        assessment.SetReviewerNotes(command.Notes);
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<AssessmentAssignmentDto> HandleAsync(AssignAssessorCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AssessmentId, nameof(Assessment));

        var role = Enum.Parse<AssessmentRole>(command.Role);
        var assignment = assessment.AssignAssessor(command.AssessorId, role);

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<AssessmentAssignmentDto>(assignment);
    }

    public async Task HandleAsync(RemoveAssessorCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AssessmentId, nameof(Assessment));

        assessment.RemoveAssessor(command.AssessorId);
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SubmitAssessmentCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Assessment));

        assessment.UpdateStatus(AssessmentStatus.InProgress);
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<AssessmentResponseDto> HandleAsync(AddAssessmentResponseCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AssessmentId, nameof(Assessment));

        var response = assessment.AddResponse(command.CriterionId, command.Score, command.Comments, command.IsManual);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<AssessmentResponseDto>(response);
    }

    public async Task<AssessmentResponseDto> HandleAsync(UpdateAssessmentResponseCommand command, CancellationToken ct = default)
    {
        var response = await _responseRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(AssessmentResponse));

        response.UpdateResponse(command.Score, command.Comments, command.IsManual);
        await _responseRepository.UpdateAsync(response, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<AssessmentResponseDto>(response);
    }

    public async Task<EvidenceDto> HandleAsync(AddEvidenceCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AssessmentId, nameof(Assessment));

        var evidence = assessment.AddEvidence(command.Title, command.Description, command.FileUrl, command.MimeType, command.FileSize);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<EvidenceDto>(evidence);
    }

    public async Task<EvidenceDto> HandleAsync(UpdateEvidenceCommand command, CancellationToken ct = default)
    {
        var evidence = await _evidenceRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Evidence));

        evidence.UpdateDetails(command.Title, command.Description, command.FileUrl, command.MimeType, command.FileSize);
        await _evidenceRepository.UpdateAsync(evidence, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<EvidenceDto>(evidence);
    }

    public async Task HandleAsync(DeleteEvidenceCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AssessmentId, nameof(Assessment));

        assessment.RemoveEvidence(command.EvidenceId);
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SubmitEvidenceCommand command, CancellationToken ct = default)
    {
        var evidence = await _evidenceRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Evidence));

        evidence.Submit(command.UserId);
        await _evidenceRepository.UpdateAsync(evidence, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(VerifyEvidenceCommand command, CancellationToken ct = default)
    {
        var evidence = await _evidenceRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Evidence));

        evidence.Verify(command.UserId, command.Notes);
        await _evidenceRepository.UpdateAsync(evidence, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(RejectEvidenceCommand command, CancellationToken ct = default)
    {
        var evidence = await _evidenceRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Evidence));

        evidence.Reject(command.UserId, command.Notes);
        await _evidenceRepository.UpdateAsync(evidence, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<FindingDto> HandleAsync(CreateFindingCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AssessmentId, nameof(Assessment));

        var finding = assessment.AddFinding(
            command.Title,
            command.Description,
            Enum.Parse<FindingSeverity>(command.Severity),
            Enum.Parse<FindingCategory>(command.Category),
            command.CriterionId);

        if (!string.IsNullOrEmpty(command.RootCause))
        {
            finding.UpdateDetails(finding.Title, finding.Description, finding.Severity, finding.Category, command.RootCause, command.Impact, command.Recommendation);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<FindingDto>(finding);
    }

    public async Task<FindingDto> HandleAsync(UpdateFindingCommand command, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Finding));

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

        return _mapper.Map<FindingDto>(finding);
    }

    public async Task HandleAsync(DeleteFindingCommand command, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(command.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.AssessmentId, nameof(Assessment));

        assessment.RemoveFinding(command.FindingId);
        await _assessmentRepository.UpdateAsync(assessment, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateFindingStatusCommand command, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Finding));

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

    public async Task HandleAsync(ResolveFindingCommand command, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Finding));

        finding.Resolve(command.UserId, command.ResolutionNotes);
        await _findingRepository.UpdateAsync(finding, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(ReopenFindingCommand command, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Finding));

        finding.Reopen();
        await _findingRepository.UpdateAsync(finding, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<AssessmentDto?> HandleAsync(GetAssessmentByIdQuery query, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(query.Id, ct);
        return assessment != null ? _mapper.Map<AssessmentDto>(assessment) : null;
    }

    public async Task<PaginatedResult<AssessmentListDto>> HandleAsync(GetAssessmentsQuery query, CancellationToken ct = default)
    {
        var assessments = await _assessmentRepository.ListAsync(ct);

        var filtered = assessments.AsQueryable();

        if (query.OrganizationId.HasValue)
        {
            filtered = filtered.Where(a => a.OrganizationId == query.OrganizationId.Value);
        }

        if (query.ProgramId.HasValue)
        {
            filtered = filtered.Where(a => a.ProgramId == query.ProgramId.Value);
        }

        if (query.StandardId.HasValue)
        {
            filtered = filtered.Where(a => a.StandardId == query.StandardId.Value);
        }

        if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<AssessmentStatus>(query.Status, out var status))
        {
            filtered = filtered.Where(a => a.Status == status);
        }

        if (query.AssessorId.HasValue)
        {
            filtered = filtered.Where(a =>
                a.LeadAssessorId == query.AssessorId.Value ||
                a.Assignments.Any(asg => asg.AssessorId == query.AssessorId.Value && asg.RemovedAt == null));
        }

        if (query.ReviewerId.HasValue)
        {
            filtered = filtered.Where(a => a.ReviewerId == query.ReviewerId.Value);
        }

        if (query.ScheduledFrom.HasValue)
        {
            filtered = filtered.Where(a => a.ScheduledDate >= query.ScheduledFrom.Value);
        }

        if (query.ScheduledTo.HasValue)
        {
            filtered = filtered.Where(a => a.ScheduledDate <= query.ScheduledTo.Value);
        }

        var totalCount = filtered.Count();
        var items = filtered
            .OrderByDescending(a => a.ScheduledDate)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<AssessmentListDto>>(items);

        return new PaginatedResult<AssessmentListDto>
        {
            Items = dtos,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<AssessmentSummaryDto?> HandleAsync(GetAssessmentSummaryQuery query, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(query.Id, ct);
        if (assessment == null) return null;

        var dto = _mapper.Map<AssessmentSummaryDto>(assessment);
        dto.TotalCriteria = assessment.Responses.Count;
        dto.RespondedCriteria = assessment.Responses.Count(r => r.RespondedAt.HasValue);
        dto.TotalEvidence = assessment.Evidence.Count;
        dto.VerifiedEvidence = assessment.Evidence.Count(e => e.Status == EvidenceStatus.Verified);
        dto.TotalFindings = assessment.Findings.Count;
        dto.OpenFindings = assessment.Findings.Count(f => f.IsOpen);
        dto.CriticalFindings = assessment.Findings.Count(f => f.Severity == FindingSeverity.Critical);
        dto.HighFindings = assessment.Findings.Count(f => f.Severity == FindingSeverity.High);
        dto.MediumFindings = assessment.Findings.Count(f => f.Severity == FindingSeverity.Medium);
        dto.LowFindings = assessment.Findings.Count(f => f.Severity == FindingSeverity.Low);

        return dto;
    }

    public async Task<IReadOnlyList<AssessmentResponseDto>> HandleAsync(GetAssessmentResponsesQuery query, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(query.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.AssessmentId, nameof(Assessment));

        return _mapper.Map<IReadOnlyList<AssessmentResponseDto>>(assessment.Responses);
    }

    public async Task<AssessmentResponseDto?> HandleAsync(GetAssessmentResponseByIdQuery query, CancellationToken ct = default)
    {
        var response = await _responseRepository.GetByIdAsync(query.ResponseId, ct);
        return response != null ? _mapper.Map<AssessmentResponseDto>(response) : null;
    }

    public async Task<IReadOnlyList<EvidenceDto>> HandleAsync(GetAssessmentEvidenceQuery query, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(query.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.AssessmentId, nameof(Assessment));

        return _mapper.Map<IReadOnlyList<EvidenceDto>>(assessment.Evidence);
    }

    public async Task<EvidenceDto?> HandleAsync(GetEvidenceByIdQuery query, CancellationToken ct = default)
    {
        var evidence = await _evidenceRepository.GetByIdAsync(query.EvidenceId, ct);
        return evidence != null ? _mapper.Map<EvidenceDto>(evidence) : null;
    }

    public async Task<IReadOnlyList<FindingDto>> HandleAsync(GetAssessmentFindingsQuery query, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(query.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.AssessmentId, nameof(Assessment));

        return _mapper.Map<IReadOnlyList<FindingDto>>(assessment.Findings);
    }

    public async Task<FindingDto?> HandleAsync(GetFindingByIdQuery query, CancellationToken ct = default)
    {
        var finding = await _findingRepository.GetByIdAsync(query.FindingId, ct);
        return finding != null ? _mapper.Map<FindingDto>(finding) : null;
    }

    public async Task<IReadOnlyList<AssessmentAssignmentDto>> HandleAsync(GetAssessmentAssignmentsQuery query, CancellationToken ct = default)
    {
        var assessment = await _assessmentRepository.GetByIdAsync(query.AssessmentId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.AssessmentId, nameof(Assessment));

        return _mapper.Map<IReadOnlyList<AssessmentAssignmentDto>>(assessment.Assignments);
    }

    public async Task<IReadOnlyList<AssessmentListDto>> HandleAsync(GetAssessmentsByOrganizationQuery query, CancellationToken ct = default)
    {
        var assessments = await _assessmentRepository.ListAsync(ct);
        var filtered = assessments.Where(a => a.OrganizationId == query.OrganizationId).ToList();
        return _mapper.Map<IReadOnlyList<AssessmentListDto>>(filtered);
    }

    public async Task<IReadOnlyList<AssessmentListDto>> HandleAsync(GetAssessmentsByAssessorQuery query, CancellationToken ct = default)
    {
        var assessments = await _assessmentRepository.ListAsync(ct);
        var filtered = assessments.Where(a =>
            a.LeadAssessorId == query.AssessorId ||
            a.Assignments.Any(asg => asg.AssessorId == query.AssessorId && asg.RemovedAt == null)).ToList();
        return _mapper.Map<IReadOnlyList<AssessmentListDto>>(filtered);
    }

    public async Task<IReadOnlyList<AssessmentListDto>> HandleAsync(GetOverdueAssessmentsQuery query, CancellationToken ct = default)
    {
        var assessments = await _assessmentRepository.ListAsync(ct);
        var now = DateTime.UtcNow;
        var filtered = assessments.Where(a =>
            a.ScheduledDate < now &&
            (a.Status == AssessmentStatus.Draft || a.Status == AssessmentStatus.InProgress || a.Status == AssessmentStatus.OnHold)).ToList();
        return _mapper.Map<IReadOnlyList<AssessmentListDto>>(filtered);
    }
}