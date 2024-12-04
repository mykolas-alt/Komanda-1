using Projektas.Shared.Models;

namespace Projektas.Server.Services {
    public class AimTrainerService {
        private readonly IScoreRepository _scoreRepository;

        public AimTrainerService (IScoreRepository scoreRepository) {
            _scoreRepository=scoreRepository;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<AimTrainerData> data) {
            await _scoreRepository.AddScoreToUserAsync<AimTrainerData>(data.Username,data.GameData,data.GameData.Scores);
        }

        public async Task<UserScoreDto<AimTrainerData>?> GetUserHighscoreAsync(string username) {
            return await _scoreRepository.GetHighscoreFromUserAsync<AimTrainerData>(username);
        }

        public async Task<List<UserScoreDto<AimTrainerData>>> GetTopScoresAsync(int topCount) {
            List<UserScoreDto<AimTrainerData>> userScores=await _scoreRepository.GetAllScoresAsync<AimTrainerData>();
            List<UserScoreDto<AimTrainerData>> topScores=new List<UserScoreDto<AimTrainerData>>();
            
            for(int i=0;i<topCount && i<userScores.Count;i++) {
                topScores.Add(userScores[i]);
            }
            
            return topScores;
        }
    }
}
