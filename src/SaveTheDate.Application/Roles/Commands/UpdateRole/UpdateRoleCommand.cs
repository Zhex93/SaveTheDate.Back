using MediatR;
using SaveTheDate.Application.Roles.DTOs;

namespace SaveTheDate.Application.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(int Id, string Description) : IRequest<RoleDto>;
