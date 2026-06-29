using SaveTheDate.Domain.Common;

namespace SaveTheDate.Domain.Users;

public class User : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    private User() { }

    public User(string email, string passwordHash, string role)
    {
        SetEmail(email);
        SetPasswordHash(passwordHash);
        SetRole(role);
        CreatedAt = DateTime.UtcNow;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ValidationException("User email cannot be empty.");
        Email = email;
    }

    public void SetPasswordHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ValidationException("Password hash cannot be empty.");
        PasswordHash = hash;
    }

    public void SetRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ValidationException("User role cannot be empty.");
        Role = role;
    }
}
