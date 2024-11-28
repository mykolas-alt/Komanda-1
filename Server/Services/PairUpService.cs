using Projektas.Shared.Models;

namespace Projektas.Server.Services {
    public class PairUpService {
        private readonly IScoreRepository<PairUpM> _scoreRepository;

        public PairUpService (IScoreRepository<PairUpM> scoreRepository) {
            _scoreRepository=scoreRepository;
        }

        public async Task AddScoreToDb(UserScoreDto data) {
            await _scoreRepository.AddScoreToUserAsync(data.Username,data.Score);
        }

        public async Task<int?> GetUserHighscore(string username) {
            return await _scoreRepository.GetHighscoreFromUserAsync(username);
        }

        public async Task<List<UserScoreDto>> GetTopScores(int topCount)
        {
            List<UserScoreDto> userScores = await _scoreRepository.GetAllScoresAsync();
            List<UserScoreDto> topScores = userScores
            .OrderByDescending(score => score.Score)
            .Take(topCount)
            .ToList();

            return topScores;
        }
    }
}
