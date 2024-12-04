using Projektas.Shared.Models;

namespace Projektas.Server.Interfaces {
    public interface IUserService {
        public Task CreateUserAsync(User newUser);
        public Task<bool> LogInToUserAsync(User userInfo);
        public void LogOffFromUser(string username);
        public Task<List<string>> GetUsernamesAsync();
        public string GenerateJwtToken(User user);
    }
}
