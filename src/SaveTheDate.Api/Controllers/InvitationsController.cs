using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveTheDate.Api.Contracts.Invitations;
using SaveTheDate.Application.Invitations.Commands.ChangeState;
using SaveTheDate.Application.Invitations.Commands.CreateInvitation;
using SaveTheDate.Application.Invitations.Commands.DeleteInvitation;
using SaveTheDate.Application.Invitations.Commands.UpdateInvitation;
using SaveTheDate.Application.Invitations.Queries.GetInvitationById;
using SaveTheDate.Application.Invitations.Queries.GetInvitations;
using SaveTheDate.Domain.Invitations;

namespace SaveTheDate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvitationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public InvitationsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetInvitationsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetInvitationByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInvitationRequest request)
    {
        var result = await _mediator.Send(new CreateInvitationCommand(
            request.GuestId, request.Observations ?? string.Empty));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInvitationRequest request)
    {
        var result = await _mediator.Send(new UpdateInvitationCommand(
            id, request.GuestId, request.Observations ?? string.Empty));
        return Ok(result);
    }

    [HttpPatch("{id:int}/state")]
    public async Task<IActionResult> ChangeState(int id, [FromBody] ChangeInvitationStateRequest request)
    {
        if (!Enum.TryParse<InvitationState>(request.State, true, out var state))
            return BadRequest($"Invalid state '{request.State}'. Allowed: {string.Join(", ", Enum.GetNames<InvitationState>())}");

        var result = await _mediator.Send(new ChangeInvitationStateCommand(id, state));
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteInvitationCommand(id));
        return NoContent();
    }
}
