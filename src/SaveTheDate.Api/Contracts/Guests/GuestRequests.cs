namespace SaveTheDate.Api.Contracts.Guests;

public record CreateGuestRequest(string Name, string Surname, string? Email, string Phone, int RoleId);
public record UpdateGuestRequest(string Name, string Surname, string? Email, string Phone, int RoleId);
