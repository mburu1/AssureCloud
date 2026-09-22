using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Certifications;
using AssureCloud.Application.DTOs.Certifications;
using AssureCloud.Application.Queries.Certifications;
using AssureCloud.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Mvc.Controllers;

[Authorize]
public class CertificationsController : Controller
{
    private readonly ApiClient _apiClient;

    public CertificationsController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index(GetCertificationsQuery query, CancellationToken ct)
    {
        var result = await _apiClient.GetCertificationsAsync(query, ct);
        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var certification = await _apiClient.GetCertificationAsync(id, ct);
        if (certification == null)
        {
            return NotFound();
        }
        return View(certification);
    }

    public IActionResult Create()
    {
        return View(new CreateCertificationCommand());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCertificationCommand command, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _apiClient.CreateCertificationAsync(command, ct);
            if (result != null)
            {
                TempData["Success"] = "Certification created successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to create certification.");
        }
        return View(command);
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var certification = await _apiClient.GetCertificationAsync(id, ct);
        if (certification == null)
        {
            return NotFound();
        }

        var command = new UpdateCertificationCommand
        {
            Id = certification.Id,
            CertificateNumber = certification.CertificateNumber,
            ProgramId = certification.ProgramId,
            OrganizationId = certification.OrganizationId,
            AuditId = certification.AuditId,
            Type = certification.Type,
            Status = certification.Status,
            Scope = certification.Scope,
            StandardReference = certification.StandardReference,
            CertificationBody = certification.CertificationBody,
            AccreditationBody = certification.AccreditationBody,
            IssueDate = certification.IssueDate,
            ExpiryDate = certification.ExpiryDate,
            Notes = certification.Notes
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateCertificationCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _apiClient.UpdateCertificationAsync(id, command, ct);
            if (result != null)
            {
                TempData["Success"] = "Certification updated successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to update certification.");
        }
        return View(command);
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var certification = await _apiClient.GetCertificationAsync(id, ct);
        if (certification == null)
        {
            return NotFound();
        }
        return View(certification);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken ct)
    {
        await _apiClient.DeleteCertificationAsync(id, ct);
        TempData["Success"] = "Certification deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}