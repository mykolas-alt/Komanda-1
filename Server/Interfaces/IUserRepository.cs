using Projektas.Shared.Models;

namespace Projektas.Server.Interfaces {
	public interface IUserRepository {
        Task CreateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> ValidateUserAsync(string username,string password);
    }
}
