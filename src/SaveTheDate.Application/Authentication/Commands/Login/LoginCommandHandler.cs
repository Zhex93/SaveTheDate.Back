using MediatR;
using SaveTheDate.Application.Authentication.DTOs;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Authentication.Commands.Login;

public class LoginCommandHandler(
    IAuthService authService,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, error) = await authService.CheckPasswordAsync(request.Email, request.Password);
        if (!succeeded)
            throw new ValidationException(error!);

        var (userId, email, roles) = await authService.FindByEmailAsync(request.Email);
        if (string.IsNullOrEmpty(userId))
            throw new ValidationException("Invalid email or password.");

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
