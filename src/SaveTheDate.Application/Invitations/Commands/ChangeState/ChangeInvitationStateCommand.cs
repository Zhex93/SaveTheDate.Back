using MediatR;
using SaveTheDate.Application.Invitations.DTOs;
using SaveTheDate.Domain.Invitations;

namespace SaveTheDate.Application.Invitations.Commands.ChangeState;

public record ChangeInvitationStateCommand(int Id, InvitationState State) : IRequest<InvitationDto>;
