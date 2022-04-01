using System.ComponentModel;

namespace GameServer.API.Models
{
    public class DataProviderSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        [DefaultValue(true)]
        public string Host { get; set; } = "localhost";
        [DefaultValue(true)]
        public string Port { get; set; } = "27017";

    }
}
