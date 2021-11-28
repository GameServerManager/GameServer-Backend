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

        // GET: api/<ServerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ServerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ServerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ServerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
