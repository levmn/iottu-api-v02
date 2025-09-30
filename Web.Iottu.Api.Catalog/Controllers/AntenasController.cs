using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;
using Web.Iottu.Api.Catalog.Helpers;

namespace Web.Iottu.Api.Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AntenasController : ControllerBase
    {
        private readonly IAntenaService _antenaService;

        public AntenasController(IAntenaService antenaService)
        {
            _antenaService = antenaService;
        }

        // GET /api/antenas?page=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _antenaService.GetAllAsync(page, pageSize);
            var withLinks = result.Select(a => HateoasHelper.AddLinks(a, new Dictionary<string, string>
            {
                ["self"] = $"/api/antenas/{a.Id}",
                ["update"] = $"/api/antenas/{a.Id}",
                ["delete"] = $"/api/antenas/{a.Id}"
            }));
            return Ok(withLinks);
        }

        // GET /api/antenas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(Guid id)
        {
            var antena = await _antenaService.GetByIdAsync(id);
            if (antena == null) return NotFound();

            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/antenas/{id}",
                ["update"] = $"/api/antenas/{id}",
                ["delete"] = $"/api/antenas/{id}"
            };

            return Ok(HateoasHelper.AddLinks(antena, links));
        }

        // POST /api/antenas
        [HttpPost]
        public async Task<ActionResult<object>> Create([FromBody] CreateAntenaDto dto)
        {
            var created = await _antenaService.CreateAsync(dto);
            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/antenas/{created.Id}",
                ["update"] = $"/api/antenas/{created.Id}",
                ["delete"] = $"/api/antenas/{created.Id}"
            };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, HateoasHelper.AddLinks(created, links));
        }

        // PUT /api/antenas/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(Guid id, [FromBody] UpdateAntenaDto dto)
        {
            var updated = await _antenaService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();

            var links = new Dictionary<string, string>
            {
                ["self"] = $"/api/antenas/{updated.Id}",
                ["update"] = $"/api/antenas/{updated.Id}",
                ["delete"] = $"/api/antenas/{updated.Id}"
            };
            return Ok(HateoasHelper.AddLinks(updated, links));
        }

        // DELETE /api/antenas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _antenaService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
