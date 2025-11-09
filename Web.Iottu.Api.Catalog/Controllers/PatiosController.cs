using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;
using Web.Iottu.Api.Catalog.Helpers;

namespace Web.Iottu.Api.Catalog.Controllers
{
    /// <summary>
    /// Endpoints para gerenciamento de Patios.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class PatiosController : ControllerBase
    {
        private readonly IPatioService _patioService;

        public PatiosController(IPatioService patioService)
        {
            _patioService = patioService;
        }

        /// <summary>
        /// Lista patios com paginação.
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

            var totalItems = await _patioService.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages > 0 && page > totalPages)
                return BadRequest(new { message = $"Página solicitada ({page}) maior que total de páginas ({totalPages})." });

            var patios = await _patioService.GetAllAsync(page, pageSize);
            var result = patios.Select(p => HateoasHelper.AddLinks(p, new Dictionary<string, string>
            {
                ["self"] = $"/api/patios/{p.Id}",
                ["update"] = $"/api/patios/{p.Id}",
                ["delete"] = $"/api/patios/{p.Id}"
            }));

            var envelope = new Shared.Iottu.Contracts.DTOs.PagedResponse<object>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = result,
                Links = HateoasHelper.CollectionLinks("/api/patios", page, pageSize, totalPages)
            };

            return Ok(envelope);
        }

        /// <summary>
        /// Obtém um patio por id.
        /// </summary>
        /// <param name="id">Id do patio.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetById(Guid id)
        {
            var patio = await _patioService.GetByIdAsync(id);
            if (patio == null) return NotFound();

            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/patios/{id}",
                ["update"] = $"/api/patios/{id}",
                ["delete"] = $"/api/patios/{id}"
            };
            return Ok(HateoasHelper.AddLinks(patio, links));
        }

        /// <summary>
        /// Cria um novo patio.
        /// </summary>
        /// <param name="dto">Dados de criação do patio.</param>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> Create([FromBody] CreatePatioDto dto)
        {
            var created = await _patioService.CreateAsync(dto);
            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/patios/{created.Id}",
                ["update"] = $"/api/patios/{created.Id}",
                ["delete"] = $"/api/patios/{created.Id}"
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, HateoasHelper.AddLinks(created, links));
        }

        /// <summary>
        /// Atualiza um patio existente.
        /// </summary>
        /// <param name="id">Id do patio.</param>
        /// <param name="dto">Dados para atualização.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> Update(Guid id, [FromBody] UpdatePatioDto dto)
        {
            var updated = await _patioService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/patios/{updated.Id}",
                ["update"] = $"/api/patios/{updated.Id}",
                ["delete"] = $"/api/patios/{updated.Id}"
            };
            return Ok(HateoasHelper.AddLinks(updated, links));
        }

        /// <summary>
        /// Remove um patio.
        /// </summary>
        /// <param name="id">Id do patio.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _patioService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
