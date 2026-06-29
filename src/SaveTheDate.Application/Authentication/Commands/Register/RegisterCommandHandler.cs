using MediatR;
using SaveTheDate.Application.Authentication.DTOs;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Domain.Abstractions;
using SaveTheDate.Domain.Users;

namespace SaveTheDate.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (existing is not null)
            throw new Domain.Common.ValidationException("A user with this email already exists.");

        var hash = _passwordHasher.Hash(request.Password);
        var user = new User(request.Email, hash, request.Role);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, user.Role);
        return new AuthResponseDto(user.Id, user.Email, user.Role, token, DateTime.UtcNow.AddHours(1));
    }
}
