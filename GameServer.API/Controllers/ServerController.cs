using GameServer.API.Models;
using GameServer.API.Services;
using GameServer.Host.Api;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GameServer.API.Controllers
{
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
        public async Task<IEnumerable<Server>> Get()
        {
            var list = await _service.GetAll();
            return list.AsEnumerable();
        }

        [HttpGet("{id}")]
        public async Task<Server> Get(string id)
        {
            var list = await _service.Get(id);
            return list;
        }

        [HttpPost("Import")]
        public async Task<ActionResult> Import([FromBody] ServerConfig config)
        {
            await _service.Import(config, (v) => WesocketSend(v));
            return Ok();
        }


        [HttpPost("{id}/Update")]
        public async Task<ActionResult> Update(string id)
        {
            await _service.Update(id);
            return Ok();
        }


        [HttpPost("{id}/Start")]
        public async Task<ActionResult> Start(string id)
        {
            await _service.Start(id);
            return Ok();
        }


        [HttpPost("{id}/Stop")]
        public async Task<ActionResult> Stop(string id)
        {
            await _service.Stop(id);
            return Ok();
        }


        [HttpPost("{id}/Attach")]
        public async Task<ActionResult> Attach(string id)
        {
            await _service.Attach(id, (v) => WesocketSend(v));
            return Ok();
        }


        [HttpPost("{id}/Log")]
        public async Task<string> Log(string id)
        {
            var log = await _service.GetLog(id);
            return log;
        }

        private void WesocketSend(string v)
        {
            Console.WriteLine(v);
        }
    }
}
