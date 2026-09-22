using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssureCloud.Application.DTOs;

public class ReportDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public Guid? ProgramId { get; set; }
    public string? ProgramName { get; set; }
    public Guid? AssessmentId { get; set; }
    public string? AssessmentTitle { get; set; }
    public Guid? AuditId { get; set; }
    public string? AuditTitle { get; set; }
    public Guid? CertificationId { get; set; }
    public string? CertificateNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public string? MimeType { get; set; }
    public long? FileSize { get; set; }
    public Guid? GeneratedById { get; set; }
    public string? GeneratedByName { get; set; }
    public DateTime? GeneratedAt { get; set; }
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public string? Parameters { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ReportListDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? GeneratedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateReportDto
{
    [Required]
    public Guid OrganizationId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    public Guid? ProgramId { get; set; }
    public Guid? AssessmentId { get; set; }
    public Guid? AuditId { get; set; }
    public Guid? CertificationId { get; set; }
    [MaxLength(1000)]
    public string? Description { get; set; }
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public string? Parameters { get; set; }
}

public class GenerateReportDto
{
    public string? Parameters { get; set; }
}

public class ReportSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? GeneratedAt { get; set; }
    public string? FileUrl { get; set; }
}