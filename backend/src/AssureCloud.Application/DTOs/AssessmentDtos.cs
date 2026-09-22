using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssureCloud.Application.DTOs;

public class AssessmentDto
{
    public Guid Id { get; set; }
    public Guid ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public Guid OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public Guid StandardId { get; set; }
    public string? StandardName { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal? Score { get; set; }
    public string? AssessorNotes { get; set; }
    public string? ReviewerNotes { get; set; }
    public Guid? LeadAssessorId { get; set; }
    public string? LeadAssessorName { get; set; }
    public Guid? ReviewerId { get; set; }
    public string? ReviewerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ResponsesCount { get; set; }
    public int EvidenceCount { get; set; }
    public int FindingsCount { get; set; }
    public int OpenFindingsCount { get; set; }
}

public class AssessmentListDto
{
    public Guid Id { get; set; }
    public Guid ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public Guid OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public decimal? Score { get; set; }
    public Guid? LeadAssessorId { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateAssessmentDto
{
    [Required]
    public Guid ProgramId { get; set; }

    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid StandardId { get; set; }

    [Required]
    public DateTime ScheduledDate { get; set; }

    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}

public class UpdateAssessmentDto
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime? ScheduledDate { get; set; }

    [MaxLength(1000)]
    public string? AssessorNotes { get; set; }
}

public class AssessmentResponseDto
{
    public Guid Id { get; set; }
    public Guid AssessmentId { get; set; }
    public Guid CriterionId { get; set; }
    public string? CriterionName { get; set; }
    public decimal Score { get; set; }
    public string? Comments { get; set; }
    public bool IsManual { get; set; }
    public DateTime? RespondedAt { get; set; }
    public Guid? RespondedById { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateAssessmentResponseDto
{
    [Required]
    public Guid CriterionId { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Score { get; set; }

    [MaxLength(2000)]
    public string? Comments { get; set; }

    public bool IsManual { get; set; }
}

public class EvidenceDto
{
    public Guid Id { get; set; }
    public Guid AssessmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string FileUrl { get; set; } = string.Empty;
    public string? MimeType { get; set; }
    public long? FileSize { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid? SubmittedById { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public Guid? VerifiedById { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public string? VerificationNotes { get; set; }
    public int Version { get; set; }
    public Guid? PreviousVersionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateEvidenceDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(500)]
    public string FileUrl { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? MimeType { get; set; }

    public long? FileSize { get; set; }
}

public class FindingDto
{
    public Guid Id { get; set; }
    public Guid AssessmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public Guid? CriterionId { get; set; }
    public string? CriterionName { get; set; }
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

public class CreateFindingDto
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
    public string? RootCause { get; set; }
    public string? Impact { get; set; }
    public string? Recommendation { get; set; }
}

public class AssessmentAssignmentDto
{
    public Guid Id { get; set; }
    public Guid AssessmentId { get; set; }
    public Guid AssessorId { get; set; }
    public string AssessorName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
    public DateTime? RemovedAt { get; set; }
}

public class AssignAssessorDto
{
    [Required]
    public Guid AssessorId { get; set; }

    [Required]
    public string Role { get; set; } = "Assessor";
}

public class AssessmentSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal? Score { get; set; }
    public int TotalCriteria { get; set; }
    public int RespondedCriteria { get; set; }
    public int TotalEvidence { get; set; }
    public int VerifiedEvidence { get; set; }
    public int TotalFindings { get; set; }
    public int OpenFindings { get; set; }
    public int CriticalFindings { get; set; }
    public int HighFindings { get; set; }
    public int MediumFindings { get; set; }
    public int LowFindings { get; set; }
    public DateTime? CompletedDate { get; set; }
}