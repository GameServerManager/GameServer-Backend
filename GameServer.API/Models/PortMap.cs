using System.Text.Json.Serialization;

namespace GameServer.API.Models
{
    public class PortMap
    {
        [JsonPropertyName("hostPorts")]
        public string[] HostPorts { get; set; }
        [JsonPropertyName("serverPort")]
        public string ServerPort { get; set; }
    }
}