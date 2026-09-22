using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Audits;
using AssureCloud.Application.DTOs.Audits;
using AssureCloud.Application.Queries.Audits;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Api.Controllers;

[ApiController]
[Route("api/v1/audits")]
[Authorize]
public class AuditsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuditsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AuditDto>>> GetAudits(
        [FromQuery] GetAuditsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuditDto>> GetAudit(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetAuditByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/findings")]
    public async Task<ActionResult<PagedResult<AuditFindingDto>>> GetAuditFindings(
        Guid id,
        [FromQuery] GetAuditFindingsQuery query,
        CancellationToken cancellationToken)
    {
        query = query with { AuditId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/corrective-actions")]
    public async Task<ActionResult<PagedResult<CorrectiveActionDto>>> GetCorrectiveActions(
        Guid id,
        [FromQuery] GetCorrectiveActionsQuery query,
        CancellationToken cancellationToken)
    {
        query = query with { AuditId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AuditDto>> CreateAudit(
        [FromBody] CreateAuditCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAudit), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AuditDto>> UpdateAudit(
        Guid id,
        [FromBody] UpdateAuditCommand command,
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
    public async Task<ActionResult> DeleteAudit(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAuditCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/start")]
    public async Task<ActionResult> StartAudit(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new StartAuditCommand(id);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<ActionResult> CompleteAudit(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CompleteAuditCommand(id);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/findings")]
    public async Task<ActionResult<AuditFindingDto>> CreateFinding(
        Guid id,
        [FromBody] CreateAuditFindingCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.AuditId)
        {
            return BadRequest("Audit ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAudit), new { id }, result);
    }

    [HttpPost("{id:guid}/corrective-actions")]
    public async Task<ActionResult<CorrectiveActionDto>> CreateCorrectiveAction(
        Guid id,
        [FromBody] CreateCorrectiveActionCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.AuditId)
        {
            return BadRequest("Audit ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAudit), new { id }, result);
    }

    [HttpPost("corrective-actions/{correctiveActionId:guid}/verify")]
    public async Task<ActionResult> VerifyCorrectiveAction(
        Guid correctiveActionId,
        [FromBody] VerifyCorrectiveActionCommand command,
        CancellationToken cancellationToken)
    {
        if (correctiveActionId != command.CorrectiveActionId)
        {
            return BadRequest("Corrective Action ID mismatch");
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/assignments")]
    public async Task<ActionResult<AuditAssignmentDto>> AssignAudit(
        Guid id,
        [FromBody] AssignAuditCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.AuditId)
        {
            return BadRequest("Audit ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAudit), new { id }, result);
    }
}