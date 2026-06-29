using Mapster;
using MediatR;
using SaveTheDate.Application.Invitations.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Invitations.Commands.ChangeState;

public class ChangeInvitationStateCommandHandler : IRequestHandler<ChangeInvitationStateCommand, InvitationDto>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeInvitationStateCommandHandler(IInvitationRepository invitationRepository, IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InvitationDto> Handle(ChangeInvitationStateCommand request, CancellationToken cancellationToken)
    {
        var invitation = await _invitationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invitation is null)
            throw new NotFoundException("Invitation", request.Id);

        invitation.ChangeState(request.State);
        _invitationRepository.Update(invitation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return invitation.Adapt<InvitationDto>();
    }
}
