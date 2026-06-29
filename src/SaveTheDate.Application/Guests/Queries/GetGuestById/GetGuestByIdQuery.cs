using Mapster;
using MediatR;
using SaveTheDate.Application.Guests.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Guests.Queries.GetGuestById;

public record GetGuestByIdQuery(int Id) : IRequest<GuestDto>;

public class GetGuestByIdQueryHandler : IRequestHandler<GetGuestByIdQuery, GuestDto>
{
    private readonly IGuestRepository _guestRepository;

    public GetGuestByIdQueryHandler(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public async Task<GuestDto> Handle(GetGuestByIdQuery request, CancellationToken cancellationToken)
    {
        var guest = await _guestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (guest is null)
            throw new NotFoundException("Guest", request.Id);

        return guest.Adapt<GuestDto>();
    }
}
