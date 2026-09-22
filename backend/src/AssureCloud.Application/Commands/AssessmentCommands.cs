using System;
using AssureCloud.Application.Commands;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Commands.Assessments;

public record CreateAssessmentCommand(
    Guid ProgramId,
    Guid OrganizationId,
    Guid StandardId,
    DateTime ScheduledDate,
    string? Title = null,
    string? Description = null,
    string? Notes = null) : ICommand<AssessmentDto>;

public record UpdateAssessmentCommand(
    Guid Id,
    string? Title = null,
    string? Description = null,
    DateTime? ScheduledDate = null,
    string? AssessorNotes = null) : ICommand<AssessmentDto>;

public record DeleteAssessmentCommand(Guid Id) : ICommand;

public record UpdateAssessmentStatusCommand(Guid Id, string Status) : ICommand;

public record SetAssessmentScoreCommand(Guid Id, decimal Score) : ICommand;

public record SetAssessmentLeadAssessorCommand(Guid Id, Guid AssessorId) : ICommand;

public record SetAssessmentReviewerCommand(Guid Id, Guid ReviewerId) : ICommand;

public record SetAssessmentReviewerNotesCommand(Guid Id, string Notes) : ICommand;

public record AssignAssessorCommand(
    Guid AssessmentId,
    Guid AssessorId,
    string Role = "Assessor") : ICommand<AssessmentAssignmentDto>;

public record RemoveAssessorCommand(Guid AssessmentId, Guid AssessorId) : ICommand;

public record SubmitAssessmentCommand(Guid Id, Guid SubmittedById) : ICommand;

public record AddAssessmentResponseCommand(
    Guid AssessmentId,
    Guid CriterionId,
    decimal Score,
    string? Comments = null,
    bool IsManual = false) : ICommand<AssessmentResponseDto>;

public record UpdateAssessmentResponseCommand(
    Guid Id,
    decimal Score,
    string? Comments = null,
    bool? IsManual = null) : ICommand<AssessmentResponseDto>;

public record AddEvidenceCommand(
    Guid AssessmentId,
    string Title,
    string? Description,
    string FileUrl,
    string? MimeType = null,
    long? FileSize = null) : ICommand<EvidenceDto>;

public record UpdateEvidenceCommand(
    Guid Id,
    string Title,
    string? Description = null,
    string? FileUrl = null,
    string? MimeType = null,
    long? FileSize = null) : ICommand<EvidenceDto>;

public record DeleteEvidenceCommand(Guid AssessmentId, Guid EvidenceId) : ICommand;

public record SubmitEvidenceCommand(Guid Id, Guid UserId) : ICommand;

public record VerifyEvidenceCommand(Guid Id, Guid UserId, string? Notes = null) : ICommand;

public record RejectEvidenceCommand(Guid Id, Guid UserId, string? Notes = null) : ICommand;

public record CreateFindingCommand(
    Guid AssessmentId,
    string Title,
    string Description,
    string Severity,
    string Category,
    Guid? CriterionId = null,
    string? RootCause = null,
    string? Impact = null,
    string? Recommendation = null) : ICommand<FindingDto>;

public record UpdateFindingCommand(
    Guid Id,
    string Title,
    string Description,
    string Severity,
    string Category,
    string? RootCause = null,
    string? Impact = null,
    string? Recommendation = null) : ICommand<FindingDto>;

public record DeleteFindingCommand(Guid AssessmentId, Guid FindingId) : ICommand;

public record UpdateFindingStatusCommand(Guid Id, string Status) : ICommand;

public record ResolveFindingCommand(Guid Id, Guid UserId, string? ResolutionNotes = null) : ICommand;

public record ReopenFindingCommand(Guid Id) : ICommand;