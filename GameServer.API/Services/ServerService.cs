using GameServer.Host.Api;
using Grpc.Net.Client;

namespace GameServer.API.Services
{
    public class ServerService : IServerService
    {
        private GrpcChannel channel;
        private ServerAPI.ServerAPIClient client;

        public ServerService(string url)
        {
            channel = GrpcChannel.ForAddress(url);
            client = new ServerAPI.ServerAPIClient(channel);

        }

        public async Task Attach(string id, Action<string> callBack)
        {
            var stream = client.Attach(new AttachRequest() { Id = id });
            var cToken = new CancellationTokenSource();
            while (await stream.ResponseStream.MoveNext(cToken.Token))
            {
                callBack(stream.ResponseStream.Current.Msg);
            }
        }

        public void Dispose()
        {
            channel.Dispose();
        }

        public async Task<Server> Get(string id)
        {
            return await client.GetAsync(new ServerRequest() { Id = id });
        }

        public async Task<List<Server>> GetAll()
        {
            var response = await client.GetAllAsync(new Google.Protobuf.WellKnownTypes.Empty());
            return response.Servers.ToList();
        }

        public async Task<string> GetLog(string id)
        {
            var result = await client.GetLogAsync(new LogRequest() { Id = id });
            return result.Log;
        }

        public async Task Import(string id, Action<string> callBack)
        {
            var stream = client.Import(new ImportRequest() { Id = id });
            var cToken = new CancellationTokenSource();
            while (await stream.ResponseStream.MoveNext(cToken.Token))
            {
                callBack(stream.ResponseStream.Current.Msg);
            }
        }

        public async Task<Status> Start(string id)
        {
            var result = await client.StartAsync(new StartRequest() { Id = id });
            return result;
        }

        public async Task<Status> Stop(string id)
        {
            var result = await client.StopAsync(new StopRequest() { Id = id });
            return result;
        }

        public async Task Update(string id)
        {
            await client.UpdateAsync(new UpdateRequest() { Id = id });
        }
    }
}
