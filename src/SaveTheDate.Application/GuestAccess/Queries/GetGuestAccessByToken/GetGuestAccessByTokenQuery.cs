using MediatR;
using SaveTheDate.Application.GuestAccess.DTOs;

namespace SaveTheDate.Application.GuestAccess.Queries.GetGuestAccessByToken;

public record GetGuestAccessByTokenQuery(string Token) : IRequest<GuestAccessDto>;
