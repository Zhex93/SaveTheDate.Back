using Mapster;
using MediatR;
using SaveTheDate.Application.Roles.DTOs;
using SaveTheDate.Domain.Abstractions;

namespace SaveTheDate.Application.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<IEnumerable<RoleDto>>;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync(cancellationToken);
        return roles.Adapt<IEnumerable<RoleDto>>();
    }
}
