namespace SaveTheDate.Application.Abstractions;

public record GeneratedToken(string Token, DateTime ExpiresAt);

public interface IJwtTokenGenerator
{
    GeneratedToken GenerateAccessToken(string userId, string email, IList<string> roles);
    GeneratedToken GenerateRefreshToken();
}
