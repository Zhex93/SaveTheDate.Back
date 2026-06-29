using MediatR;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Guests.Commands.DeleteGuest;

public class DeleteGuestCommandHandler : IRequestHandler<DeleteGuestCommand>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGuestCommandHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork)
    {
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = await _guestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (guest is null)
            throw new NotFoundException("Guest", request.Id);

        _guestRepository.Delete(guest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
