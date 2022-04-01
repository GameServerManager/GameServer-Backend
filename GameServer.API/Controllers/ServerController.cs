using GameServer.API.Models;
using GameServer.API.Services;
using GameServer.Host.Api;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameServer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        private IServerService _service;

        public ServerController(IServerService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = ("User"))]
        public async Task<IEnumerable<Server>> Get()
        {
            var list = await _service.GetAll();
            return list.AsEnumerable();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = ("User"))]
        public async Task<Server> Get(string id)
        {
            var list = await _service.Get(id);
            return list;
        }

        [HttpPost("Import")]
        [Authorize(Roles = ("User"))]
        public async Task<ActionResult> Import([FromBody] ServerConfig config)
        {
            await _service.Import(config, (v) => { });
            return Ok();
        }


        [HttpPost("{id}/Update")]
        [Authorize(Roles = ("User"))]
        public async Task<ActionResult> Update(string id)
        {
            await _service.Update(id);
            return Ok();
        }


        [HttpPost("{id}/Start")]
        [Authorize(Roles = ("User"))]
        public async Task<ActionResult> Start(string id)
        {
            await _service.Start(id);
            return Ok();
        }


        [HttpPost("{id}/Stop")]
        [Authorize(Roles = ("User"))]
        public async Task<ActionResult> Stop(string id)
        {
            await _service.Stop(id);
            return Ok();
        }

        [HttpGet("{id}/Log")]
        [Authorize(Roles = ("User"))]
        public async Task<string> Log(string id)
        {
            var log = await _service.GetLog(id);
            return JsonSerializer.Serialize(log);
        }
    }
}
