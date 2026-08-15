using MediatR;
using SaveTheDate.Application.GuestAccess.DTOs;

namespace SaveTheDate.Application.GuestAccess.Commands.UpdateProfileByToken;

public record UpdateProfileByTokenCommand(
    string Token,
    string Name,
    string Surname,
    string? Email,
    string Phone) : IRequest<GuestAccessDto>;
