using Mapster;
using MediatR;
using SaveTheDate.Application.Guests.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Guests.Commands.UpdateGuest;

public class UpdateGuestCommandHandler : IRequestHandler<UpdateGuestCommand, GuestDto>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGuestCommandHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork)
    {
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GuestDto> Handle(UpdateGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = await _guestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (guest is null)
            throw new NotFoundException("Guest", request.Id);

        guest.Update(request.Name, request.Surname, request.Email, request.Phone, request.RoleId);
        _guestRepository.Update(guest);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return guest.Adapt<GuestDto>();
    }
}
