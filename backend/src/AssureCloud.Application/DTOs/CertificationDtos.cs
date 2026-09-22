using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssureCloud.Application.DTOs;

public class CertificationDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public Guid ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public Guid? AssessmentId { get; set; }
    public string? CertificateNumber { get; set; }
    public string? Title { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? ApplicationDate { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? SuspensionDate { get; set; }
    public string? SuspensionReason { get; set; }
    public DateTime? RevocationDate { get; set; }
    public string? RevocationReason { get; set; }
    public Guid? IssuedById { get; set; }
    public string? IssuedByName { get; set; }
    public string? CertificateUrl { get; set; }
    public int SurveillanceIntervalMonths { get; set; }
    public DateTime? NextSurveillanceDue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int DecisionsCount { get; set; }
    public int ScopesCount { get; set; }
}

public class CertificationListDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public Guid ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public string? CertificateNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsExpiringSoon { get; set; }
}

public class CreateCertificationDto
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    public Guid ProgramId { get; set; }

    public Guid? AssessmentId { get; set; }
    [MaxLength(200)]
    public string? Title { get; set; }
    [MaxLength(50)]
    public string? CertificateNumber { get; set; }
    public int SurveillanceIntervalMonths { get; set; } = 12;
}

public class UpdateCertificationDto
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(50)]
    public string? CertificateNumber { get; set; }

    public int? SurveillanceIntervalMonths { get; set; }
}

public class CertificationDecisionDto
{
    public Guid Id { get; set; }
    public Guid CertificationId { get; set; }
    public string Decision { get; set; } = string.Empty;
    public string Rationale { get; set; } = string.Empty;
    public Guid DecidedById { get; set; }
    public string? DecidedByName { get; set; }
    public DateTime DecidedAt { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateCertificationDecisionDto
{
    [Required]
    [MaxLength(500)]
    public string Decision { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Rationale { get; set; } = string.Empty;

    [Required]
    public Guid DecidedById { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty;
}

public class CertificationScopeDto
{
    public Guid Id { get; set; }
    public Guid CertificationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid StandardId { get; set; }
    public string? StandardName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateCertificationScopeDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public Guid StandardId { get; set; }
}

public class CertificationSummaryDto
{
    public Guid Id { get; set; }
    public string? CertificateNumber { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? IssueDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? NextSurveillanceDue { get; set; }
    public bool IsActive { get; set; }
    public bool IsExpired { get; set; }
    public bool IsExpiringSoon { get; set; }
    public bool IsSuspended { get; set; }
    public int DaysUntilExpiration { get; set; }
    public int DaysUntilSurveillance { get; set; }
}