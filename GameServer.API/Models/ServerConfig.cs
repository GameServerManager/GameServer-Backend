using System.Text.Json.Serialization;

namespace GameServer.API.Models
{
    public class ServerConfig
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("mounts")]
        public MountingPoint[] Mounts { get; set; }

        [JsonPropertyName("ports")]
        public PortMap[] Ports { get; set; } 

        [JsonPropertyName("containerScripts")]
        public ServerScripts ContainerScripts { get; set; } 

        [JsonPropertyName("variables")]
        public Variable[] Variables { get; set; }
    }
}