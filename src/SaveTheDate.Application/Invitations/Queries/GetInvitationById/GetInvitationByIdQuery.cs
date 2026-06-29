using Mapster;
using MediatR;
using SaveTheDate.Application.Invitations.DTOs;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Invitations.Queries.GetInvitationById;

public record GetInvitationByIdQuery(int Id) : IRequest<InvitationDto>;

public class GetInvitationByIdQueryHandler : IRequestHandler<GetInvitationByIdQuery, InvitationDto>
{
    private readonly IInvitationRepository _invitationRepository;

    public GetInvitationByIdQueryHandler(IInvitationRepository invitationRepository)
    {
        _invitationRepository = invitationRepository;
    }

    public async Task<InvitationDto> Handle(GetInvitationByIdQuery request, CancellationToken cancellationToken)
    {
        var invitation = await _invitationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (invitation is null)
            throw new NotFoundException("Invitation", request.Id);

        return invitation.Adapt<InvitationDto>();
    }
}
