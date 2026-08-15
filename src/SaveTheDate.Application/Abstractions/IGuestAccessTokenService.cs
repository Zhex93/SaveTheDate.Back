namespace SaveTheDate.Application.Abstractions;

public interface IGuestAccessTokenService
{
    string GenerateToken();
    string HashToken(string token);
}
