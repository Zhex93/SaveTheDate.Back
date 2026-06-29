using MediatR;

namespace SaveTheDate.Application.Guests.Commands.DeleteGuest;

public record DeleteGuestCommand(int Id) : IRequest;
