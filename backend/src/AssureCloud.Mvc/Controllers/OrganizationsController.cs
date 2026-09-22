using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Organizations;
using AssureCloud.Application.DTOs.Organizations;
using AssureCloud.Application.Queries.Organizations;
using AssureCloud.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Mvc.Controllers;

[Authorize]
public class OrganizationsController : Controller
{
    private readonly ApiClient _apiClient;

    public OrganizationsController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index(GetOrganizationsQuery query, CancellationToken ct)
    {
        var result = await _apiClient.GetOrganizationsAsync(query, ct);
        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var organization = await _apiClient.GetOrganizationAsync(id, ct);
        if (organization == null)
        {
            return NotFound();
        }
        return View(organization);
    }

    public IActionResult Create()
    {
        return View(new CreateOrganizationCommand());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrganizationCommand command, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _apiClient.CreateOrganizationAsync(command, ct);
            if (result != null)
            {
                TempData["Success"] = "Organization created successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to create organization.");
        }
        return View(command);
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var organization = await _apiClient.GetOrganizationAsync(id, ct);
        if (organization == null)
        {
            return NotFound();
        }

        var command = new UpdateOrganizationCommand
        {
            Id = organization.Id,
            Name = organization.Name,
            Description = organization.Description,
            RegistrationNumber = organization.RegistrationNumber,
            TaxId = organization.TaxId,
            Website = organization.Website,
            LogoUrl = organization.LogoUrl,
            ParentOrganizationId = organization.ParentOrganizationId,
            Status = organization.Status
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateOrganizationCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _apiClient.UpdateOrganizationAsync(id, command, ct);
            if (result != null)
            {
                TempData["Success"] = "Organization updated successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to update organization.");
        }
        return View(command);
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var organization = await _apiClient.GetOrganizationAsync(id, ct);
        if (organization == null)
        {
            return NotFound();
        }
        return View(organization);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken ct)
    {
        await _apiClient.DeleteOrganizationAsync(id, ct);
        TempData["Success"] = "Organization deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}