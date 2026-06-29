namespace SaveTheDate.Api.Contracts.Auth;

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string Email, string Password, string Role);
