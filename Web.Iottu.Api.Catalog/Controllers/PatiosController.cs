using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;
using Web.Iottu.Api.Catalog.Helpers;

namespace Web.Iottu.Api.Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatiosController : ControllerBase
    {
        private readonly IPatioService _patioService;

        public PatiosController(IPatioService patioService)
        {
            _patioService = patioService;
        }

        // GET /api/patios?page=1&pageSize=10
        [HttpGet]
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

        // GET /api/patios/{id}
        [HttpGet("{id}")]
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

        // POST /api/patios
        [HttpPost]
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

        // PUT /api/patios/{id}
        [HttpPut("{id}")]
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

        // DELETE /api/patios/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _patioService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
