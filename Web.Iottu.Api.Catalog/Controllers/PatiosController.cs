using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;
using Web.Iottu.Api.Catalog.Helpers;

namespace Web.Iottu.Api.Catalog.Controllers
{
    /// <summary>
    /// Endpoints para gerenciamento de Patios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
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
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var patios = await _patioService.GetAllAsync(page, pageSize);
            var result = patios.Select(p => HateoasHelper.AddLinks(p, new Dictionary<string, string>
            {
                ["self"] = $"/api/patios/{p.Id}",
                ["update"] = $"/api/patios/{p.Id}",
                ["delete"] = $"/api/patios/{p.Id}"
            }));
            return Ok(result);
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
