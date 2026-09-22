using System;
using AssureCloud.Application.Commands;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Commands.Audits;

public record CreateAuditCommand(
    Guid OrganizationId,
    string Type,
    DateTime PlannedStartDate,
    DateTime PlannedEndDate,
    Guid? AssessmentId = null,
    string? Title = null,
    string? Description = null,
    string? Scope = null,
    string? Criteria = null) : ICommand<AuditDto>;

public record UpdateAuditCommand(
    Guid Id,
    string? Title = null,
    string? Description = null,
    string? Type = null,
    DateTime? PlannedStartDate = null,
    DateTime? PlannedEndDate = null,
    string? Scope = null,
    string? Criteria = null) : ICommand<AuditDto>;

public record DeleteAuditCommand(Guid Id) : ICommand;

public record UpdateAuditStatusCommand(Guid Id, string Status) : ICommand;

public record SetAuditLeadAuditorCommand(Guid Id, Guid AuditorId) : ICommand;

public record SetAuditOverallScoreCommand(Guid Id, decimal Score) : ICommand;

public record SetAuditReportUrlCommand(Guid Id, string Url) : ICommand;

public record AssignAuditorCommand(
    Guid AuditId,
    Guid AuditorId,
    string Role = "Auditor") : ICommand<AuditAssignmentDto>;

public record RemoveAuditorCommand(Guid AuditId, Guid AuditorId) : ICommand;

public record CreateAuditFindingCommand(
    Guid AuditId,
    string Title,
    string Description,
    string Severity,
    string Category,
    Guid? CriterionId = null,
    string? Requirement = null,
    string? RootCause = null,
    string? Impact = null,
    string? Recommendation = null) : ICommand<AuditFindingDto>;

public record UpdateAuditFindingCommand(
    Guid Id,
    string Title,
    string Description,
    string Severity,
    string Category,
    string? RootCause = null,
    string? Impact = null,
    string? Recommendation = null) : ICommand<AuditFindingDto>;

public record DeleteAuditFindingCommand(Guid AuditId, Guid FindingId) : ICommand;

public record UpdateAuditFindingStatusCommand(Guid Id, string Status) : ICommand;

public record ResolveAuditFindingCommand(Guid Id, Guid UserId, string? ResolutionNotes = null) : ICommand;

public record ReopenAuditFindingCommand(Guid Id) : ICommand;

public record CreateCorrectiveActionCommand(
    Guid AuditId,
    Guid FindingId,
    string Title,
    string Description,
    Guid ResponsiblePartyId,
    DateTime DueDate,
    string? RootCause = null,
    string? ActionPlan = null,
    string? VerificationMethod = null,
    int Priority = 3) : ICommand<CorrectiveActionDto>;

public record UpdateCorrectiveActionCommand(
    Guid Id,
    string Title,
    string Description,
    string? RootCause = null,
    string? ActionPlan = null,
    string? VerificationMethod = null,
    int? Priority = null,
    DateTime? DueDate = null) : ICommand<CorrectiveActionDto>;

public record CompleteCorrectiveActionCommand(Guid Id, string? VerificationNotes = null) : ICommand;

public record VerifyCorrectiveActionCommand(Guid Id, Guid UserId, string? VerificationNotes = null) : ICommand;

public record RejectCorrectiveActionCommand(Guid Id, Guid UserId, string? VerificationNotes = null) : ICommand;

public record ExtendCorrectiveActionDueDateCommand(Guid Id, DateTime NewDueDate, string? Reason = null) : ICommand;