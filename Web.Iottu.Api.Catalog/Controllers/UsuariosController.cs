using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;
using Web.Iottu.Api.Catalog.Helpers;

namespace Web.Iottu.Api.Catalog.Controllers
{
    /// <summary>
    /// Endpoints para gerenciamento de Usuários.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Cria um novo usuário (registro).
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> Create([FromBody] CreateUsuarioDto dto)
        {
            try
            {
                var created = await _usuarioService.CreateAsync(dto);

                var resourceWithLinks = HateoasHelper.AddLinks(created, new Dictionary<string, string>
                {
                    ["self"] = $"/api/user/{created.Id}",
                    ["delete"] = $"/api/user/{created.Id}"
                });

                return CreatedAtAction(nameof(GetAll), new { id = created.Id }, resourceWithLinks);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Lista usuários com paginação.
        /// </summary>
        /// <param name="page">Página (>= 1).</param>
        /// <param name="pageSize">Itens por página.</param>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest(new { message = "Parâmetros de paginação inválidos: page >= 1 e pageSize >= 1" });

            var totalItems = await _usuarioService.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages > 0 && page > totalPages)
                return BadRequest(new { message = $"Página solicitada ({page}) maior que total de páginas ({totalPages})." });

            var users = await _usuarioService.GetAllAsync(page, pageSize);
            var itemsWithLinks = users.Select(u => HateoasHelper.AddLinks(u, new Dictionary<string, string>
            {
                ["self"] = $"/api/user/{u.Id}",
                ["update"] = $"/api/user/{u.Id}",
                ["delete"] = $"/api/user/{u.Id}"
            }));

            var envelope = new Shared.Iottu.Contracts.DTOs.PagedResponse<object>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = itemsWithLinks,
                Links = HateoasHelper.CollectionLinks("/api/user", page, pageSize, totalPages)
            };

            return Ok(envelope);
        }
    }
}