using Projektas.Server.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Services {
    public class AimTrainerService {
        private readonly IScoreRepository _scoreRepository;

        public AimTrainerService (IScoreRepository scoreRepository) {
            _scoreRepository = scoreRepository;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<AimTrainerData> data) {
            await _scoreRepository.AddScoreToUserAsync<AimTrainerData>(data.Username, data.GameData);
        }

        public async Task<UserScoreDto<AimTrainerData>> GetUserHighscoreAsync(string username) {
            var scores = await _scoreRepository.GetHighscoreFromUserAsync<AimTrainerData>(username);

            return scores.OrderByDescending(s => s.GameData.Scores).First();
        }

        public async Task<List<UserScoreDto<AimTrainerData>>> GetTopScoresAsync(int topCount) {
            List<UserScoreDto<AimTrainerData>> userScores = await _scoreRepository.GetAllScoresAsync<AimTrainerData>();
            List<UserScoreDto<AimTrainerData>> orderedScores = userScores.OrderByDescending(s => s.GameData.Scores).ToList();
            List<UserScoreDto<AimTrainerData>> topScores = new List<UserScoreDto<AimTrainerData>>();
            
            for(int i = 0; i < topCount && i < orderedScores.Count; i++) {
                topScores.Add(orderedScores[i]);
            }
            
            return topScores;
        }
    }
}
