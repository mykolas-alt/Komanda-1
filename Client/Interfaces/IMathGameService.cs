using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces {
    public interface IMathGameService {
        public Task<string> GetQuestionAsync(int score);
        public Task<List<int>> GetOptionsAsync();
        public Task<bool> CheckAnswerAsync(int option);
        public Task SaveScoreAsync(string username,int score);
        public Task<UserScoreDto<MathGameData>?> GetUserHighscoreAsync(string username);
		public Task<List<UserScoreDto<MathGameData>>> GetTopScoresAsync(int topCount);
    }
}
