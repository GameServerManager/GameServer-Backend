namespace GameServer.API.Models
{
    public class User
    {
        public string Role { get; set; }
        public string Username { get; set; }
        public string[] AccessibleServerIDs { get; set; }
        public byte[] Hash { get; set; }
        public string Salt { get; set; }
    }
}
