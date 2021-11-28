using GameServer.Host.Api;
using Grpc.Net.Client;

namespace GameServer.API.Services
{
    public class HealthCheckService : IHealthCheckService
    {
        private GrpcChannel channel;
        private HealthCheckAPI.HealthCheckAPIClient client;

        public HealthCheckService(string url)
        {
            channel = GrpcChannel.ForAddress(url);
            client = new HealthCheckAPI.HealthCheckAPIClient(channel);
        }

        public void Dispose()
        {
            channel.Dispose();
        }

        public async Task<string> Echo(string msg)
        {
            var result = await client.EchoAsync(new HelloRequest() { Name = msg });

            return result.Message;
        }
    }
}
