using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;
using Web.Iottu.Api.Catalog.Helpers;

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
        public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _tagService.GetAllAsync(page, pageSize);
            var withLinks = result.Select(t => HateoasHelper.AddLinks(t, new Dictionary<string, string>
            {
                ["self"] = $"/api/tags/{t.Id}",
                ["update"] = $"/api/tags/{t.Id}",
                ["delete"] = $"/api/tags/{t.Id}"
            }));
            return Ok(withLinks);
        }

        // GET /api/tags/{id}
        [HttpGet("{id}")]
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

        // POST /api/tags
        [HttpPost]
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

        // PUT /api/tags/{id}
        [HttpPut("{id}")]
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

        // DELETE /api/tags/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _tagService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
