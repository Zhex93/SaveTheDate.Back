using MediatR;
using SaveTheDate.Application.Guests.DTOs;

namespace SaveTheDate.Application.Guests.Commands.CreateGuest;

public record CreateGuestCommand(string Name, string Surname, string? Email, string Phone, int RoleId) : IRequest<GuestDto>;
