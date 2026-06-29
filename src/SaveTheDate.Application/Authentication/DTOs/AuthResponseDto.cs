namespace SaveTheDate.Application.Authentication.DTOs;

public record AuthResponseDto(int UserId, string Email, string Role, string Token, DateTime ExpiresAt);
