using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Programs;
using AssureCloud.Application.DTOs.Programs;
using AssureCloud.Application.Queries.Programs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Api.Controllers;

[ApiController]
[Route("api/v1/programs")]
[Authorize]
public class ProgramsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProgramsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ProgramDto>>> GetPrograms(
        [FromQuery] GetProgramsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProgramDto>> GetProgram(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetProgramByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/standards")]
    public async Task<ActionResult<PagedResult<StandardDto>>> GetProgramStandards(
        Guid id,
        [FromQuery] GetStandardsQuery query,
        CancellationToken cancellationToken)
    {
        query = query with { ProgramId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProgramDto>> CreateProgram(
        [FromBody] CreateProgramCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProgram), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ProgramDto>> UpdateProgram(
        Guid id,
        [FromBody] UpdateProgramCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteProgram(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProgramCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/standards")]
    public async Task<ActionResult<StandardDto>> AddStandard(
        Guid id,
        [FromBody] AddStandardCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.ProgramId)
        {
            return BadRequest("Program ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProgram), new { id }, result);
    }

    [HttpPost("standards/{standardId:guid}/requirements")]
    public async Task<ActionResult<RequirementDto>> AddRequirement(
        Guid standardId,
        [FromBody] AddRequirementCommand command,
        CancellationToken cancellationToken)
    {
        if (standardId != command.StandardId)
        {
            return BadRequest("Standard ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProgram), new { id = standardId }, result);
    }

    [HttpPost("requirements/{requirementId:guid}/criteria")]
    public async Task<ActionResult<CriterionDto>> AddCriterion(
        Guid requirementId,
        [FromBody] AddCriterionCommand command,
        CancellationToken cancellationToken)
    {
        if (requirementId != command.RequirementId)
        {
            return BadRequest("Requirement ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProgram), new { id = requirementId }, result);
    }

    [HttpPost("criteria/{criterionId:guid}/controls")]
    public async Task<ActionResult<ControlDto>> AddControl(
        Guid criterionId,
        [FromBody] AddControlCommand command,
        CancellationToken cancellationToken)
    {
        if (criterionId != command.CriterionId)
        {
            return BadRequest("Criterion ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetProgram), new { id = criterionId }, result);
    }
}