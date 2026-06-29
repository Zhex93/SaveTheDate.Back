using Mapster;
using MediatR;
using SaveTheDate.Application.Roles.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(int Id) : IRequest<RoleDto>;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (role is null)
            throw new NotFoundException("Role", request.Id);

        return role.Adapt<RoleDto>();
    }
}
