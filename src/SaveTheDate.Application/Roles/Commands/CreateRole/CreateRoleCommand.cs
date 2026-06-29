using MediatR;
using SaveTheDate.Application.Roles.DTOs;

namespace SaveTheDate.Application.Roles.Commands.CreateRole;

public record CreateRoleCommand(string Description) : IRequest<RoleDto>;
