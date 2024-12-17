using Projektas.Server.Interfaces;
using Projektas.Shared.Models;
using Projektas.Shared.Enums;

namespace Projektas.Server.Services {
    public class AimTrainerService {
        private readonly IScoreRepository _scoreRepository;

        public AimTrainerService (IScoreRepository scoreRepository) {
            _scoreRepository = scoreRepository;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<AimTrainerData> data) {
            await _scoreRepository.AddScoreToUserAsync<AimTrainerData>(data.Username, data.GameData, data.Timestamp);
        }

        public async Task<UserScoreDto<AimTrainerData>> GetUserHighscoreAsync(string username, GameDifficulty difficulty) {
            var scores = await _scoreRepository.GetHighscoreFromUserAsync<AimTrainerData>(username);
            
            var filteredScores = scores
                .Where(s => s.GameData.Difficulty == difficulty)
                .OrderByDescending(s => s.GameData.Scores)
                .ToList();

            if(!filteredScores.Any()) {
                return null;
            }

            return filteredScores.First();
        }

        public async Task<List<UserScoreDto<AimTrainerData>>> GetTopScoresAsync(int topCount, GameDifficulty difficulty) {
            List<UserScoreDto<AimTrainerData>> userScores = await _scoreRepository.GetAllScoresAsync<AimTrainerData>();
            List<UserScoreDto<AimTrainerData>> orderedScores = userScores
                .Where(s => !s.IsPrivate && s.GameData.Difficulty == difficulty)
                .OrderByDescending(s => s.GameData.Scores)
                .ToList();
            List<UserScoreDto<AimTrainerData>> topScores = new List<UserScoreDto<AimTrainerData>>();
            
            if(!orderedScores.Any()) {
                return topScores;
            }

            for(int i = 0; i < topCount && i < orderedScores.Count; i++) {
                topScores.Add(orderedScores[i]);
            }
            
            return topScores;
        }
    }
}
