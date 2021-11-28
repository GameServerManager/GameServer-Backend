namespace GameServer.API.Services
{
    public interface IHealthCheckService : IDisposable
    {
        Task<string> Echo(string msg);
    }
}
