using GameServer.Host.Api;

namespace GameServer.API.Services
{
    public interface ILoggerService : IDisposable
    {
        Task StartPerformanceLogger(string id);
        Task StopPerformanceLogger(string id);
        Task<History> GetHistoryAsync(string id);
    }
}
