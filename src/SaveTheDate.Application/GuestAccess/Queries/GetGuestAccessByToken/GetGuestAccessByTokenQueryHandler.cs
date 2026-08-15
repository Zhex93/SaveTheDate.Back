using MediatR;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Application.GuestAccess.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;
using SaveTheDate.Domain.Invitations;

namespace SaveTheDate.Application.GuestAccess.Queries.GetGuestAccessByToken;

public class GetGuestAccessByTokenQueryHandler : IRequestHandler<GetGuestAccessByTokenQuery, GuestAccessDto>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IGuestAccessTokenService _tokenService;

    public GetGuestAccessByTokenQueryHandler(
        IInvitationRepository invitationRepository,
        IGuestAccessTokenService tokenService)
    {
        _invitationRepository = invitationRepository;
        _tokenService = tokenService;
    }

    public async Task<GuestAccessDto> Handle(GetGuestAccessByTokenQuery request, CancellationToken cancellationToken)
    {
        var invitation = await GetInvitationByTokenAsync(request.Token, cancellationToken);
        return ToDto(invitation);
    }

    private async Task<Invitation> GetInvitationByTokenAsync(string token, CancellationToken cancellationToken)
    {
        var invitation = await _invitationRepository.GetByAccessTokenHashAsync(
            _tokenService.HashToken(token), cancellationToken);

        if (invitation is null)
            throw new NotFoundException("Invalid or revoked guest access link.");

        return invitation;
    }

    internal static GuestAccessDto ToDto(Invitation invitation) => new(
        invitation.Id,
        invitation.State.ToString(),
        invitation.Observations,
        invitation.GuestId,
        invitation.Guest.Name,
        invitation.Guest.Surname,
        invitation.Guest.Email,
        invitation.Guest.Phone);
}
