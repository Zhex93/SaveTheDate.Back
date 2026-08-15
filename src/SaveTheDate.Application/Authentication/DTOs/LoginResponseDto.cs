namespace SaveTheDate.Application.Authentication.DTOs;

public record LoginResponseDto(string UserId, string Email, string Role, DateTime ExpiresAt);
