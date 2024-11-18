using Projektas.Shared.Models;

namespace Projektas.Server.Interfaces
{
    public interface IUserService
    {
        public Task<bool> LogInToUser(User userInfo);

        public Task CreateUser(User newUser);

        public string GenerateJwtToken(User user);

        public Task<List<string>> GetUsernamesAsync();
    }
}
