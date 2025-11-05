using Core.Iottu.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.Iottu.Contracts.DTOs;

namespace Web.Iottu.Api.Catalog.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    // ideal: injetar um serviço que verifica credenciais (ex.: IUserService)
    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult Token([FromBody] LoginRequest request)
    {
        if (request.Username == "admin" && request.Password == "password")
        {
            var token = _tokenService.GenerateToken(request.Username, new[] { new KeyValuePair<string, string>("role", "admin") });
            return Ok(new AuthResponse { AccessToken = token, ExpiresAt = DateTime.UtcNow.AddMinutes(60) });
        }

        return Unauthorized();
    }

    [HttpGet("whoami")]
    [Authorize]
    public IActionResult WhoAmI()
    {
        var sub = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
        return Ok(new { Subject = sub, Claims = User.Claims.Select(c => new { c.Type, c.Value }) });
    }
}
