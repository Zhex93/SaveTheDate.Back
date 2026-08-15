using MediatR;
using SaveTheDate.Application.GuestAccess.DTOs;
using SaveTheDate.Domain.Invitations;

namespace SaveTheDate.Application.GuestAccess.Commands.UpdateRsvpByToken;

public record UpdateRsvpByTokenCommand(string Token, InvitationState State) : IRequest<GuestAccessDto>;
