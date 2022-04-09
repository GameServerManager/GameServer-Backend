using System.Text.Json.Serialization;

namespace GameServer.API.Models
{
    public class Variable
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("envVariable")]
        public string EnvVariable { get; set; }
        [JsonPropertyName("defaultValue")]
        public string DefaultValue { get; set; }
        [JsonPropertyName("userViewable")]
        public bool UserViewable { get; set; }
        [JsonPropertyName("userEditable")]
        public bool UserEditable { get; set; }
    }
}