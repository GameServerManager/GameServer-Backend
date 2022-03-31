namespace GameServer.API.Services
{
    public interface IHubClientManager
    {
        void AddClient(string clientID);
        void RemoveClient(string clientID);
        void Attach(string clientID, string serverID, bool allLogs);
        void SendCommand(string containerID, string execId, string command);
    }
}
