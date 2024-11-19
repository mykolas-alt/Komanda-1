using Projektas.Shared.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Services {
	public interface IScoreRepository<T> where T : IGame {
		Task AddScoreToUserAsync(string username,int userScore);
		Task<int?> GetHighscoreFromUserAsync(string username);
		Task<List<UserScoreDto>> GetAllScoresAsync();
	}
}