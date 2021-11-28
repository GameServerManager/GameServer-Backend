using GameServer.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GameServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private ILoggerService _service;

        public LoggerController(ILoggerService service)
        {
            _service = service;
        }
    }
}
