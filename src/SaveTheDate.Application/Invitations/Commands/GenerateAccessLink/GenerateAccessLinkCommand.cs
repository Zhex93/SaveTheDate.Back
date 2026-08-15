using MediatR;
using SaveTheDate.Application.Invitations.DTOs;

namespace SaveTheDate.Application.Invitations.Commands.GenerateAccessLink;

public record GenerateAccessLinkCommand(int InvitationId) : IRequest<InvitationAccessLinkDto>;
