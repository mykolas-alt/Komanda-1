using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces {
    public interface IPairUpService {
        public Task SaveScoreAsync(string username,int score);
        public Task<int> GetUserHighscore(string username);
        public Task<List<UserScoreDto>> GetTopScoresAsync(int topCount);
    }
}
