using GameServer.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GameServer.API.Services
{
    public class HubClientManager : IHubClientManager
    {
        private readonly Dictionary<string, List<string>> AttatchedClients = new Dictionary<string, List<string>>();
        private readonly IHubContext<ConsoleHub> _hubContext;
        private readonly IServerService _serverService;

        public HubClientManager(IHubContext<ConsoleHub> hubContext, IServerService serverService)
        {
            _serverService = serverService;
            _hubContext = hubContext;
        }

        public void AddClient(string clientID)
        {}

        public async void Attach(string clientID, string serverID, bool allLogs)
        {
            var contains = AttatchedClients.TryGetValue(serverID, out var clientIds);

            Dictionary<string, Dictionary<string, string>> logs;
            if (allLogs)
            {
                logs = await _serverService.GetLog(serverID);
            }
            else
            {
                logs = await _serverService.GetActiveLogs(serverID);
            }
            await _hubContext.Clients.Client(clientID).SendAsync("ConsoleMessage", serverID, logs);
            if (!contains)
            {
                AttatchedClients.Add(serverID, new() { clientID });
                 _ = _serverService.Attach(serverID, msgTree => Replay(serverID, msgTree), execID => Close(serverID, execID));
            }
            else
            {
                clientIds.Add(clientID);
            }
        }

        public void RemoveClient(string clientID)
        {
            foreach (var ids in AttatchedClients.Values)
            {
                ids.Remove(clientID);
            }
        }

        public async void SendCommand(string containerID, string execId, string command)
        {
            await _serverService.SendCommand(containerID, execId, command);
        }

        private async void Replay(string id, Dictionary<string, Dictionary<string, string>> messageTree)
        {
            var contains = AttatchedClients.TryGetValue(id, out var clientIds);

            if (contains)
            {
                await _hubContext.Clients.Clients(clientIds).SendAsync("ConsoleMessage", id, messageTree);
            }
        }

        private async void Close(string id, string execID)
        {
            var contains = AttatchedClients.TryGetValue(id, out var clientIds);

            if (contains)
            {
                await _hubContext.Clients.Clients(clientIds).SendAsync("StdOutClosed", id, execID);
            }
        }
    }
}
