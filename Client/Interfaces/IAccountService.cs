using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces {
    public interface IAccountService {
        public Task<string?> LogInAsync(User user);
        public void LogOffAsync(string username);
        public Task<HttpResponseMessage> CreateAccountAsync(User user);
        public Task<List<string>> GetUsernamesAsync();
    }
}
