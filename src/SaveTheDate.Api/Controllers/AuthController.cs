using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SaveTheDate.Api.Contracts.Auth;
using SaveTheDate.Application.Authentication.Commands.Login;
using SaveTheDate.Application.Authentication.Commands.RefreshToken;
using SaveTheDate.Application.Authentication.Commands.Register;
using SaveTheDate.Application.Authentication.DTOs;

namespace SaveTheDate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IWebHostEnvironment environment) : ControllerBase
{
    private const string AuthTokenCookieName = "authToken";
    private const string RefreshTokenCookieName = "refreshToken";

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await mediator.Send(new LoginCommand(request.Email, request.Password));

        AppendCookie(AuthTokenCookieName, response.Token, response.ExpiresAt);
        AppendCookie(RefreshTokenCookieName, response.RefreshToken, response.RefreshTokenExpiresAt);

        return Ok(response);
    }

    [HttpPost("register")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await mediator.Send(new RegisterCommand(request.Email, request.Password, request.Role));
        return Ok(result);
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Refresh([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] RefreshTokenRequest? request)
    {
        var refreshToken = request?.RefreshToken;
        if (string.IsNullOrEmpty(refreshToken))
            refreshToken = Request.Cookies[RefreshTokenCookieName];

        if (string.IsNullOrEmpty(refreshToken))
            return BadRequest(new { message = "Refresh token is required." });

        var response = await mediator.Send(new RefreshTokenCommand(refreshToken));

        AppendCookie(AuthTokenCookieName, response.Token, response.ExpiresAt);
        AppendCookie(RefreshTokenCookieName, response.RefreshToken, response.RefreshTokenExpiresAt);

        return Ok(response);
    }

    private void AppendCookie(string name, string value, DateTime expiresAt)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !environment.IsDevelopment(),
            SameSite = environment.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.None,
            Expires = expiresAt
        };

        Response.Cookies.Append(name, value, cookieOptions);
    }
}
