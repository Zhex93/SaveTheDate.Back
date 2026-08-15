using MediatR;
using SaveTheDate.Application.Guests.DTOs;

namespace SaveTheDate.Application.Guests.Commands.UpdateGuest;

public record UpdateGuestCommand(int Id, string Name, string Surname, string? Email, string Phone, int RoleId) : IRequest<GuestDto>;
