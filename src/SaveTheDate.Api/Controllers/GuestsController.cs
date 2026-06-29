using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveTheDate.Api.Contracts.Guests;
using SaveTheDate.Application.Guests.Commands.CreateGuest;
using SaveTheDate.Application.Guests.Commands.DeleteGuest;
using SaveTheDate.Application.Guests.Commands.UpdateGuest;
using SaveTheDate.Application.Guests.Queries.GetGuestById;
using SaveTheDate.Application.Guests.Queries.GetGuests;

namespace SaveTheDate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GuestsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GuestsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetGuestsQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetGuestByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGuestRequest request)
    {
        var result = await _mediator.Send(new CreateGuestCommand(
            request.Name, request.Surname, request.Email, request.Phone, request.RoleId));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateGuestRequest request)
    {
        var result = await _mediator.Send(new UpdateGuestCommand(
            id, request.Name, request.Surname, request.Email, request.Phone, request.RoleId));
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteGuestCommand(id));
        return NoContent();
    }
}
