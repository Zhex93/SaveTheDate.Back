namespace SaveTheDate.Application.GuestAccess.DTOs;

public record GuestAccessDto(
    int InvitationId,
    string State,
    string Observations,
    int GuestId,
    string Name,
    string Surname,
    string? Email,
    string Phone);
