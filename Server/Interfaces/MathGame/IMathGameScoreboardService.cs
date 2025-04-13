using Projektas.Shared.Models;
using Projektas.Shared.Enums;

namespace Projektas.Server.Interfaces.MathGame {
    public interface IMathGameScoreboardService {
        public Task AddScoreToDbAsync(UserScoreDto<MathGameData> data);
        public Task<UserScoreDto<MathGameData>> GetUserHighscoreAsync(string username, GameDifficulty difficulty);
        public Task<List<UserScoreDto<MathGameData>>> GetTopScoresAsync(int topCount, GameDifficulty difficulty);

	}
}
