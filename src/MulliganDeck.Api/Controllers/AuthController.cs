using Microsoft.AspNetCore.Mvc;
using MulliganDeck.Api.Dtos;
using MulliganDeck.Infrastructure.Auth;

namespace MulliganDeck.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = await _authService.RegisterAsync(dto.Email, dto.Password);

        if (user == null)
            return Conflict(new { message = "Ya existe un usuario con ese email." });

        return Ok(new { user.Id, user.Email });
    }
}