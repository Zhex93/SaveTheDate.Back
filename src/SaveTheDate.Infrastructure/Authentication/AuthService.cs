using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SaveTheDate.Application.Abstractions;
using SaveTheDate.Infrastructure.Persistence;

namespace SaveTheDate.Infrastructure.Authentication;

public class AuthService : IAuthService
{
    private const string RefreshTokenProvider = "SaveTheDate";
    private const string RefreshTokenName = "RefreshToken";
    private const string RefreshTokenExpiryName = "RefreshTokenExpiry";

    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _context;

    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
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

    public async Task SaveRefreshTokenAsync(string userId, string refreshToken, DateTime expiresAt)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new InvalidOperationException($"User {userId} not found.");

        await _userManager.SetAuthenticationTokenAsync(user, RefreshTokenProvider, RefreshTokenName, refreshToken);
        await _userManager.SetAuthenticationTokenAsync(
            user,
            RefreshTokenProvider,
            RefreshTokenExpiryName,
            expiresAt.ToString("O", CultureInfo.InvariantCulture));
    }

    public async Task<(bool Succeeded, string? UserId, string? Email, IList<string> Roles)> ValidateRefreshTokenAsync(string refreshToken)
    {
        var tokenEntry = await _context.UserTokens
            .FirstOrDefaultAsync(t =>
                t.LoginProvider == RefreshTokenProvider &&
                t.Name == RefreshTokenName &&
                t.Value == refreshToken);

        if (tokenEntry is null)
            return (false, null, null, new List<string>());

        var expiryEntry = await _context.UserTokens
            .FirstOrDefaultAsync(t =>
                t.UserId == tokenEntry.UserId &&
                t.LoginProvider == RefreshTokenProvider &&
                t.Name == RefreshTokenExpiryName);

        if (expiryEntry?.Value is null ||
            !DateTime.TryParse(expiryEntry.Value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var expiresAt) ||
            expiresAt <= DateTime.UtcNow)
        {
            return (false, null, null, new List<string>());
        }

        var user = await _userManager.FindByIdAsync(tokenEntry.UserId);
        if (user is null)
            return (false, null, null, new List<string>());

        var roles = await _userManager.GetRolesAsync(user);
        return (true, user.Id, user.Email, roles);
    }
}
