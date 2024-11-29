using Projektas.Shared.Models;

namespace Projektas.Server.Services {
    public class AimTrainerService {
        private readonly IScoreRepository _scoreRepository;

        public AimTrainerService (IScoreRepository scoreRepository) {
            _scoreRepository=scoreRepository;
        }

        public async Task AddScoreToDb(UserScoreDto data) {
            var aimTrainer=new AimTrainerModel {
                UserScores=data.Data
            };

            await _scoreRepository.AddScoreToUserAsync<AimTrainerModel>(data.Username,aimTrainer,data.Data);
        }

        public async Task<int?> GetUserHighscore(string username) {
            return await _scoreRepository.GetHighscoreFromUserAsync<AimTrainerModel>(username);
        }

        public async Task<List<UserScoreDto>> GetTopScores(int topCount) {
            List<UserScoreDto> userScores=await _scoreRepository.GetAllScoresAsync<AimTrainerModel>();
            List<UserScoreDto> topScores=new List<UserScoreDto>();
            
            for(int i=0;i<topCount && i<userScores.Count;i++) {
                topScores.Add(userScores[i]);
            }
            
            return topScores;
        }
    }
}
