using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces {
    public interface IAimTrainerService {
        public Task SaveScoreAsync(string username,int score);
        public Task<UserScoreDto<AimTrainerData>> GetUserHighscoreAsync(string username);
        public Task<List<UserScoreDto<AimTrainerData>>> GetTopScoresAsync(int topCount);
    }
}
