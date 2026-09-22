using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Assessments;
using AssureCloud.Application.DTOs.Assessments;
using AssureCloud.Application.Queries.Assessments;
using AssureCloud.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Mvc.Controllers;

[Authorize]
public class AssessmentsController : Controller
{
    private readonly ApiClient _apiClient;

    public AssessmentsController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index(GetAssessmentsQuery query, CancellationToken ct)
    {
        var result = await _apiClient.GetAssessmentsAsync(query, ct);
        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var assessment = await _apiClient.GetAssessmentAsync(id, ct);
        if (assessment == null)
        {
            return NotFound();
        }
        return View(assessment);
    }

    public IActionResult Create()
    {
        return View(new CreateAssessmentCommand());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAssessmentCommand command, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _apiClient.CreateAssessmentAsync(command, ct);
            if (result != null)
            {
                TempData["Success"] = "Assessment created successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to create assessment.");
        }
        return View(command);
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var assessment = await _apiClient.GetAssessmentAsync(id, ct);
        if (assessment == null)
        {
            return NotFound();
        }

        var command = new UpdateAssessmentCommand
        {
            Id = assessment.Id,
            Title = assessment.Title,
            Description = assessment.Description,
            ProgramId = assessment.ProgramId,
            OrganizationId = assessment.OrganizationId,
            AssessorId = assessment.AssessorId,
            Status = assessment.Status,
            ScheduledStartDate = assessment.ScheduledStartDate,
            ScheduledEndDate = assessment.ScheduledEndDate,
            Scope = assessment.Scope,
            Methodology = assessment.Methodology
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateAssessmentCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _apiClient.UpdateAssessmentAsync(id, command, ct);
            if (result != null)
            {
                TempData["Success"] = "Assessment updated successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to update assessment.");
        }
        return View(command);
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var assessment = await _apiClient.GetAssessmentAsync(id, ct);
        if (assessment == null)
        {
            return NotFound();
        }
        return View(assessment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken ct)
    {
        await _apiClient.DeleteAssessmentAsync(id, ct);
        TempData["Success"] = "Assessment deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}