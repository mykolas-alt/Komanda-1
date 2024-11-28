using Projektas.Shared.Models;
using Projektas.Server.Interfaces.MathGame;

namespace Projektas.Server.Services.MathGame {
    public class MathGameScoreboardService : IComparer<int>, IMathGameScoreboardService {
        private readonly IScoreRepository _scoreRepository;

        public MathGameScoreboardService(IScoreRepository scoreRepository) {
            _scoreRepository = scoreRepository;
        }

        public async Task AddScoreToDb(UserScoreDto data) {
            await _scoreRepository.AddScoreToUserAsync<MathGameModel>(data.Username,new MathGameModel(),data.Data);
        }

        public async Task<int?> GetUserHighscore(string username) {
            return await _scoreRepository.GetHighscoreFromUserAsync<MathGameModel>(username);
        }

        public async Task<List<UserScoreDto>> GetTopScores(int topCount) {
            List<UserScoreDto> userScores=await _scoreRepository.GetAllScoresAsync<MathGameModel>();
            List<UserScoreDto> topScores=new List<UserScoreDto>();
            
            for(int i=0;i<topCount && i<userScores.Count;i++) {
                topScores.Add(userScores[i]);
            }
            
            return topScores;
        }

        public int Compare(int a,int b) {
            return b.CompareTo(a);
        }
    }
}
