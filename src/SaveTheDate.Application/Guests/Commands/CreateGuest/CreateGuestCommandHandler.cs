using Mapster;
using MediatR;
using SaveTheDate.Application.Guests.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Guests;

namespace SaveTheDate.Application.Guests.Commands.CreateGuest;

public class CreateGuestCommandHandler : IRequestHandler<CreateGuestCommand, GuestDto>
{
    private readonly IGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateGuestCommandHandler(IGuestRepository guestRepository, IUnitOfWork unitOfWork)
    {
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<GuestDto> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
    {
        var guest = new Guest(request.Name, request.Surname, request.Email, request.Phone, request.RoleId);
        await _guestRepository.AddAsync(guest, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return guest.Adapt<GuestDto>();
    }
}
