using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Users;
using AssureCloud.Application.DTOs.Users;
using AssureCloud.Application.Queries.Users;
using AssureCloud.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Mvc.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly ApiClient _apiClient;

    public UsersController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index(GetUsersQuery query, CancellationToken ct)
    {
        var result = await _apiClient.GetUsersAsync(query, ct);
        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct)
    {
        var user = await _apiClient.GetUserAsync(id, ct);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    public IActionResult Create()
    {
        return View(new CreateUserCommand());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            var result = await _apiClient.CreateUserAsync(command, ct);
            if (result != null)
            {
                TempData["Success"] = "User created successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to create user.");
        }
        return View(command);
    }

    public async Task<IActionResult> Edit(Guid id, CancellationToken ct)
    {
        var user = await _apiClient.GetUserAsync(id, ct);
        if (user == null)
        {
            return NotFound();
        }

        var command = new UpdateUserCommand
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            OrganizationId = user.OrganizationId,
            Status = user.Status,
            Timezone = user.Timezone,
            Language = user.Language
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateUserCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var result = await _apiClient.UpdateUserAsync(id, command, ct);
            if (result != null)
            {
                TempData["Success"] = "User updated successfully.";
                return RedirectToAction(nameof(Details), new { id = result.Id });
            }
            ModelState.AddModelError("", "Failed to update user.");
        }
        return View(command);
    }

    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var user = await _apiClient.GetUserAsync(id, ct);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken ct)
    {
        await _apiClient.DeleteUserAsync(id, ct);
        TempData["Success"] = "User deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}