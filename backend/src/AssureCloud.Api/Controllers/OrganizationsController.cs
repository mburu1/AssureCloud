using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Organizations;
using AssureCloud.Application.DTOs.Organizations;
using AssureCloud.Application.Queries.Organizations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Api.Controllers;

[ApiController]
[Route("api/v1/organizations")]
[Authorize]
public class OrganizationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<OrganizationDto>>> GetOrganizations(
        [FromQuery] GetOrganizationsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrganizationDto>> GetOrganization(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetOrganizationByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OrganizationDto>> CreateOrganization(
        [FromBody] CreateOrganizationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOrganization), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<OrganizationDto>> UpdateOrganization(
        Guid id,
        [FromBody] UpdateOrganizationCommand command,
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
    public async Task<ActionResult> DeleteOrganization(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteOrganizationCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/locations")]
    public async Task<ActionResult<OrganizationLocationDto>> AddLocation(
        Guid id,
        [FromBody] AddOrganizationLocationCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.OrganizationId)
        {
            return BadRequest("Organization ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOrganization), new { id }, result);
    }

    [HttpPut("{id:guid}/locations/{locationId:guid}")]
    public async Task<ActionResult<OrganizationLocationDto>> UpdateLocation(
        Guid id,
        Guid locationId,
        [FromBody] UpdateOrganizationLocationCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.OrganizationId || locationId != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}/locations/{locationId:guid}")]
    public async Task<ActionResult> DeleteLocation(
        Guid id,
        Guid locationId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteOrganizationLocationCommand(id, locationId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/suppliers")]
    public async Task<ActionResult<SupplierDto>> AddSupplier(
        Guid id,
        [FromBody] AddSupplierCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.OrganizationId)
        {
            return BadRequest("Organization ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetOrganization), new { id }, result);
    }

    [HttpPut("{id:guid}/suppliers/{supplierId:guid}")]
    public async Task<ActionResult<SupplierDto>> UpdateSupplier(
        Guid id,
        Guid supplierId,
        [FromBody] UpdateSupplierCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.OrganizationId || supplierId != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}/suppliers/{supplierId:guid}")]
    public async Task<ActionResult> DeleteSupplier(
        Guid id,
        Guid supplierId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSupplierCommand(id, supplierId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}