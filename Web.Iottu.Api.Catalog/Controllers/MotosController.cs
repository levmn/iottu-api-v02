using Core.Iottu.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Iottu.Contracts.DTOs;
using Web.Iottu.Api.Catalog.Helpers;

namespace Web.Iottu.Api.Catalog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly IMotoService _motoService;

        public MotosController(IMotoService motoService)
        {
            _motoService = motoService;
        }

        // GET /api/motos?page=1&pageSize=10
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var motos = await _motoService.GetAllAsync(page, pageSize);

            var result = motos.Select(m => HateoasHelper.AddLinks(m, new Dictionary<string, string>
            {
                ["self"] = $"/api/motos/{m.Id}",
                ["update"] = $"/api/motos/{m.Id}",
                ["delete"] = $"/api/motos/{m.Id}"
            }));

            return Ok(result);
        }

        // GET /api/motos/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        // POST /api/motos
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        // PUT /api/motos/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        // DELETE /api/motos/{id}
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
