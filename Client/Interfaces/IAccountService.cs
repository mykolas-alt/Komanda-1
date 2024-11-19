using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces {
    public interface IAccountService {
        public Task<string?> LogIn(User user);
        public void LogOff(string username);
        public Task<HttpResponseMessage> CreateAccountAsync(User user);
        public Task<List<string>> GetUsernames();
    }
}
