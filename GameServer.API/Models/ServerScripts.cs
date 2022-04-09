using System.Text.Json.Serialization;

namespace GameServer.API.Models
{
    public class ServerScripts
    {
        [JsonPropertyName("instalationScript")]
        public Script InstalationScript { get; set; }
        [JsonPropertyName("startScript")]
        public Script StartScript { get; set; } 
        [JsonPropertyName("updateScript")]
        public Script UpdateScript { get; set; }
    }
}