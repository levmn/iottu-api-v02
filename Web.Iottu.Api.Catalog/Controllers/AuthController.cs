using Core.Iottu.Domain.Interfaces;
using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.Iottu.Contracts.DTOs.Auth;

namespace Web.Iottu.Api.Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Tags("Autenticação")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ITokenService _tokenService;

        public AuthController(IUsuarioService usuarioService, ITokenService tokenService)
        {
            _usuarioService = usuarioService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Realiza autenticação do usuário e retorna um token JWT
        /// </summary>
        /// <param name="request">Credenciais do usuário</param>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /api/auth/login
        ///     {
        ///         "username": "admin",
        ///         "password": "admin123"
        ///     }
        /// </remarks>
        /// <response code="200">Retorna o token JWT</response>
        /// <response code="400">Credenciais inválidas ou ausentes</response>
        /// <response code="401">Usuário não autorizado</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Usuário e senha são obrigatórios." });

            var user = await _usuarioService.AuthenticateAsync(request.Username, request.Password);
            if (user == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos." });

            var token = _tokenService.GenerateToken(user.Username, new[]
            {
                new KeyValuePair<string, string>("role", user.Role ?? "user")
            });

            var response = new AuthResponse
            {
                AccessToken = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            return Ok(response);
        }

        /// <summary>
        /// Retorna informações sobre o usuário autenticado.
        /// </summary>
        [HttpGet("whoami")]
        [Authorize]
        public IActionResult WhoAmI()
        {
            var username = User.Identity?.Name;
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(new { Username = username, Claims = claims });
        }
    }
}
