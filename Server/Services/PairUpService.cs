using Projektas.Shared.Models;

namespace Projektas.Server.Services {
    public class PairUpService {
        private readonly IScoreRepository _scoreRepository;

        public PairUpService (IScoreRepository scoreRepository) {
            _scoreRepository=scoreRepository;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<PairUpData> data) {
            await _scoreRepository.AddScoreToUserAsync<PairUpData>(data.Username,data.GameData,(data.GameData.TimeInSeconds,data.GameData.Fails));
        }

        public async Task<UserScoreDto<PairUpData>?> GetUserHighscoreAsync(string username) {
            return await _scoreRepository.GetHighscoreFromUserAsync<PairUpData>(username);
        }

        public async Task<List<UserScoreDto<PairUpData>>> GetTopScoresAsync(int topCount) {
            List<UserScoreDto<PairUpData>> userScores=await _scoreRepository.GetAllScoresAsync<PairUpData>();
            List<UserScoreDto<PairUpData>> topScores=new List<UserScoreDto<PairUpData>>();
            
            for(int i=0;i<topCount && i<userScores.Count;i++) {
                topScores.Add(userScores[i]);
            }
            
            return topScores;
        }
    }
}
