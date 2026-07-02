using Microsoft.AspNetCore.Identity;
using SaveTheDate.Application.Abstractions;

namespace SaveTheDate.Infrastructure.Authentication;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<(string UserId, string Email, IList<string> Roles)> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return (string.Empty, string.Empty, new List<string>());

        var roles = await _userManager.GetRolesAsync(user);
        return (user.Id, user.Email!, roles);
    }

    public async Task<(bool Succeeded, string? Error)> CheckPasswordAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return (false, "Invalid email or password.");

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        if (!result.Succeeded)
            return (false, "Invalid email or password.");

        return (true, null);
    }

    public async Task<(bool Succeeded, string? Error)> CreateUserAsync(string email, string password, string role)
    {
        var existing = await _userManager.FindByEmailAsync(email);
        if (existing is not null)
            return (false, "A user with this email already exists.");

        var user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return (false, $"User registration failed: {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
            return (false, $"Failed to assign role: {errors}");
        }

        return (true, null);
    }

    public async Task<bool> RoleExistsAsync(string role)
        => await _roleManager.RoleExistsAsync(role);

    public async Task<(bool Succeeded, string? Error)> CreateRoleAsync(string role)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole(role));
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return (false, $"Failed to create role: {errors}");
        }
        return (true, null);
    }
}
