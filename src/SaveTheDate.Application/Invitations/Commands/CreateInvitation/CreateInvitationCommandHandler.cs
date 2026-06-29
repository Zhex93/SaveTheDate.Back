using Mapster;
using MediatR;
using SaveTheDate.Application.Invitations.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Invitations;

namespace SaveTheDate.Application.Invitations.Commands.CreateInvitation;

public class CreateInvitationCommandHandler : IRequestHandler<CreateInvitationCommand, InvitationDto>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateInvitationCommandHandler(IInvitationRepository invitationRepository, IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<InvitationDto> Handle(CreateInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = new Invitation(request.GuestId, request.Observations);
        await _invitationRepository.AddAsync(invitation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return invitation.Adapt<InvitationDto>();
    }
}
