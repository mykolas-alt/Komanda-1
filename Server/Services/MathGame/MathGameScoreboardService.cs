using Projektas.Shared.Models;

namespace Projektas.Server.Services.MathGame {
    public class MathGameScoreboardService : IComparer<int> {
        private readonly ScoreRepository<MathGameM> _scoreRepository;

        public MathGameScoreboardService(ScoreRepository<MathGameM> scoreRepository) {
            _scoreRepository = scoreRepository;
        }

        public async Task AddScoreToDb(UserScoreDto data) {
            await _scoreRepository.AddScoreToUserAsync(data.Username,data.Score);
        }

        public async Task<int?> GetUserHighscore(string username) {
            return await _scoreRepository.GetHighscoreFromUserAsync(username);
        }

        public async Task<List<UserScoreDto>> GetTopScores(int topCount) {
            List<UserScoreDto> userScores=await _scoreRepository.GetAllScoresAsync();
            List<UserScoreDto> topScores=new List<UserScoreDto>();
            
            for (int i=0;i<topCount && i<userScores.Count;i++) {
                topScores.Add(userScores[i]);
            }
            
            return topScores;
        }

        public int Compare(int a,int b) {
            return b.CompareTo(a);
        }
    }
}
