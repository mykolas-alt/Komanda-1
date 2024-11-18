using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces
{
    public interface IMathGameService
    {
        public Task<string> GetQuestionAsync(int score);
        public Task<List<int>> GetOptionsAsync();
        public Task<bool> CheckAnswerAsync(int option);
        public Task SaveScoreAsync(string username, int score);
        public Task<int> GetUserHighscore(string username);

		public Task<List<UserScoreDto>> GetTopScoresAsync(int topCount);
    }
}
