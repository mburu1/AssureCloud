using System;
using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Commands.Users;
using AssureCloud.Application.DTOs.Users;
using AssureCloud.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssureCloud.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers(
        [FromQuery] GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("{id:guid}/profile")]
    public async Task<ActionResult<UserProfileDto>> GetUserProfile(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserProfileQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetUser), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserDto>> UpdateUser(
        Guid id,
        [FromBody] UpdateUserCommand command,
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
    public async Task<ActionResult> DeleteUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult> ActivateUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new ActivateUserCommand(id);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult> DeactivateUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateUserCommand(id);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("{id:guid}/roles")]
    public async Task<ActionResult> AssignRole(
        Guid id,
        [FromBody] AssignUserRoleCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.UserId)
        {
            return BadRequest("User ID mismatch");
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}/roles/{role}")]
    public async Task<ActionResult> RemoveRole(
        Guid id,
        string role,
        CancellationToken cancellationToken)
    {
        var command = new RemoveUserRoleCommand(id, role);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id:guid}/profile")]
    public async Task<ActionResult<UserProfileDto>> UpdateProfile(
        Guid id,
        [FromBody] UpdateUserProfileCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.UserId)
        {
            return BadRequest("User ID mismatch");
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}/change-password")]
    public async Task<ActionResult> ChangePassword(
        Guid id,
        [FromBody] ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.UserId)
        {
            return BadRequest("User ID mismatch");
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}