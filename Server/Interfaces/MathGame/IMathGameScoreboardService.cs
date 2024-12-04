using Projektas.Shared.Models;

namespace Projektas.Server.Interfaces.MathGame {
    public interface IMathGameScoreboardService {
        public Task AddScoreToDbAsync(UserScoreDto<MathGameData> data);
        public Task<UserScoreDto<MathGameData>?> GetUserHighscoreAsync(string username);
        public Task<List<UserScoreDto<MathGameData>>> GetTopScoresAsync(int topCount);

	}
}
