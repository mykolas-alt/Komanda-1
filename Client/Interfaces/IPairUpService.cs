using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces {
    public interface IPairUpService {
        public Task SaveScoreAsync(string username, int score, int fails, GameDifficulty difficulty);
        public Task<UserScoreDto<PairUpData>> GetUserHighscoreAsync(string username);
        public Task<List<UserScoreDto<PairUpData>>> GetTopScoresAsync(int topCount);
    }
}
