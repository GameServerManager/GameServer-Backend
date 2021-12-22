using GameServer.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.API.Hubs
{
    public class ConsoleHub : Hub
    {
        private readonly IServerService _service;
        private readonly ILogger<ConsoleHub> _logger;

        private readonly Dictionary<string, List<string>> AttatchedClients = new Dictionary<string, List<string>>();

        public ConsoleHub(ILogger<ConsoleHub> logger, IServerService service)
        {
            _logger = logger;
            _service = service;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            _logger.LogDebug($"Client connect: {Context.ConnectionId}");
        }
        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception); 
            
            _logger.LogDebug($"Client disconnect: {Context.ConnectionId} ({exception?.Message})");
        }

        public async Task Attach(string id)
        {
            var contains = AttatchedClients.TryGetValue(id, out var clientIds);
            _logger.LogDebug($"Client Attatched: {Context.ConnectionId}");

            if (!contains)
            {
                AttatchedClients.Add(id, new (){ Context.ConnectionId });
                await _service.Attach(id, (msg) => Replay(id, msg));
            }
            else
            {
                clientIds.Add(Context.ConnectionId);
            }
        }

        private void Replay(string id, string message)
        {
            var contains = AttatchedClients.TryGetValue(id, out var clientIds);

            if (contains)
            {
                _logger.LogDebug($"sending:\n    {message} \nto:\n    {string.Join(";", clientIds)}");
                Clients.Clients(clientIds).SendAsync("ConsoleMessage", id, message);
            }
        }
    }
}
