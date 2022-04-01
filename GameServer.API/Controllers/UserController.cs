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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        async public Task<string> Authenticate([FromBody] Credentials model)
        {
            string token;
            token = await _service.Authenticate(model.Username, model.Password);

            if (token == null)
                return null;
            return token;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        async public Task<bool> Register([FromBody] Register model)
        {
            bool regComplete;
            regComplete = await _service.Register(model.Username, model.Password, model.MaudiSecret);

            return regComplete;
        }
    }
}
