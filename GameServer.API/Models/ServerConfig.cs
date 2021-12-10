namespace GameServer.API.Models
{
    public class ServerConfig
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Discription { get; set; }
        public string Image { get; set; }
        public MountingPoint[] Mounts { get; set; }
        public PortMap[] Ports { get; set; } 
        public ServerScripts ContainerScripts { get; set; } 
        public Variable[] Variables { get; set; }
    }
}