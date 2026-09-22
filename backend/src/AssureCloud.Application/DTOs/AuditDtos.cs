using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssureCloud.Application.DTOs;

public class AuditDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public Guid? AssessmentId { get; set; }
    public string? AssessmentTitle { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime PlannedStartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public DateTime? ActualStartDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public Guid? LeadAuditorId { get; set; }
    public string? LeadAuditorName { get; set; }
    public string? Scope { get; set; }
    public string? Criteria { get; set; }
    public string? ReportUrl { get; set; }
    public decimal? OverallScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int FindingsCount { get; set; }
    public int OpenFindingsCount { get; set; }
    public int CorrectiveActionsCount { get; set; }
    public int OverdueCorrectiveActionsCount { get; set; }
}

public class AuditListDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime PlannedStartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }
    public Guid? LeadAuditorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int FindingsCount { get; set; }
}

public class CreateAuditDto
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    public DateTime PlannedStartDate { get; set; }

    [Required]
    public DateTime PlannedEndDate { get; set; }

    public Guid? AssessmentId { get; set; }
    [MaxLength(200)]
    public string? Title { get; set; }
    [MaxLength(1000)]
    public string? Description { get; set; }
    [MaxLength(2000)]
    public string? Scope { get; set; }
    [MaxLength(2000)]
    public string? Criteria { get; set; }
}

public class UpdateAuditDto
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public string? Type { get; set; }

    public DateTime? PlannedStartDate { get; set; }
    public DateTime? PlannedEndDate { get; set; }
    [MaxLength(2000)]
    public string? Scope { get; set; }
    [MaxLength(2000)]
    public string? Criteria { get; set; }
}

public class AuditFindingDto
{
    public Guid Id { get; set; }
    public Guid AuditId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Guid? CriterionId { get; set; }
    public string? CriterionName { get; set; }
    public string? Requirement { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RootCause { get; set; }
    public string? Impact { get; set; }
    public string? Recommendation { get; set; }
    public Guid? IdentifiedById { get; set; }
    public DateTime? IdentifiedAt { get; set; }
    public Guid? ResolvedById { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? ResolutionNotes { get; set; }
    public CorrectiveActionDto? CorrectiveAction { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateAuditFindingDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Severity { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    public Guid? CriterionId { get; set; }
    [MaxLength(500)]
    public string? Requirement { get; set; }
    public string? RootCause { get; set; }
    public string? Impact { get; set; }
    public string? Recommendation { get; set; }
}

public class CorrectiveActionDto
{
    public Guid Id { get; set; }
    public Guid AuditId { get; set; }
    public Guid FindingId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty
;
    public string? RootCause { get; set; }
    public Guid ResponsiblePartyId { get; set; }
    public string? ResponsiblePartyName { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ActionPlan { get; set; }
    public string? VerificationMethod { get; set; }
    public string? VerificationNotes { get; set; }
    public Guid? VerifiedById { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateCorrectiveActionDto
{
    [Required]
    public Guid FindingId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid ResponsiblePartyId { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [MaxLength(2000)]
    public string? RootCause { get; set; }

    [MaxLength(2000)]
    public string? ActionPlan { get; set; }

    [MaxLength(1000)]
    public string? VerificationMethod { get; set; }

    [Range(1, 5)]
    public int Priority { get; set; } = 3;
}

public class AuditAssignmentDto
{
    public Guid Id { get; set; }
    public Guid AuditId { get; set; }
    public Guid AuditorId { get; set; }
    public string AuditorName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
}

public class AssignAuditorDto
{
    [Required]
    public Guid AuditorId { get; set; }

    [Required]
    public string Role { get; set; } = "Auditor";
}

public class AuditSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal? OverallScore { get; set; }
    public int TotalFindings { get; set; }
    public int OpenFindings { get; set; }
    public int CriticalFindings { get; set; }
    public int MajorFindings { get; set; }
    public int MinorFindings { get; set; }
    public int TotalCorrectiveActions { get; set; }
    public int CompletedCorrectiveActions { get; set; }
    public int OverdueCorrectiveActions { get; set; }
    public DateTime? ActualEndDate { get; set; }
}