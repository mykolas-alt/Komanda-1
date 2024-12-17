using Projektas.Server.Interfaces;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;
using System.Drawing;

namespace Projektas.Server.Services {
    public class PairUpService {
        private readonly IScoreRepository _scoreRepository;

        public PairUpService (IScoreRepository scoreRepository) {
            _scoreRepository = scoreRepository;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<PairUpData> data) {
            await _scoreRepository.AddScoreToUserAsync<PairUpData>(data.Username, data.GameData, data.Timestamp);
        }

        public async Task<UserScoreDto<PairUpData>> GetUserHighscoreAsync(string username, GameDifficulty difficulty) {
            var scores = await _scoreRepository.GetHighscoreFromUserAsync<PairUpData>(username);

            var filteredScores = scores
                .Where(s => s.GameData.Difficulty == difficulty)
                .OrderBy(s => s.GameData.TimeInSeconds)
                .ThenBy(s => s.GameData.Fails)
                .ToList();

            if(!filteredScores.Any()) {
                return null;
            }

            return filteredScores.First();
        }

        public async Task<List<UserScoreDto<PairUpData>>> GetTopScoresAsync(int topCount, GameDifficulty difficulty) {
            List<UserScoreDto<PairUpData>> userScores = await _scoreRepository.GetAllScoresAsync<PairUpData>();
            List<UserScoreDto<PairUpData>> orderedScores = userScores
                .Where(s => !s.IsPrivate && s.GameData.Difficulty == difficulty)
                .OrderBy(s => s.GameData.TimeInSeconds)
                .ThenBy(s => s.GameData.Fails)
                .ToList();
            List<UserScoreDto<PairUpData>> topScores = new List<UserScoreDto<PairUpData>>();
            
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
