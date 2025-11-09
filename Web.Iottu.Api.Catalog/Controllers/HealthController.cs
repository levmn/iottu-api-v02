using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Web.Iottu.Api.Catalog.Helpers;

namespace Web.Iottu.Api.Catalog.Controllers
{
    /// <summary>
    /// Endpoint de monitoramento de saúde da aplicação.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// Verifica o status de saúde da API e suas dependências.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();

            var response = new
            {
                status = report.Status.ToString(),
                totalDuration = report.TotalDuration.TotalMilliseconds,
                entries = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    duration = e.Value.Duration.TotalMilliseconds
                })
            };

            var links = new Dictionary<string, string>
            {
                ["self"] = "/api/health",
                ["swagger"] = "/swagger",
                ["api_root"] = "/api"
            };

            var hateoasResponse = HateoasHelper.AddLinks(response, links);

            return report.Status == HealthStatus.Healthy
                ? Ok(hateoasResponse)
                : StatusCode(StatusCodes.Status503ServiceUnavailable, hateoasResponse);
        }
    }
}
