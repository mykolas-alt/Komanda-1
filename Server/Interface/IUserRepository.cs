using Projektas.Shared.Models;

namespace Projektas.Server.Interface {
	public interface IUserRepository {
		Task CreateUserAsync(User user);
		Task<List<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(int id);
		Task AddScoreToUserAsync(string username,int userScore);
		Task<List<UserScoreDto>> GetAllScoresAsync();
		Task<bool> ValidateUserAsync(string username,string password);
	}
}
