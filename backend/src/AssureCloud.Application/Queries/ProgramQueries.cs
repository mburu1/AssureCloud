using System;
using AssureCloud.Application.Queries;
using AssureCloud.Application.DTOs;

namespace AssureCloud.Application.Queries.Programs;

public record GetProgramByIdQuery(Guid Id) : IQuery<ProgramDto?>;

public record GetProgramsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? SearchTerm = null,
    string? Status = null) : IQuery<PaginatedResult<ProgramListDto>>;

public record GetStandardsByProgramQuery(Guid ProgramId) : IQuery<IReadOnlyList<StandardDto>>;

public record GetStandardByIdQuery(Guid ProgramId, Guid StandardId) : IQuery<StandardDto?>;

public record GetRequirementsByStandardQuery(Guid StandardId) : IQuery<IReadOnlyList<RequirementDto>>;

public record GetRequirementByIdQuery(Guid StandardId, Guid RequirementId) : IQuery<RequirementDto?>;

public record GetCriteriaByRequirementQuery(Guid RequirementId) : IQuery<IReadOnlyList<CriterionDto>>;

public record GetCriterionByIdQuery(Guid RequirementId, Guid CriterionId) : IQuery<CriterionDto?>;

public record GetControlsByCriterionQuery(Guid CriterionId) : IQuery<IReadOnlyList<ControlDto>>;

public record GetControlByIdQuery(Guid CriterionId, Guid ControlId) : IQuery<ControlDto?>;