using MediatR;
using SaveTheDate.Application.Authentication.DTOs;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IAuthService _authService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IAuthService authService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _authService = authService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, error) = await _authService.CheckPasswordAsync(request.Email, request.Password);
        if (!succeeded)
            throw new ValidationException(error!);

        var (userId, email, roles) = await _authService.FindByEmailAsync(request.Email);
        if (string.IsNullOrEmpty(userId))
            throw new ValidationException("Invalid email or password.");

        var token = _jwtTokenGenerator.GenerateToken(userId, email, roles);
        var role = roles.FirstOrDefault() ?? string.Empty;
        return new AuthResponseDto(userId, email, role, token, DateTime.UtcNow.AddHours(1));
    }
}
