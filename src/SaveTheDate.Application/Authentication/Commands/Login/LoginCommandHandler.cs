using MediatR;
using SaveTheDate.Application.Authentication.DTOs;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Common;

namespace SaveTheDate.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            throw new ValidationException("Invalid email or password.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new ValidationException("Invalid email or password.");

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Role);
        return new AuthResponseDto(user.Id, user.Email, user.Role, token, DateTime.UtcNow.AddHours(1));
    }
}
