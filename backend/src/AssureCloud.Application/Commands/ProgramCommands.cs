using System;
using AssureCloud.Application.Commands;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Commands.Programs;

public record CreateProgramCommand(
    string Name,
    string? Description = null,
    string? Code = null,
    string? Version = null,
    DateTime? EffectiveDate = null,
    DateTime? ExpirationDate = null,
    Guid? OwnerId = null) : ICommand<ProgramDto>;

public record UpdateProgramCommand(
    Guid Id,
    string Name,
    string? Description = null,
    string? Code = null,
    string? LogoUrl = null) : ICommand<ProgramDto>;

public record DeleteProgramCommand(Guid Id) : ICommand;

public record UpdateProgramStatusCommand(Guid Id, string Status) : ICommand;

public record SetProgramDatesCommand(Guid Id, DateTime? EffectiveDate, DateTime? ExpirationDate) : ICommand;

public record SetProgramVersionCommand(Guid Id, string Version) : ICommand;

public record SetProgramOwnerCommand(Guid Id, Guid OwnerId) : ICommand;

public record CreateStandardCommand(
    Guid ProgramId,
    string Name,
    string? Description = null,
    string? Code = null,
    int Order = 0,
    DateTime? EffectiveDate = null,
    DateTime? ExpirationDate = null) : ICommand<StandardDto>;

public record UpdateStandardCommand(
    Guid Id,
    string Name,
    string? Description = null,
    string? Code = null,
    int Order = 0) : ICommand<StandardDto>;

public record DeleteStandardCommand(Guid ProgramId, Guid StandardId) : ICommand;

public record UpdateStandardStatusCommand(Guid Id, string Status) : ICommand;

public record SetStandardVersionCommand(Guid Id, string Version) : ICommand;

public record SetStandardDatesCommand(Guid Id, DateTime? EffectiveDate, DateTime? ExpirationDate) : ICommand;

public record ReorderStandardsCommand(Guid ProgramId, Guid[] StandardIdsInOrder) : ICommand;

public record CreateRequirementCommand(
    Guid StandardId,
    string Name,
    string? Description = null,
    string? Code = null,
    int Order = 0,
    string Type = "Standard",
    bool IsMandatory = true,
    decimal Weight = 1.0m,
    string? Guidance = null,
    string? ReferenceUrl = null) : ICommand<RequirementDto>;

public record UpdateRequirementCommand(
    Guid Id,
    string Name,
    string? Description = null,
    string? Code = null,
    int Order = 0,
    string? Type = null,
    bool? IsMandatory = null,
    decimal? Weight = null,
    string? Guidance = null,
    string? ReferenceUrl = null) : ICommand<RequirementDto>;

public record DeleteRequirementCommand(Guid StandardId, Guid RequirementId) : ICommand;

public record UpdateRequirementStatusCommand(Guid Id, string Status) : ICommand;

public record ReorderRequirementsCommand(Guid StandardId, Guid[] RequirementIdsInOrder) : ICommand;

public record CreateCriterionCommand(
    Guid RequirementId,
    string Name,
    string? Description = null,
    string? Code = null,
    int Order = 0,
    string ScoringMethod = "Percentage",
    decimal MaxScore = 100,
    decimal PassThreshold = 70,
    bool IsCritical = false,
    string? Guidance = null,
    string? ReferenceUrl = null) : ICommand<CriterionDto>;

public record UpdateCriterionCommand(
    Guid Id,
    string Name,
    string? Description = null,
    string? Code = null,
    int Order = 0,
    string? ScoringMethod = null,
    decimal? MaxScore = null,
    decimal? PassThreshold = null,
    bool? IsCritical = null,
    string? Guidance = null,
    string? ReferenceUrl = null) : ICommand<CriterionDto>;

public record DeleteCriterionCommand(Guid RequirementId, Guid CriterionId) : ICommand;

public record ReorderCriteriaCommand(Guid RequirementId, Guid[] CriterionIdsInOrder) : ICommand;

public record CreateControlCommand(
    Guid CriterionId,
    string Name,
    string? Description = null,
    string? Code = null,
    string Type = "Preventive",
    int Frequency = 1,
    string? FrequencyUnit = null,
    string? ResponsibleRole = null,
    string? EvidenceRequirements = null,
    string? TestProcedure = null,
    decimal Weight = 1.0m) : ICommand<ControlDto>;

public record UpdateControlCommand(
    Guid Id,
    string Name,
    string? Description = null,
    string? Code = null,
    string? Type = null,
    string? Status = null,
    int? Frequency = null,
    string? FrequencyUnit = null,
    string? ResponsibleRole = null,
    string? EvidenceRequirements = null,
    string? TestProcedure = null,
    decimal? Weight = null) : ICommand<ControlDto>;

public record DeleteControlCommand(Guid CriterionId, Guid ControlId) : ICommand;

public record UpdateControlStatusCommand(Guid Id, string Status) : ICommand;