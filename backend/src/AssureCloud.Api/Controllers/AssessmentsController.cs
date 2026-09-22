using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Assessments;
using AssureCloud.Application.DTOs.Assessments;
using AssureCloud.Application.Queries.Assessments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Api.Controllers;

[ApiController]
[Route("api/v1/assessments")]
[Authorize]
public class AssessmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AssessmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<AssessmentDto>>> GetAssessments(
        [FromQuery] GetAssessmentsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AssessmentDto>> GetAssessment(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetAssessmentByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/responses")]
    public async Task<ActionResult<PagedResult<AssessmentResponseDto>>> GetAssessmentResponses(
        Guid id,
        [FromQuery] GetAssessmentResponsesQuery query,
        CancellationToken cancellationToken)
    {
        query = query with { AssessmentId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/evidence")]
    public async Task<ActionResult<PagedResult<EvidenceDto>>> GetAssessmentEvidence(
        Guid id,
        [FromQuery] GetEvidenceQuery query,
        CancellationToken cancellationToken)
    {
        query = query with { AssessmentId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/findings")]
    public async Task<ActionResult<PagedResult<FindingDto>>> GetAssessmentFindings(
        Guid id,
        [FromQuery] GetFindingsQuery query,
        CancellationToken cancellationToken)
    {
        query = query with { AssessmentId = id };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<AssessmentDto>> CreateAssessment(
        [FromBody] CreateAssessmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAssessment), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<AssessmentDto>> UpdateAssessment(
        Guid id,
        [FromBody] UpdateAssessmentCommand command,
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
    public async Task<ActionResult> DeleteAssessment(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAssessmentCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/start")]
    public async Task<ActionResult> StartAssessment(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new StartAssessmentCommand(id);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<ActionResult> CompleteAssessment(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CompleteAssessmentCommand(id);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/responses")]
    public async Task<ActionResult<AssessmentResponseDto>> SubmitResponse(
        Guid id,
        [FromBody] SubmitAssessmentResponseCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.AssessmentId)
        {
            return BadRequest("Assessment ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAssessment), new { id }, result);
    }

    [HttpPost("{id:guid}/evidence")]
    public async Task<ActionResult<EvidenceDto>> UploadEvidence(
        Guid id,
        [FromBody] UploadEvidenceCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.AssessmentId)
        {
            return BadRequest("Assessment ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAssessment), new { id }, result);
    }

    [HttpPost("{id:guid}/findings")]
    public async Task<ActionResult<FindingDto>> CreateFinding(
        Guid id,
        [FromBody] CreateFindingCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.AssessmentId)
        {
            return BadRequest("Assessment ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAssessment), new { id }, result);
    }

    [HttpPost("{id:guid}/assignments")]
    public async Task<ActionResult<AssessmentAssignmentDto>> AssignAssessment(
        Guid id,
        [FromBody] AssignAssessmentCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.AssessmentId)
        {
            return BadRequest("Assessment ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAssessment), new { id }, result);
    }
}