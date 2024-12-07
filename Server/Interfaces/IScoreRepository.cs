using Projektas.Shared.Enums;
using Projektas.Shared.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Interfaces {
	public interface IScoreRepository {
		Task AddScoreToUserAsync<T>(string username, T gameInfo) where T : IGame;
		Task<List<UserScoreDto<T>>> GetHighscoreFromUserAsync<T>(string username) where T : IGame;
		Task<List<UserScoreDto<T>>> GetAllScoresAsync<T>() where T : IGame;
	}
}