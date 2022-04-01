using GameServer.API.Models;

namespace GameServer.API.Services
{
    public interface IDatabaseService
    {
        Task<User> GetUser(string username);
        Task SaveNewUser(User user);

    }
}
