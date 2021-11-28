using GameServer.Host.Api;

namespace GameServer.API.Services
{
    public interface ILoggerService : IDisposable
    {
        Task StartPerformanceLogger(string id);
        Task StopPerformanceLogger(string id);
        Task<List<History>> GetHistoryAsync(string id);
    }
}
