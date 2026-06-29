namespace SaveTheDate.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userId, string email, string role);
}
