using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;

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
        public async Task<ActionResult<IEnumerable<AntenaDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _antenaService.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        // GET /api/antenas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AntenaDto>> GetById(Guid id)
        {
            var antena = await _antenaService.GetByIdAsync(id);
            if (antena == null) return NotFound();

            return Ok(antena);
        }

        // POST /api/antenas
        [HttpPost]
        public async Task<ActionResult<AntenaDto>> Create([FromBody] CreateAntenaDto dto)
        {
            var created = await _antenaService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
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
