using GameServer.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private IHealthCheckService _service;

        public HealthCheckController(IHealthCheckService service)
        {
            _service = service;
        }

        [HttpGet("{msg}")]
        public async Task<string> Get(string msg)
        {
            return await _service.Echo(msg);
        }
    }
}
