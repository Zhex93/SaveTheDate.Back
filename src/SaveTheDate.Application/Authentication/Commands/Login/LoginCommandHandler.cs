using MediatR;
using SaveTheDate.Application.Authentication.DTOs;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Authentication.Commands.Login;

public class LoginCommandHandler(
    IAuthService authService,
    IJwtTokenGenerator jwtTokenGenerator)
    : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, error) = await authService.CheckPasswordAsync(request.Email, request.Password);
        if (!succeeded)
            throw new ValidationException(error!);

        var (userId, email, roles) = await authService.FindByEmailAsync(request.Email);
        if (string.IsNullOrEmpty(userId))
            throw new ValidationException("Invalid email or password.");

        var token = jwtTokenGenerator.GenerateToken(userId, email, roles);
        var role = roles.FirstOrDefault() ?? string.Empty;
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var response = new LoginResponseDto(userId, email, role, expiresAt);
        return new LoginResult(token, response);
    }
}
