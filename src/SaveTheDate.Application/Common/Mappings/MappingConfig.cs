using Mapster;
using SaveTheDate.Application.Authentication.DTOs;
using SaveTheDate.Application.Guests.DTOs;
using SaveTheDate.Application.Invitations.DTOs;
using SaveTheDate.Application.Roles.DTOs;
using SaveTheDate.Domain.Guests;
using SaveTheDate.Domain.Invitations;
using SaveTheDate.Domain.Roles;

namespace SaveTheDate.Application.Common.Mappings;

public class MappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Role, RoleDto>();
        config.NewConfig<Guest, GuestDto>();
        config.NewConfig<Invitation, InvitationDto>();
    }
}
