using MediatR;

namespace SaveTheDate.Application.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(int Id) : IRequest;
