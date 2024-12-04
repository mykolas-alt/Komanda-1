using Projektas.Shared.Models;
using Projektas.Server.Interfaces.MathGame;

namespace Projektas.Server.Services.MathGame {
    public class MathGameScoreboardService : IComparer<int>, IMathGameScoreboardService {
        private readonly IScoreRepository _scoreRepository;

        public MathGameScoreboardService(IScoreRepository scoreRepository) {
            _scoreRepository = scoreRepository;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<MathGameData> data) {
            await _scoreRepository.AddScoreToUserAsync<MathGameData>(data.Username,data.GameData,data.GameData.Scores);
        }

        public async Task<UserScoreDto<MathGameData>?> GetUserHighscoreAsync(string username) {
            return await _scoreRepository.GetHighscoreFromUserAsync<MathGameData>(username);
        }

        public async Task<List<UserScoreDto<MathGameData>>> GetTopScoresAsync(int topCount) {
            List<UserScoreDto<MathGameData>> userScores=await _scoreRepository.GetAllScoresAsync<MathGameData>();
            List<UserScoreDto<MathGameData>> topScores=new List<UserScoreDto<MathGameData>>();
            
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