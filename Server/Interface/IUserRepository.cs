using Projektas.Shared.Models;

namespace Projektas.Server.Interface {
	public interface IUserRepository {
		Task<int> CreateUserAsync(User user);
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(int id);
	}
}
