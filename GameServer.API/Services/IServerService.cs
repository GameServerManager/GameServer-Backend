using GameServer.API.Models;
using GameServer.Host.Api;

namespace GameServer.API.Services
{
    public interface IServerService : IDisposable
    {
        Task<Server> Get(string id);
        Task<List<Server>> GetAll();
        Task<string> Import(ServerConfig id);
        Task<Status> Start(string id);
        Task<Status> Stop(string id);
        Task Update(string id);
        Task<Dictionary<string, Dictionary<string, string>>> GetLog(string id);
        Task<Dictionary<string, Dictionary<string, string>>> GetActiveLogs(string id);
        Task Attach(string id, Action<Dictionary<string, Dictionary<string, string>>> callBack, Action<string> closeCallback);
        Task SendCommand(string containerID, string execId, string command);
    }
}
