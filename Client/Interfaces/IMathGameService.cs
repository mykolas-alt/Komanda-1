using Projektas.Shared.Models;
using Projektas.Shared.Enums;

namespace Projektas.Client.Interfaces {
    public interface IMathGameService {
        public Task<string> GetQuestionAsync(int score, GameDifficulty difficulty);
        public Task<List<int>> GetOptionsAsync();
        public Task<bool> CheckAnswerAsync(int option);
        public Task SaveScoreAsync(string username, int score);
        public Task<UserScoreDto<MathGameData>> GetUserHighscoreAsync(string username, GameDifficulty difficulty);
		public Task<List<UserScoreDto<MathGameData>>> GetTopScoresAsync(GameDifficulty difficulty, int topCount);
    }
}
