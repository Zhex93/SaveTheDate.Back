using MediatR;
using SaveTheDate.Application.Authentication.DTOs;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IAuthService _authService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(
        IAuthService authService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _authService = authService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (succeeded, error) = await _authService.CreateUserAsync(request.Email, request.Password, request.Role);
        if (!succeeded)
            throw new ValidationException(error!);

        var (userId, email, roles) = await _authService.FindByEmailAsync(request.Email);
        var accessToken = _jwtTokenGenerator.GenerateAccessToken(userId, email, roles);
        var role = roles.FirstOrDefault() ?? string.Empty;
        return new AuthResponseDto(userId, email, role, accessToken.Token, accessToken.ExpiresAt);
    }
}
