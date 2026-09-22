using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Certifications;
using AssureCloud.Application.DTOs.Certifications;
using AssureCloud.Application.Queries.Certifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Api.Controllers;

[ApiController]
[Route("api/v1/certifications")]
[Authorize]
public class CertificationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CertificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<CertificationDto>>> GetCertifications(
        [FromQuery] GetCertificationsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CertificationDto>> GetCertification(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetCertificationByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/decisions")]
    public async Task<ActionResult<PagedResult<CertificationDecisionDto>>> GetCertificationDecisions(
        Guid id,
        [FromQuery] GetCertificationDecisionsQuery query,
        CancellationToken cancellationToken)
    {
        query = query with { CertificationId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/scopes")]
    public async Task<ActionResult<PagedResult<CertificationScopeDto>>> GetCertificationScopes(
        Guid id,
        [FromQuery] GetCertificationScopesQuery query,
        CancellationToken cancellationToken)
    {
        query = query with { CertificationId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CertificationDto>> CreateCertification(
        [FromBody] CreateCertificationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCertification), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CertificationDto>> UpdateCertification(
        Guid id,
        [FromBody] UpdateCertificationCommand command,
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
    public async Task<ActionResult> DeleteCertification(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCertificationCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/issue")]
    public async Task<ActionResult> IssueCertification(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new IssueCertificationCommand(id);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/suspend")]
    public async Task<ActionResult> SuspendCertification(
        Guid id,
        [FromBody] SuspendCertificationCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.CertificationId)
        {
            return BadRequest("Certification ID mismatch");
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/revoke")]
    public async Task<ActionResult> RevokeCertification(
        Guid id,
        [FromBody] RevokeCertificationCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.CertificationId)
        {
            return BadRequest("Certification ID mismatch");
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/renew")]
    public async Task<ActionResult> RenewCertification(
        Guid id,
        [FromBody] RenewCertificationCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.CertificationId)
        {
            return BadRequest("Certification ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCertification), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/decisions")]
    public async Task<ActionResult<CertificationDecisionDto>> AddDecision(
        Guid id,
        [FromBody] AddCertificationDecisionCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.CertificationId)
        {
            return BadRequest("Certification ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCertification), new { id }, result);
    }

    [HttpPost("{id:guid}/scopes")]
    public async Task<ActionResult<CertificationScopeDto>> AddScope(
        Guid id,
        [FromBody] AddCertificationScopeCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.CertificationId)
        {
            return BadRequest("Certification ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetCertification), new { id }, result);
    }
}