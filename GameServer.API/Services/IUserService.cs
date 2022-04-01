namespace GameServer.API.Services
{
    public interface IUserService
    {
        Task<string> Authenticate(string username, string password);
        Task<bool> Register(string username, string password, string maudiSecret);
    }
}
