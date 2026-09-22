using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Audits;
using AssureCloud.Application.DTOs.Audits;
using AssureCloud.Application.Queries.Audits;
using AssureCloud.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Mvc.Controllers;

[Authorize]
public class AuditsController : Controller
{
    private readonly ApiClient _apiClient;

    public AuditsController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index(GetAuditsQuery query, CancellationToken ct)
    {
        var result = await _apiClient.GetAuditsAsync(query, ct);
        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var audit = await _apiClient.GetAuditAsync(id, ct);
        if (audit == null)
        {
            return NotFound();
        }
        return View(audit);
    }

    public IActionResult Create()
    {
        return View(new CreateAuditCommand());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAuditCommand command, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _apiClient.CreateAuditAsync(command, ct);
            if (result != null)
            {
                TempData["Success"] = "Audit created successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to create audit.");
        }
        return View(command);
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var audit = await _apiClient.GetAuditAsync(id, ct);
        if (audit == null)
        {
            return NotFound();
        }

        var command = new UpdateAuditCommand
        {
            Id = audit.Id,
            Title = audit.Title,
            Description = audit.Description,
            ProgramId = audit.ProgramId,
            OrganizationId = audit.OrganizationId,
            LeadAuditorId = audit.LeadAuditorId,
            Status = audit.Status,
            Type = audit.Type,
            ScheduledStartDate = audit.ScheduledStartDate,
            ScheduledEndDate = audit.ScheduledEndDate,
            Scope = audit.Scope,
            Criteria = audit.Criteria
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateAuditCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _apiClient.UpdateAuditAsync(id, command, ct);
            if (result != null)
            {
                TempData["Success"] = "Audit updated successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to update audit.");
        }
        return View(command);
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var audit = await _apiClient.GetAuditAsync(id, ct);
        if (audit == null)
        {
            return NotFound();
        }
        return View(audit);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken ct)
    {
        await _apiClient.DeleteAuditAsync(id, ct);
        TempData["Success"] = "Audit deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}