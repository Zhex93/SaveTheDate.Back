using MediatR;
using SaveTheDate.Application.Authentication.DTOs;

namespace SaveTheDate.Application.Authentication.Commands.Register;

public record RegisterCommand(string Email, string Password, string Role) : IRequest<AuthResponseDto>;
