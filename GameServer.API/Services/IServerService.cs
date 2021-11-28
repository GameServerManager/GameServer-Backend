using GameServer.Host.Api;

namespace GameServer.API.Services
{
    public interface IServerService : IDisposable
    {
        Task<Server> Get(string id);
        Task<List<Server>> GetAll();
        Task Import(string id, Action<string> callBack);
        Task<Status> Start(string id);
        Task<Status> Stop(string id);
        Task Update(string id);
        Task<string> GetLog(string id);
        Task Attach(string id, Action<string> callBack);
    }
}
