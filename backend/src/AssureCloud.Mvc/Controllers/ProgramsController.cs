using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Programs;
using AssureCloud.Application.DTOs.Programs;
using AssureCloud.Application.Queries.Programs;
using AssureCloud.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Mvc.Controllers;

[Authorize]
public class ProgramsController : Controller
{
    private readonly ApiClient _apiClient;

    public ProgramsController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index(GetProgramsQuery query, CancellationToken ct)
    {
        var result = await _apiClient.GetProgramsAsync(query, ct);
        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var program = await _apiClient.GetProgramAsync(id, ct);
        if (program == null)
        {
            return NotFound();
        }
        return View(program);
    }

    public IActionResult Create()
    {
        return View(new CreateProgramCommand());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProgramCommand command, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _apiClient.CreateProgramAsync(command, ct);
            if (result != null)
            {
                TempData["Success"] = "Program created successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to create program.");
        }
        return View(command);
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var program = await _apiClient.GetProgramAsync(id, ct);
        if (program == null)
        {
            return NotFound();
        }

        var command = new UpdateProgramCommand
        {
            Id = program.Id,
            Name = program.Name,
            Description = program.Description,
            Code = program.Code,
            Version = program.Version,
            OrganizationId = program.OrganizationId,
            Status = program.Status,
            EffectiveDate = program.EffectiveDate,
            ExpiryDate = program.ExpiryDate
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateProgramCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _apiClient.UpdateProgramAsync(id, command, ct);
            if (result != null)
            {
                TempData["Success"] = "Program updated successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to update program.");
        }
        return View(command);
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var program = await _apiClient.GetProgramAsync(id, ct);
        if (program == null)
        {
            return NotFound();
        }
        return View(program);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken ct)
    {
        await _apiClient.DeleteProgramAsync(id, ct);
        TempData["Success"] = "Program deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}