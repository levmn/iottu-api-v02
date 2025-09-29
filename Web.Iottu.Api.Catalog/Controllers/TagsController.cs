using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;

namespace Web.Iottu.Api.Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET /api/tags?page=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _tagService.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        // GET /api/tags/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TagDto>> GetById(Guid id)
        {
            var tag = await _tagService.GetByIdAsync(id);
            if (tag == null) return NotFound();

            return Ok(tag);
        }

        // POST /api/tags
        [HttpPost]
        public async Task<ActionResult<TagDto>> Create([FromBody] CreateTagDto dto)
        {
            var created = await _tagService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // DELETE /api/tags/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _tagService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }

        // PUT /api/tags/{id}/ativar
        [HttpPut("{id}/ativar")]
        public async Task<IActionResult> Ativar(Guid id)
        {
            var result = await _tagService.AtivarAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }

        // PUT /api/tags/{id}/desativar
        [HttpPut("{id}/desativar")]
        public async Task<IActionResult> Desativar(Guid id)
        {
            var result = await _tagService.DesativarAsync(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
