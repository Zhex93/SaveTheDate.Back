namespace SaveTheDate.Application.Authentication.DTOs;

public record LoginResponseDto(
    string UserId,
    string Email,
    string Role,
    string Token,
    DateTime ExpiresAt,
    string RefreshToken,
    DateTime RefreshTokenExpiresAt);
