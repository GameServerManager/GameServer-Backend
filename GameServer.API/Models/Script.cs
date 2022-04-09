using System.Text.Json.Serialization;

namespace GameServer.API.Models
{
    public class Script
    {
        [JsonPropertyName("scriptCommand")]
        public string ScriptCommand { get; set; }
        [JsonPropertyName("entrypoint")]
        public string Entrypoint { get; set; }

    }
}