using System.Security.Cryptography;
using System.Text;
using SaveTheDate.Application.Abstractions;

namespace SaveTheDate.Infrastructure.Authentication;

public class GuestAccessTokenService : IGuestAccessTokenService
{
    public string GenerateToken()
        => Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

    public string HashToken(string token)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
