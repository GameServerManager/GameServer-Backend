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
        private IDatabaseService _databaseService;

        public ServerController(IServerService service, IDatabaseService databaseService)
        {
            _service = service;
            _databaseService = databaseService;
        }

        [HttpGet]
        [Authorize(Roles = ("User"))]
        public async Task<IEnumerable<Server>> Get()
        {
            var user = await _databaseService.GetUser(HttpContext.User.Identity.Name);

            var serverList = new List<Server>();
            foreach (var id in user.AccessibleServerIDs)
            {
                try
                {
                    serverList.Add(await _service.Get(id));
                }
                catch (Exception){}
            }
            return serverList;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = ("User"))]
        public async Task<Server> Get(string id)
        {
            if (! await CheckPermission(id, HttpContext.User.Identity.Name))
                return null;

            var list = await _service.Get(id);
            return list;
        }

        [HttpPost("Import")]
        [Authorize(Roles = ("User"))]
        public async Task<ActionResult> Import([FromBody] ServerConfig config)
        {
            var id = await _service.Import(config);
            await _databaseService.AddServerToUser(HttpContext.User.Identity.Name, id);
            return Ok(id);
        }

        [HttpPost("{id}/Update")]
        [Authorize(Roles = ("User"))]
        public async Task<ActionResult> Update(string id)
        {
            if (! await CheckPermission(id, HttpContext.User.Identity.Name))
                return Forbid();

            await _service.Update(id);
            return Ok();
        }


        [HttpPost("{id}/Start")]
        [Authorize(Roles = ("User"))]
        public async Task<ActionResult> Start(string id)
        {
            if (! await CheckPermission(id, HttpContext.User.Identity.Name))
                return Forbid();

            await _service.Start(id);
            return Ok();
        }


        [HttpPost("{id}/Stop")]
        [Authorize(Roles = ("User"))]
        public async Task<ActionResult> Stop(string id)
        {
            if(! await CheckPermission(id, HttpContext.User.Identity.Name))
                return Forbid();

            await _service.Stop(id);
            return Ok();
        }

        [HttpGet("{id}/Log")]
        [Authorize(Roles = ("User"))]
        public async Task<string> Log(string id)
        {
            if (! await CheckPermission(id, HttpContext.User.Identity.Name))
                return "";

            var log = await _service.GetLog(id);
            return JsonSerializer.Serialize(log);
        }

        private async Task<bool> CheckPermission(string serverID, string clientID)
        {
            var user = await _databaseService.GetUser(clientID);

            return user.AccessibleServerIDs.Contains(serverID);
        }
    }
}
