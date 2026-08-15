using MediatR;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Application.Invitations.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;
using SaveTheDate.Domain.Invitations;

namespace SaveTheDate.Application.Invitations.Commands.GenerateAccessLink;

public class GenerateAccessLinkCommandHandler : IRequestHandler<GenerateAccessLinkCommand, InvitationAccessLinkDto>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IGuestAccessTokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateAccessLinkCommandHandler(
        IInvitationRepository invitationRepository,
        IGuestAccessTokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<InvitationAccessLinkDto> Handle(GenerateAccessLinkCommand request, CancellationToken cancellationToken)
    {
        var invitation = await _invitationRepository.GetByIdAsync(request.InvitationId, cancellationToken);
        if (invitation is null)
            throw new NotFoundException("Invitation", request.InvitationId);

        var token = _tokenService.GenerateToken();
        invitation.SetAccessToken(_tokenService.HashToken(token));

        if (invitation.State == InvitationState.Pending)
            invitation.ChangeState(InvitationState.Sent);

        _invitationRepository.Update(invitation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new InvitationAccessLinkDto(token);
    }
}
