namespace SaveTheDate.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string email, IList<string> roles);
}
