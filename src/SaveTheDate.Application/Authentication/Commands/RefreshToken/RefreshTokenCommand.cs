using MediatR;
using SaveTheDate.Application.Authentication.DTOs;

namespace SaveTheDate.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<LoginResponseDto>;
