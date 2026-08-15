using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveTheDate.Api.Contracts.GuestAccess;
using SaveTheDate.Application.GuestAccess.Commands.UpdateObservationsByToken;
using SaveTheDate.Application.GuestAccess.Commands.UpdateProfileByToken;
using SaveTheDate.Application.GuestAccess.Commands.UpdateRsvpByToken;
using SaveTheDate.Application.GuestAccess.DTOs;
using SaveTheDate.Application.GuestAccess.Queries.GetGuestAccessByToken;
using SaveTheDate.Domain.Invitations;

namespace SaveTheDate.Api.Controllers;

[ApiController]
[Route("api/guest-access")]
[AllowAnonymous]
public class GuestAccessController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestAccessController(IMediator mediator) => _mediator = mediator;

    [HttpGet("{token}")]
    [ProducesResponseType(typeof(GuestAccessDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string token)
    {
        var result = await _mediator.Send(new GetGuestAccessByTokenQuery(token));
        return Ok(result);
    }

    [HttpPut("{token}/rsvp")]
    [ProducesResponseType(typeof(GuestAccessDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateRsvp(string token, [FromBody] UpdateRsvpRequest request)
    {
        if (!Enum.TryParse<InvitationState>(request.State, true, out var state) ||
            state is not (InvitationState.Accepted or InvitationState.Declined))
            return BadRequest($"Invalid state '{request.State}'. Allowed: Accepted, Declined");

        var result = await _mediator.Send(new UpdateRsvpByTokenCommand(token, state));
        return Ok(result);
    }

    [HttpPut("{token}/observations")]
    [ProducesResponseType(typeof(GuestAccessDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateObservations(string token, [FromBody] UpdateObservationsRequest request)
    {
        var result = await _mediator.Send(new UpdateObservationsByTokenCommand(
            token, request.Observations ?? string.Empty));
        return Ok(result);
    }

    [HttpPut("{token}/profile")]
    [ProducesResponseType(typeof(GuestAccessDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProfile(string token, [FromBody] UpdateGuestProfileRequest request)
    {
        var result = await _mediator.Send(new UpdateProfileByTokenCommand(
            token, request.Name, request.Surname, request.Email, request.Phone));
        return Ok(result);
    }
}
