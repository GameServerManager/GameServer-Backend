using System.Text.Json.Serialization;

namespace GameServer.API.Models
{
    public class MountingPoint
    {
        [JsonPropertyName("hostPath")]
        public string HostPath { get; set; }
        [JsonPropertyName("serverPath")]
        public string ServerPath { get; set; }
    }
}