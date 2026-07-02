namespace SaveTheDate.Application.Authentication.DTOs;

public record AuthResponseDto(string UserId, string Email, string Role, string Token, DateTime ExpiresAt);
