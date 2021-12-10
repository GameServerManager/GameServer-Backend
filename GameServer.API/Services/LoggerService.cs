using GameServer.Host.Api;
using Grpc.Net.Client;

namespace GameServer.API.Services
{
    public class LoggerService : ILoggerService
    {
        private GrpcChannel channel;
        private LoggerAPI.LoggerAPIClient client;

        public LoggerService(string url)
        {
            channel = GrpcChannel.ForAddress(url);
            client = new LoggerAPI.LoggerAPIClient(channel);
        }

        public void Dispose()
        {
            channel.Dispose();
        }

        public async Task<History> GetHistoryAsync(string id)
        {
            var response = await client.GetHistoryAsync(new GetHistoryRequest() { Id = id });
            return response;
        }

        public async Task StartPerformanceLogger(string id)
        {
            await client.StartPerformanceLoggerAsync(new StartLoggerRequest() { Id = id });
        }

        public async Task StopPerformanceLogger(string id)
        {
            await client.StopPerformanceLoggerAsync(new StopLoggerRequest() { Id = id });
        }
    }
}
