using MediatR;
using SaveTheDate.Application.Authentication.DTOs;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IAuthService authService,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, userId, email, roles) = await authService.ValidateRefreshTokenAsync(request.RefreshToken);
        if (!succeeded || string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email))
            throw new ValidationException("Invalid or expired refresh token.");

        var accessToken = jwtTokenGenerator.GenerateAccessToken(userId, email, roles);
        var refreshToken = jwtTokenGenerator.GenerateRefreshToken();

        await authService.SaveRefreshTokenAsync(userId, refreshToken.Token, refreshToken.ExpiresAt);

        var role = roles.FirstOrDefault() ?? string.Empty;
        return new LoginResponseDto(
            userId,
            email,
            role,
            accessToken.Token,
            accessToken.ExpiresAt,
            refreshToken.Token,
            refreshToken.ExpiresAt);
    }
}
