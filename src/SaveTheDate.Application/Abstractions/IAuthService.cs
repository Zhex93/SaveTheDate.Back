namespace SaveTheDate.Application.Abstractions;

public interface IAuthService
{
    Task<(string UserId, string Email, IList<string> Roles)> FindByEmailAsync(string email);
    Task<(bool Succeeded, string? Error)> CheckPasswordAsync(string email, string password);
    Task<(bool Succeeded, string? Error)> CreateUserAsync(string email, string password, string role);
    Task<bool> RoleExistsAsync(string role);
    Task<(bool Succeeded, string? Error)> CreateRoleAsync(string role);
}
