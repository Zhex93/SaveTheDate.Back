using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaveTheDate.Api.Contracts.Auth;
using SaveTheDate.Application.Authentication.Commands.Login;
using SaveTheDate.Application.Authentication.Commands.Register;
using SaveTheDate.Application.Authentication.DTOs;

namespace SaveTheDate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IWebHostEnvironment environment) : ControllerBase
{
    private const string AuthTokenCookieName = "authToken";

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (token, response) = await mediator.Send(new LoginCommand(request.Email, request.Password));

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = !environment.IsDevelopment(),
            SameSite = environment.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.None,
            Expires = response.ExpiresAt
        };

        Response.Cookies.Append(AuthTokenCookieName, token, cookieOptions);
        
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
}
