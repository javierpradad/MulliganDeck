using Microsoft.AspNetCore.Mvc;
using MulliganDeck.Api.Dtos;
using MulliganDeck.Infrastructure.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _authService.LoginAsync(dto.Email, dto.Password);

        if (token == null)
            return Unauthorized(new { message = "Credenciales inválidas." });

        return Ok(new { token });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var id = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        return Ok(new { id, email });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("promote")]
    public async Task<IActionResult> Promote(PromoteDto dto)
    {
        var user = await _authService.PromoteAsync(dto.Email);
        if (user == null)
            return NotFound(new { message = "Usuario no encontrado." });

        return Ok(new { user.Id, user.Email, user.Role });
    }
}