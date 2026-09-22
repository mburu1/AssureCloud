using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AssureCloud.Application.DTOs;

public class ProgramDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? Version { get; set; }
    public Guid? OwnerId { get; set; }
    public string? OwnerName { get; set; }
    public string? LogoUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int StandardsCount { get; set; }
    public int AssessmentsCount { get; set; }
}

public class ProgramListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Version { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public int StandardsCount { get; set; }
}

public class CreateProgramDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Code { get; set; }

    [MaxLength(20)]
    public string? Version { get; set; }

    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public Guid? OwnerId { get; set; }
}

public class UpdateProgramDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Code { get; set; }

    [MaxLength(500)]
    public string? LogoUrl { get; set; }
}

public class StandardDto
{
    public Guid Id { get; set; }
    public Guid ProgramId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public int Order { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Version { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int RequirementsCount { get; set; }
}

public class CreateStandardDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Code { get; set; }

    public int Order { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}

public class RequirementDto
{
    public Guid Id { get; set; }
    public Guid StandardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public int Order { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
    public decimal Weight { get; set; }
    public string? Guidance { get; set; }
    public string? ReferenceUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int CriteriaCount { get; set; }
}

public class CreateRequirementDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Code { get; set; }

    public int Order { get; set; }
    public string Type { get; set; } = "Standard";
    public bool IsMandatory { get; set; } = true;
    public decimal Weight { get; set; } = 1.0m;
    public string? Guidance { get; set; }
    public string? ReferenceUrl { get; set; }
}

public class CriterionDto
{
    public Guid Id { get; set; }
    public Guid RequirementId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public int Order { get; set; }
    public string ScoringMethod { get; set; } = string.Empty;
    public decimal MaxScore { get; set; }
    public decimal PassThreshold { get; set; }
    public bool IsCritical { get; set; }
    public string? Guidance { get; set; }
    public string? ReferenceUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ControlsCount { get; set; }
}

public class CreateCriterionDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Code { get; set; }

    public int Order { get; set; }
    public string ScoringMethod { get; set; } = "Percentage";
    public decimal MaxScore { get; set; } = 100;
    public decimal PassThreshold { get; set; } = 70;
    public bool IsCritical { get; set; }
    public string? Guidance { get; set; }
    public string? ReferenceUrl { get; set; }
}

public class ControlDto
{
    public Guid Id { get; set; }
    public Guid CriterionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Frequency { get; set; }
    public string? FrequencyUnit { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? EvidenceRequirements { get; set; }
    public string? TestProcedure { get; set; }
    public decimal Weight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateControlDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? Code { get; set; }

    public string Type { get; set; } = "Preventive";
    public int Frequency { get; set; } = 1;
    public string? FrequencyUnit { get; set; }
    public string? ResponsibleRole { get; set; }
    public string? EvidenceRequirements { get; set; }
    public string? TestProcedure { get; set; }
    public decimal Weight { get; set; } = 1.0m;
}