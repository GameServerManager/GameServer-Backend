using GameServer.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.API.Hubs
{
    public class ConsoleHub : Hub
    {
        private readonly ILogger<ConsoleHub> _logger;
        private readonly IHubClientManager _hubManager;

        public ConsoleHub(ILogger<ConsoleHub> logger, IHubClientManager hubManager)
        {
            _hubManager = hubManager;
            _logger = logger;
        }

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            _logger.LogDebug($"Client connect: {Context.ConnectionId}");
            _hubManager.AddClient(Context.ConnectionId);
        }

        public async override Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
            _logger.LogDebug($"Client disconnect: {Context.ConnectionId} ({exception?.Message})");
            _hubManager.RemoveClient(Context.ConnectionId);
        }

        [HubMethodName("Attach")]
        public async Task Attach(string id, bool allLogs)
        {
            _logger.LogDebug($"Client Attatched: {Context.ConnectionId}");
            _hubManager.Attach(Context.ConnectionId, id, allLogs);
        }

        [HubMethodName("SendCommand")]
        public async Task SendCommand(string containerID, string execId, string command)
        {
            _logger.LogDebug($"send Command: {command}");
            _hubManager.SendCommand(containerID, execId, command);
        }
    }
}
