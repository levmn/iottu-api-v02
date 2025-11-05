using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;
using Web.Iottu.Api.Catalog.Helpers;

namespace Web.Iottu.Api.Catalog.Controllers
{
    /// <summary>
    /// Endpoints para gerenciamento de Tags.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Lista tags com paginação.
        /// </summary>
        /// <param name="page">Página (>= 1).</param>
        /// <param name="pageSize">Itens por página.</param>
        /// <returns>Lista paginada de tags com HATEOAS.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest(new { message = "Parâmetros de paginação inválidos: page >= 1 e pageSize >= 1" });

            var totalItems = await _tagService.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages > 0 && page > totalPages)
                return BadRequest(new { message = $"Página solicitada ({page}) maior que total de páginas ({totalPages})." });

            var result = await _tagService.GetAllAsync(page, pageSize);
            var withLinks = result.Select(t => HateoasHelper.AddLinks(t, new Dictionary<string, string>
            {
                ["self"] = $"/api/tags/{t.Id}",
                ["update"] = $"/api/tags/{t.Id}",
                ["delete"] = $"/api/tags/{t.Id}"
            }));

            var envelope = new Shared.Iottu.Contracts.DTOs.PagedResponse<object>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = withLinks,
                Links = HateoasHelper.CollectionLinks("/api/tags", page, pageSize, totalPages)
            };

            return Ok(envelope);
        }

        /// <summary>
        /// Obtém uma tag por id.
        /// </summary>
        /// <param name="id">Id da tag.</param>
        /// <returns>Tag com HATEOAS.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<object>> GetById(Guid id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null) return NotFound();

            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/tags/{id}",
                ["update"] = $"/api/tags/{id}",
                ["delete"] = $"/api/tags/{id}"
            };

            return Ok(HateoasHelper.AddLinks(tag, links));
        }

        /// <summary>
        /// Cria uma nova tag.
        /// </summary>
        /// <param name="dto">Dados de criação da tag.</param>
        /// <returns>Tag criada com HATEOAS.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> Create([FromBody] CreateTagDto dto)
        {
            var created = await _tagService.CreateAsync(dto);
            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/tags/{created.Id}",
                ["update"] = $"/api/tags/{created.Id}",
                ["delete"] = $"/api/tags/{created.Id}"
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, HateoasHelper.AddLinks(created, links));
        }

        /// <summary>
        /// Atualiza uma tag.
        /// </summary>
        /// <param name="id">Id da tag.</param>
        /// <param name="dto">Dados para atualização.</param>
        /// <returns>Tag atualizada com HATEOAS.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<object>> Update(Guid id, [FromBody] UpdateTagDto dto)
        {
            var updated = await _tagService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/tags/{updated.Id}",
                ["update"] = $"/api/tags/{updated.Id}",
                ["delete"] = $"/api/tags/{updated.Id}"
            };
            return Ok(HateoasHelper.AddLinks(updated, links));
        }

        /// <summary>
        /// Remove uma tag.
        /// </summary>
        /// <param name="id">Id da tag.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _tagService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
