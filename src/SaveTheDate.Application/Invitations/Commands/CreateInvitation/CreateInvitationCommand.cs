using MediatR;
using SaveTheDate.Application.Invitations.DTOs;

namespace SaveTheDate.Application.Invitations.Commands.CreateInvitation;

public record CreateInvitationCommand(int GuestId, string Observations) : IRequest<InvitationDto>;
