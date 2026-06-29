using Mapster;
using MediatR;
using SaveTheDate.Application.Guests.DTOs;
using SaveTheDate.Domain.Abstractions;

namespace SaveTheDate.Application.Guests.Queries.GetGuests;

public record GetGuestsQuery : IRequest<IEnumerable<GuestDto>>;

public class GetGuestsQueryHandler : IRequestHandler<GetGuestsQuery, IEnumerable<GuestDto>>
{
    private readonly IGuestRepository _guestRepository;

    public GetGuestsQueryHandler(IGuestRepository guestRepository)
    {
        _guestRepository = guestRepository;
    }

    public async Task<IEnumerable<GuestDto>> Handle(GetGuestsQuery request, CancellationToken cancellationToken)
    {
        var guests = await _guestRepository.GetAllAsync(cancellationToken);
        return guests.Adapt<IEnumerable<GuestDto>>();
    }
}
