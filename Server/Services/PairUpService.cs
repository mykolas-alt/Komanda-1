using Projektas.Server.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Services {
    public class PairUpService {
        private readonly IScoreRepository _scoreRepository;

        public PairUpService (IScoreRepository scoreRepository) {
            _scoreRepository = scoreRepository;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<PairUpData> data) {
            await _scoreRepository.AddScoreToUserAsync<PairUpData>(data.Username, data.GameData, data.Timestamp);
        }

        public async Task<UserScoreDto<PairUpData>> GetUserHighscoreAsync(string username) {
            var scores = await _scoreRepository.GetHighscoreFromUserAsync<PairUpData>(username);

            return scores.OrderBy(s => s.GameData.TimeInSeconds).ThenBy(s => s.GameData.Fails).First();
        }

        public async Task<List<UserScoreDto<PairUpData>>> GetTopScoresAsync(int topCount) {
            List<UserScoreDto<PairUpData>> userScores = await _scoreRepository.GetAllScoresAsync<PairUpData>();
            List<UserScoreDto<PairUpData>> orderedScores = userScores.OrderByDescending(s => s.GameData.TimeInSeconds).ThenBy(s => s.GameData.Fails).ToList();
            List<UserScoreDto<PairUpData>> topScores = new List<UserScoreDto<PairUpData>>();
            
            for(int i = 0; i < topCount && i < orderedScores.Count; i++) {
                topScores.Add(orderedScores[i]);
            }
            
            return topScores;
        }
    }
}
