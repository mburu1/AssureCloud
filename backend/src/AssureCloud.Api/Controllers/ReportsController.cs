using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Reports;
using AssureCloud.Application.DTOs.Reports;
using AssureCloud.Application.Queries.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Api.Controllers;

[ApiController]
[Route("api/v1/reports")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<ReportDto>>> GetReports(
        [FromQuery] GetReportsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReportDto>> GetReport(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetReportByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ReportDto>> CreateReport(
        [FromBody] CreateReportCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetReport), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ReportDto>> UpdateReport(
        Guid id,
        [FromBody] UpdateReportCommand command,
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
    public async Task<ActionResult> DeleteReport(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteReportCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/generate")]
    public async Task<ActionResult<ReportDto>> GenerateReport(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new GenerateReportCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/review")]
    public async Task<ActionResult> ReviewReport(
        Guid id,
        [FromBody] ReviewReportCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.ReportId)
        {
            return BadRequest("Report ID mismatch");
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<ActionResult> ApproveReport(
        Guid id,
        [FromBody] ApproveReportCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.ReportId)
        {
            return BadRequest("Report ID mismatch");
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet("{id:guid}/download")]
    public async Task<ActionResult> DownloadReport(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new DownloadReportQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return File(result.Content, result.ContentType, result.FileName);
    }
}