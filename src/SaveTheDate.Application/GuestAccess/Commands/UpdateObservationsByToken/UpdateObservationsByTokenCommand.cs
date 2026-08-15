using MediatR;
using SaveTheDate.Application.GuestAccess.DTOs;

namespace SaveTheDate.Application.GuestAccess.Commands.UpdateObservationsByToken;

public record UpdateObservationsByTokenCommand(string Token, string Observations) : IRequest<GuestAccessDto>;
