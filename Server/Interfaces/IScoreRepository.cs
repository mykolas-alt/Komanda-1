using Projektas.Shared.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Services {
	public interface IScoreRepository {
		Task AddScoreToUserAsync<T>(string username,T gameInfo,object additionalInfo) where T : IGame;
		Task<int?> GetHighscoreFromUserAsync<T>(string username) where T : IGame;
		Task<List<UserScoreDto>> GetAllScoresAsync<T>() where T : IGame;
	}
}