using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Reports;
using AssureCloud.Application.DTOs.Reports;
using AssureCloud.Application.Queries.Reports;
using AssureCloud.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Mvc.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly ApiClient _apiClient;

    public ReportsController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index(GetReportsQuery query, CancellationToken ct)
    {
        var result = await _apiClient.GetReportsAsync(query, ct);
        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var report = await _apiClient.GetReportAsync(id, ct);
        if (report == null)
        {
            return NotFound();
        }
        return View(report);
    }

    public IActionResult Create()
    {
        return View(new CreateReportCommand());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateReportCommand command, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _apiClient.CreateReportAsync(command, ct);
            if (result != null)
            {
                TempData["Success"] = "Report created successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to create report.");
        }
        return View(command);
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var report = await _apiClient.GetReportAsync(id, ct);
        if (report == null)
        {
            return NotFound();
        }

        var command = new UpdateReportCommand
        {
            Id = report.Id,
            Title = report.Title,
            Description = report.Description,
            Type = report.Type,
            Format = report.Format,
            OrganizationId = report.OrganizationId,
            ProgramId = report.ProgramId,
            AssessmentId = report.AssessmentId,
            AuditId = report.AuditId,
            CertificationId = report.CertificationId,
            TemplateName = report.TemplateName,
            Status = report.Status
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateReportCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _apiClient.UpdateReportAsync(id, command, ct);
            if (result != null)
            {
                TempData["Success"] = "Report updated successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to update report.");
        }
        return View(command);
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var report = await _apiClient.GetReportAsync(id, ct);
        if (report == null)
        {
            return NotFound();
        }
        return View(report);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken ct)
    {
        await _apiClient.DeleteReportAsync(id, ct);
        TempData["Success"] = "Report deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}