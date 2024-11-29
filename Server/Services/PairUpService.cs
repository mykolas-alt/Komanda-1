using Projektas.Shared.Models;

namespace Projektas.Server.Services {
    public class PairUpService {
        private readonly IScoreRepository _scoreRepository;

        public PairUpService (IScoreRepository scoreRepository) {
            _scoreRepository=scoreRepository;
        }

        public async Task AddScoreToDb(UserScoreDto data) {
            var pairUp=new PairUpModel {
                UserTimeInSeconds=data.Data
            };

            await _scoreRepository.AddScoreToUserAsync<PairUpModel>(data.Username,pairUp,data.Data);
        }

        public async Task<int?> GetUserHighscore(string username) {
            return await _scoreRepository.GetHighscoreFromUserAsync<PairUpModel>(username);
        }

        public async Task<List<UserScoreDto>> GetTopScores(int topCount) {
            List<UserScoreDto> userScores=await _scoreRepository.GetAllScoresAsync<PairUpModel>();
            List<UserScoreDto> topScores=new List<UserScoreDto>();
            
            for(int i=0;i<topCount && i<userScores.Count;i++) {
                topScores.Add(userScores[i]);
            }
            
            return topScores;
        }
    }
}
