using MediatR;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Application.GuestAccess.DTOs;
using SaveTheDate.Application.GuestAccess.Queries.GetGuestAccessByToken;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.GuestAccess.Commands.UpdateRsvpByToken;

public class UpdateRsvpByTokenCommandHandler : IRequestHandler<UpdateRsvpByTokenCommand, GuestAccessDto>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IGuestAccessTokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRsvpByTokenCommandHandler(
        IInvitationRepository invitationRepository,
        IGuestAccessTokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<GuestAccessDto> Handle(UpdateRsvpByTokenCommand request, CancellationToken cancellationToken)
    {
        var invitation = await _invitationRepository.GetByAccessTokenHashAsync(
            _tokenService.HashToken(request.Token), cancellationToken);

        if (invitation is null)
            throw new NotFoundException("Invalid or revoked guest access link.");

        invitation.ChangeState(request.State);
        _invitationRepository.Update(invitation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return GetGuestAccessByTokenQueryHandler.ToDto(invitation);
    }
}
