using MediatR;
using SaveTheDate.Application.Authentication.DTOs;

namespace SaveTheDate.Application.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
