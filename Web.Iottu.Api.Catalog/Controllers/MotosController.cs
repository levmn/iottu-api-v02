using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;
using Web.Iottu.Api.Catalog.Helpers;

namespace Web.Iottu.Api.Catalog.Controllers
{
    /// <summary>
    /// Endpoints para gerenciamento de Motos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MotosController : ControllerBase
    {
        private readonly IMotoService _motoService;

        public MotosController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        /// <summary>
        /// Lista motos com paginação.
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

            var totalItems = await _motoService.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages > 0 && page > totalPages)
                return BadRequest(new { message = $"Página solicitada ({page}) maior que total de páginas ({totalPages})." });

            var motos = await _motoService.GetAllAsync(page, pageSize);
            var itemsWithLinks = motos.Select(m => HateoasHelper.AddLinks(m, new Dictionary<string, string>
            {
                ["self"] = $"/api/motos/{m.Id}",
                ["update"] = $"/api/motos/{m.Id}",
                ["delete"] = $"/api/motos/{m.Id}"
            }));

            var envelope = new Shared.Iottu.Contracts.DTOs.PagedResponse<object>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = itemsWithLinks,
                Links = HateoasHelper.CollectionLinks("/api/motos", page, pageSize, totalPages)
            };

            return Ok(envelope);
        }

        /// <summary>
        /// Obtém uma moto por id.
        /// </summary>
        /// <param name="id">Id da moto.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetById(Guid id)
        {
            var moto = await _motoService.GetByIdAsync(id);
            if (moto == null) return NotFound();

            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/motos/{id}",
                ["update"] = $"/api/motos/{id}",
                ["delete"] = $"/api/motos/{id}"
            };

            return Ok(HateoasHelper.AddLinks(moto, links));
        }

        /// <summary>
        /// Cria uma nova moto.
        /// </summary>
        /// <param name="dto">Dados de criação da moto.</param>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> Create([FromBody] CreateMotoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _motoService.CreateAsync(dto);

            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/motos/{created.Id}",
                ["update"] = $"/api/motos/{created.Id}",
                ["delete"] = $"/api/motos/{created.Id}"
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, HateoasHelper.AddLinks(created, links));
        }

        /// <summary>
        /// Atualiza uma moto existente.
        /// </summary>
        /// <param name="id">Id da moto.</param>
        /// <param name="dto">Dados de atualização.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> Update(Guid id, [FromBody] UpdateMotoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _motoService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/motos/{updated.Id}",
                ["update"] = $"/api/motos/{updated.Id}",
                ["delete"] = $"/api/motos/{updated.Id}"
            };

            return Ok(HateoasHelper.AddLinks(updated, links));
        }

        /// <summary>
        /// Remove uma moto.
        /// </summary>
        /// <param name="id">Id da moto.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _motoService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
