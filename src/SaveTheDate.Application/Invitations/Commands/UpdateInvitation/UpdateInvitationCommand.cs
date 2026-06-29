using MediatR;
using SaveTheDate.Application.Invitations.DTOs;

namespace SaveTheDate.Application.Invitations.Commands.UpdateInvitation;

public record UpdateInvitationCommand(int Id, int GuestId, string Observations) : IRequest<InvitationDto>;
