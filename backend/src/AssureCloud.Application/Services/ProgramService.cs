using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;
using AssureCloud.Application.Commands.Programs;
using AssureCloud.Application.DTOs;
using AssureCloud.Application.Queries.Programs;
using AssureCloud.Domain.Entities;
using AssureCloud.Domain.Enums;
using AutoMapper;
using MediatR;

namespace AssureCloud.Application.Services.Programs;

public class ProgramService :
    ICommandHandler<CreateProgramCommand, ProgramDto>,
    ICommandHandler<UpdateProgramCommand, ProgramDto>,
    ICommandHandler<DeleteProgramCommand>,
    ICommandHandler<UpdateProgramStatusCommand>,
    ICommandHandler<SetProgramDatesCommand>,
    ICommandHandler<SetProgramVersionCommand>,
    ICommandHandler<SetProgramOwnerCommand>,
    ICommandHandler<CreateStandardCommand, StandardDto>,
    ICommandHandler<UpdateStandardCommand, StandardDto>,
    ICommandHandler<DeleteStandardCommand>,
    ICommandHandler<UpdateStandardStatusCommand>,
    ICommandHandler<SetStandardVersionCommand>,
    ICommandHandler<SetStandardDatesCommand>,
    ICommandHandler<ReorderStandardsCommand>,
    ICommandHandler<CreateRequirementCommand, RequirementDto>,
    ICommandHandler<UpdateRequirementCommand, RequirementDto>,
    ICommandHandler<DeleteRequirementCommand>,
    ICommandHandler<UpdateRequirementStatusCommand>,
    ICommandHandler<ReorderRequirementsCommand>,
    ICommandHandler<CreateCriterionCommand, CriterionDto>,
    ICommandHandler<UpdateCriterionCommand, CriterionDto>,
    ICommandHandler<DeleteCriterionCommand>,
    ICommandHandler<ReorderCriteriaCommand>,
    ICommandHandler<CreateControlCommand, ControlDto>,
    ICommandHandler<UpdateControlCommand, ControlDto>,
    ICommandHandler<DeleteControlCommand>,
    ICommandHandler<UpdateControlStatusCommand>,
    IQueryHandler<GetProgramByIdQuery, ProgramDto?>,
    IQueryHandler<GetProgramsQuery, PaginatedResult<ProgramListDto>>,
    IQueryHandler<GetStandardsByProgramQuery, IReadOnlyList<StandardDto>>,
    IQueryHandler<GetStandardByIdQuery, StandardDto?>,
    IQueryHandler<GetRequirementsByStandardQuery, IReadOnlyList<RequirementDto>>,
    IQueryHandler<GetRequirementByIdQuery, RequirementDto?>,
    IQueryHandler<GetCriteriaByRequirementQuery, IReadOnlyList<CriterionDto>>,
    IQueryHandler<GetCriterionByIdQuery, CriterionDto?>,
    IQueryHandler<GetControlsByCriterionQuery, IReadOnlyList<ControlDto>>,
    IQueryHandler<GetControlByIdQuery, ControlDto?>
{
    private readonly IRepository<Program> _programRepository;
    private readonly IRepository<Standard> _standardRepository;
    private readonly IRepository<Requirement> _requirementRepository;
    private readonly IRepository<Criterion> _criterionRepository;
    private readonly IRepository<Control> _controlRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProgramService(
        IRepository<Program> programRepository,
        IRepository<Standard> standardRepository,
        IRepository<Requirement> requirementRepository,
        IRepository<Criterion> criterionRepository,
        IRepository<Control> controlRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _programRepository = programRepository;
        _standardRepository = standardRepository;
        _requirementRepository = requirementRepository;
        _criterionRepository = criterionRepository;
        _controlRepository = controlRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProgramDto> HandleAsync(CreateProgramCommand command, CancellationToken ct = default)
    {
        var program = new Program(command.Name, command.Description, command.Code, command.Version);

        if (command.EffectiveDate.HasValue || command.ExpirationDate.HasValue)
        {
            program.SetDates(command.EffectiveDate, command.ExpirationDate);
        }

        if (command.OwnerId.HasValue)
        {
            program.SetOwner(command.OwnerId.Value);
        }

        await _programRepository.AddAsync(program, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<ProgramDto>(program);
    }

    public async Task<ProgramDto> HandleAsync(UpdateProgramCommand command, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Program));

        program.UpdateDetails(command.Name, command.Description, command.Code, command.LogoUrl);
        await _programRepository.UpdateAsync(program, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<ProgramDto>(program);
    }

    public async Task HandleAsync(DeleteProgramCommand command, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Program));

        program.MarkAsDeleted();
        await _programRepository.UpdateAsync(program, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateProgramStatusCommand command, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Program));

        if (Enum.TryParse<ProgramStatus>(command.Status, out var status))
        {
            program.UpdateStatus(status);
            await _programRepository.UpdateAsync(program, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task HandleAsync(SetProgramDatesCommand command, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Program));

        program.SetDates(command.EffectiveDate, command.ExpirationDate);
        await _programRepository.UpdateAsync(program, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetProgramVersionCommand command, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Program));

        program.SetVersion(command.Version);
        await _programRepository.UpdateAsync(program, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetProgramOwnerCommand command, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Program));

        program.SetOwner(command.OwnerId);
        await _programRepository.UpdateAsync(program, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<StandardDto> HandleAsync(CreateStandardCommand command, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(command.ProgramId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.ProgramId, nameof(Program));

        var standard = program.AddStandard(command.Name, command.Description, command.Code, command.Order);

        if (command.EffectiveDate.HasValue || command.ExpirationDate.HasValue)
        {
            standard.SetDates(command.EffectiveDate, command.ExpirationDate);
        }

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<StandardDto>(standard);
    }

    public async Task<StandardDto> HandleAsync(UpdateStandardCommand command, CancellationToken ct = default)
    {
        var standard = await _standardRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Standard));

        standard.UpdateDetails(command.Name, command.Description, command.Code, command.Order);
        await _standardRepository.UpdateAsync(standard, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<StandardDto>(standard);
    }

    public async Task HandleAsync(DeleteStandardCommand command, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(command.ProgramId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.ProgramId, nameof(Program));

        program.RemoveStandard(command.StandardId);
        await _programRepository.UpdateAsync(program, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateStandardStatusCommand command, CancellationToken ct = default)
    {
        var standard = await _standardRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Standard));

        if (Enum.TryParse<StandardStatus>(command.Status, out var status))
        {
            standard.UpdateStatus(status);
            await _standardRepository.UpdateAsync(standard, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task HandleAsync(SetStandardVersionCommand command, CancellationToken ct = default)
    {
        var standard = await _standardRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Standard));

        standard.SetVersion(command.Version);
        await _standardRepository.UpdateAsync(standard, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(SetStandardDatesCommand command, CancellationToken ct = default)
    {
        var standard = await _standardRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Standard));

        standard.SetDates(command.EffectiveDate, command.ExpirationDate);
        await _standardRepository.UpdateAsync(standard, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(ReorderStandardsCommand command, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(command.ProgramId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.ProgramId, nameof(Program));

        program.ReorderStandards(command.StandardIdsInOrder);
        await _programRepository.UpdateAsync(program, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<RequirementDto> HandleAsync(CreateRequirementCommand command, CancellationToken ct = default)
    {
        var standard = await _standardRepository.GetByIdAsync(command.StandardId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.StandardId, nameof(Standard));

        var requirement = standard.AddRequirement(
            command.Name,
            command.Description,
            command.Code,
            command.Order,
            Enum.Parse<RequirementType>(command.Type));

        requirement.UpdateDetails(
            requirement.Name,
            requirement.Description,
            requirement.Code,
            requirement.Order,
            requirement.Type,
            command.IsMandatory,
            command.Weight,
            command.Guidance,
            command.ReferenceUrl);

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<RequirementDto>(requirement);
    }

    public async Task<RequirementDto> HandleAsync(UpdateRequirementCommand command, CancellationToken ct = default)
    {
        var requirement = await _requirementRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Requirement));

        requirement.UpdateDetails(
            command.Name,
            command.Description,
            command.Code,
            command.Order,
            command.Type != null ? Enum.Parse<RequirementType>(command.Type) : null,
            command.IsMandatory,
            command.Weight,
            command.Guidance,
            command.ReferenceUrl);

        await _requirementRepository.UpdateAsync(requirement, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<RequirementDto>(requirement);
    }

    public async Task HandleAsync(DeleteRequirementCommand command, CancellationToken ct = default)
    {
        var standard = await _standardRepository.GetByIdAsync(command.StandardId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.StandardId, nameof(Standard));

        standard.RemoveRequirement(command.RequirementId);
        await _standardRepository.UpdateAsync(standard, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateRequirementStatusCommand command, CancellationToken ct = default)
    {
        var requirement = await _requirementRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Requirement));

        if (Enum.TryParse<RequirementStatus>(command.Status, out var status))
        {
            requirement.UpdateStatus(status);
            await _requirementRepository.UpdateAsync(requirement, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task HandleAsync(ReorderRequirementsCommand command, CancellationToken ct = default)
    {
        var standard = await _standardRepository.GetByIdAsync(command.StandardId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.StandardId, nameof(Standard));

        standard.ReorderRequirements(command.RequirementIdsInOrder);
        await _standardRepository.UpdateAsync(standard, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<CriterionDto> HandleAsync(CreateCriterionCommand command, CancellationToken ct = default)
    {
        var requirement = await _requirementRepository.GetByIdAsync(command.RequirementId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.RequirementId, nameof(Requirement));

        var criterion = requirement.AddCriterion(
            command.Name,
            command.Description,
            command.Code,
            command.Order,
            Enum.Parse<ScoringMethod>(command.ScoringMethod));

        criterion.UpdateDetails(
            criterion.Name,
            criterion.Description,
            criterion.Code,
            criterion.Order,
            criterion.ScoringMethod,
            command.MaxScore,
            command.PassThreshold,
            command.IsCritical,
            command.Guidance,
            command.ReferenceUrl);

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<CriterionDto>(criterion);
    }

    public async Task<CriterionDto> HandleAsync(UpdateCriterionCommand command, CancellationToken ct = default)
    {
        var criterion = await _criterionRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Criterion));

        criterion.UpdateDetails(
            command.Name,
            command.Description,
            command.Code,
            command.Order,
            command.ScoringMethod != null ? Enum.Parse<ScoringMethod>(command.ScoringMethod) : null,
            command.MaxScore,
            command.PassThreshold,
            command.IsCritical,
            command.Guidance,
            command.ReferenceUrl);

        await _criterionRepository.UpdateAsync(criterion, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<CriterionDto>(criterion);
    }

    public async Task HandleAsync(DeleteCriterionCommand command, CancellationToken ct = default)
    {
        var requirement = await _requirementRepository.GetByIdAsync(command.RequirementId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.RequirementId, nameof(Requirement));

        requirement.RemoveCriterion(command.CriterionId);
        await _requirementRepository.UpdateAsync(requirement, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(ReorderCriteriaCommand command, CancellationToken ct = default)
    {
        var requirement = await _requirementRepository.GetByIdAsync(command.RequirementId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.RequirementId, nameof(Requirement));

        requirement.ReorderCriteria(command.CriterionIdsInOrder);
        await _requirementRepository.UpdateAsync(requirement, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<ControlDto> HandleAsync(CreateControlCommand command, CancellationToken ct = default)
    {
        var criterion = await _criterionRepository.GetByIdAsync(command.CriterionId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.CriterionId, nameof(Criterion));

        var control = criterion.AddControl(
            command.Name,
            command.Description,
            command.Code,
            Enum.Parse<ControlType>(command.Type));

        control.UpdateDetails(
            control.Name,
            control.Description,
            control.Code,
            control.Type,
            null,
            command.Frequency,
            command.FrequencyUnit,
            command.ResponsibleRole,
            command.EvidenceRequirements,
            command.TestProcedure,
            command.Weight);

        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<ControlDto>(control);
    }

    public async Task<ControlDto> HandleAsync(UpdateControlCommand command, CancellationToken ct = default)
    {
        var control = await _controlRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Control));

        control.UpdateDetails(
            command.Name,
            command.Description,
            command.Code,
            command.Type != null ? Enum.Parse<ControlType>(command.Type) : null,
            command.Status != null ? Enum.Parse<ControlStatus>(command.Status) : null,
            command.Frequency,
            command.FrequencyUnit,
            command.ResponsibleRole,
            command.EvidenceRequirements,
            command.TestProcedure,
            command.Weight);

        await _controlRepository.UpdateAsync(control, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<ControlDto>(control);
    }

    public async Task HandleAsync(DeleteControlCommand command, CancellationToken ct = default)
    {
        var criterion = await _criterionRepository.GetByIdAsync(command.CriterionId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.CriterionId, nameof(Criterion));

        criterion.RemoveControl(command.ControlId);
        await _criterionRepository.UpdateAsync(criterion, ct);
        await _unitOfWork.SaveChangesAsync(ct);
    }

    public async Task HandleAsync(UpdateControlStatusCommand command, CancellationToken ct = default)
    {
        var control = await _controlRepository.GetByIdAsync(command.Id, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(command.Id, nameof(Control));

        if (Enum.TryParse<ControlStatus>(command.Status, out var status))
        {
            control.UpdateStatus(status);
            await _controlRepository.UpdateAsync(control, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
        else
        {
            throw new ArgumentException($"Invalid status: {command.Status}");
        }
    }

    public async Task<ProgramDto?> HandleAsync(GetProgramByIdQuery query, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(query.Id, ct);
        return program != null ? _mapper.Map<ProgramDto>(program) : null;
    }

    public async Task<PaginatedResult<ProgramListDto>> HandleAsync(GetProgramsQuery query, CancellationToken ct = default)
    {
        var programs = await _programRepository.ListAsync(ct);

        var filtered = programs.AsQueryable();

        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            filtered = filtered.Where(p =>
                p.Name.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                (p.Description != null && p.Description.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrEmpty(query.Status) && Enum.TryParse<ProgramStatus>(query.Status, out var status))
        {
            filtered = filtered.Where(p => p.Status == status);
        }

        var totalCount = filtered.Count();
        var items = filtered
            .OrderBy(p => p.Name)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var dtos = _mapper.Map<List<ProgramListDto>>(items);

        return new PaginatedResult<ProgramListDto>
        {
            Items = dtos,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<IReadOnlyList<StandardDto>> HandleAsync(GetStandardsByProgramQuery query, CancellationToken ct = default)
    {
        var program = await _programRepository.GetByIdAsync(query.ProgramId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.ProgramId, nameof(Program));

        return _mapper.Map<IReadOnlyList<StandardDto>>(program.Standards);
    }

    public async Task<StandardDto?> HandleAsync(GetStandardByIdQuery query, CancellationToken ct = default)
    {
        var standard = await _standardRepository.GetByIdAsync(query.StandardId, ct);
        return standard != null ? _mapper.Map<StandardDto>(standard) : null;
    }

    public async Task<IReadOnlyList<RequirementDto>> HandleAsync(GetRequirementsByStandardQuery query, CancellationToken ct = default)
    {
        var standard = await _standardRepository.GetByIdAsync(query.StandardId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.StandardId, nameof(Standard));

        return _mapper.Map<IReadOnlyList<RequirementDto>>(standard.Requirements);
    }

    public async Task<RequirementDto?> HandleAsync(GetRequirementByIdQuery query, CancellationToken ct = default)
    {
        var requirement = await _requirementRepository.GetByIdAsync(query.RequirementId, ct);
        return requirement != null ? _mapper.Map<RequirementDto>(requirement) : null;
    }

    public async Task<IReadOnlyList<CriterionDto>> HandleAsync(GetCriteriaByRequirementQuery query, CancellationToken ct = default)
    {
        var requirement = await _requirementRepository.GetByIdAsync(query.RequirementId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.RequirementId, nameof(Requirement));

        return _mapper.Map<IReadOnlyList<CriterionDto>>(requirement.Criteria);
    }

    public async Task<CriterionDto?> HandleAsync(GetCriterionByIdQuery query, CancellationToken ct = default)
    {
        var criterion = await _criterionRepository.GetByIdAsync(query.CriterionId, ct);
        return criterion != null ? _mapper.Map<CriterionDto>(criterion) : null;
    }

    public async Task<IReadOnlyList<ControlDto>> HandleAsync(GetControlsByCriterionQuery query, CancellationToken ct = default)
    {
        var criterion = await _criterionRepository.GetByIdAsync(query.CriterionId, ct)
            ?? throw new AssureCloud.Domain.Exceptions.EntityNotFoundException(query.CriterionId, nameof(Criterion));

        return _mapper.Map<IReadOnlyList<ControlDto>>(criterion.Controls);
    }

    public async Task<ControlDto?> HandleAsync(GetControlByIdQuery query, CancellationToken ct = default)
    {
        var control = await _controlRepository.GetByIdAsync(query.ControlId, ct);
        return control != null ? _mapper.Map<ControlDto>(control) : null;
    }
}