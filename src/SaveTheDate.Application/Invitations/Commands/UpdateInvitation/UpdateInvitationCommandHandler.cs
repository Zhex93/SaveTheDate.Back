using Mapster;
using MediatR;
using SaveTheDate.Application.Invitations.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Invitations.Commands.UpdateInvitation;

public class UpdateInvitationCommandHandler : IRequestHandler<UpdateInvitationCommand, InvitationDto>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateInvitationCommandHandler(IInvitationRepository invitationRepository, IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InvitationDto> Handle(UpdateInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = await _invitationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invitation is null)
            throw new NotFoundException("Invitation", request.Id);

        invitation.SetGuestId(request.GuestId);
        invitation.SetObservations(request.Observations);
        _invitationRepository.Update(invitation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return invitation.Adapt<InvitationDto>();
    }
}
