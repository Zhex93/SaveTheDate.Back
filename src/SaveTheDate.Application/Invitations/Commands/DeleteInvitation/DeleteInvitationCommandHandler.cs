using MediatR;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Invitations.Commands.DeleteInvitation;

public class DeleteInvitationCommandHandler : IRequestHandler<DeleteInvitationCommand>
{
    private readonly IInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteInvitationCommandHandler(IInvitationRepository invitationRepository, IUnitOfWork unitOfWork)
    {
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteInvitationCommand request, CancellationToken cancellationToken)
    {
        var invitation = await _invitationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invitation is null)
            throw new NotFoundException("Invitation", request.Id);

        _invitationRepository.Delete(invitation);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
