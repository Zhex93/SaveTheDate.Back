using MediatR;

namespace SaveTheDate.Application.Invitations.Commands.DeleteInvitation;

public record DeleteInvitationCommand(int Id) : IRequest;
