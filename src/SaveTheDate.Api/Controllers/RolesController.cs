using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveTheDate.Api.Contracts.Roles;
using SaveTheDate.Application.Roles.Commands.CreateRole;
using SaveTheDate.Application.Roles.Commands.DeleteRole;
using SaveTheDate.Application.Roles.Commands.UpdateRole;
using SaveTheDate.Application.Roles.Queries.GetRoleById;
using SaveTheDate.Application.Roles.Queries.GetRoles;

namespace SaveTheDate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetRolesQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetRoleByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
    {
        var result = await _mediator.Send(new CreateRoleCommand(request.Description));
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleRequest request)
    {
        var result = await _mediator.Send(new UpdateRoleCommand(id, request.Description));
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteRoleCommand(id));
        return NoContent();
    }
}
