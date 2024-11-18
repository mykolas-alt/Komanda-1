using Projektas.Shared.Models;

namespace Projektas.Server.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddMathGameScoreToUserAsync(string username, int userScore);
        Task<int?> GetMathGameHighscoreFromUserAsync(string username);
        Task<List<UserScoreDto>> GetAllMathGameScoresAsync();
        Task<bool> ValidateUserAsync(string username, string password);
    }
}
