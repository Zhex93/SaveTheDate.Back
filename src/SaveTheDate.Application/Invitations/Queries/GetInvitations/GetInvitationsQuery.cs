using Mapster;
using MediatR;
using SaveTheDate.Application.Invitations.DTOs;
using SaveTheDate.Domain.Abstractions;

namespace SaveTheDate.Application.Invitations.Queries.GetInvitations;

public record GetInvitationsQuery : IRequest<IEnumerable<InvitationDto>>;

public class GetInvitationsQueryHandler : IRequestHandler<GetInvitationsQuery, IEnumerable<InvitationDto>>
{
    private readonly IInvitationRepository _invitationRepository;

    public GetInvitationsQueryHandler(IInvitationRepository invitationRepository)
    {
        _invitationRepository = invitationRepository;
    }

    public async Task<IEnumerable<InvitationDto>> Handle(GetInvitationsQuery request, CancellationToken cancellationToken)
    {
        var invitations = await _invitationRepository.GetAllAsync(cancellationToken);
        return invitations.Adapt<IEnumerable<InvitationDto>>();
    }
}
