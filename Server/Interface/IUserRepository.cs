using Projektas.Shared.Models;

namespace Projektas.Server.Interface {
	public interface IUserRepository {
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(int id);
		Task<int> CreateUserAsync(User user);
	}
}
