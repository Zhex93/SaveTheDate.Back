namespace SaveTheDate.Api.Contracts.GuestAccess;

public record UpdateRsvpRequest(string State);
public record UpdateObservationsRequest(string? Observations);
public record UpdateGuestProfileRequest(string Name, string Surname, string? Email, string Phone);
