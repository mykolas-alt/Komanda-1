using Projektas.Shared.Models;
using Projektas.Shared.Enums;
using Projektas.Server.Interfaces;
using Projektas.Server.Interfaces.MathGame;

namespace Projektas.Server.Services.MathGame
{
    public class MathGameScoreboardService : IComparer<int>, IMathGameScoreboardService
    {
        private readonly IScoreRepository _scoreRepository;

        public MathGameScoreboardService(IScoreRepository scoreRepository)
        {
            _scoreRepository = scoreRepository;
        }

        public async Task AddScoreToDbAsync(UserScoreDto<MathGameData> data)
        {
            await _scoreRepository.AddScoreToUserAsync<MathGameData>(data.Username, data.GameData, data.Timestamp);
        }

        // Retrieves the user's highest, singular score for a specific game difficulty, returns the score or 'null' if not found
        public async Task<UserScoreDto<MathGameData>> GetUserHighscoreAsync(string username, GameDifficulty difficulty)
        {
            var scores = await _scoreRepository.GetHighscoreFromUserAsync<MathGameData>(username);

            // Filter scores based on difficulty and orders it by score
            var filteredScores = scores
                .Where(s => s.GameData.Difficulty == difficulty)
                .OrderByDescending(s => s.GameData.Scores)
                .ToList();

            return filteredScores.FirstOrDefault(); // Returns the first (best) score of the user or 'null' if no score found
        }

        // Retrieves the top scores for a specific game difficulty, returns a list of scores
        public async Task<List<UserScoreDto<MathGameData>>> GetTopScoresAsync(int topCount, GameDifficulty difficulty)
        {
            // Get all scores from the repository
            List<UserScoreDto<MathGameData>> userScores = await _scoreRepository.GetAllScoresAsync<MathGameData>();

            // Filter scores based on difficulty and orders it by score
            List<UserScoreDto<MathGameData>> orderedScores = userScores
                .Where(s => !s.IsPrivate && s.GameData.Difficulty == difficulty)
                .OrderByDescending(s => s.GameData.Scores)
                .ToList();

            // Filter the top scores based on the specified count (typically 10)
            List<UserScoreDto<MathGameData>> topScores = new List<UserScoreDto<MathGameData>>();

            for (int i = 0; i < topCount && i < orderedScores.Count; i++)
            {
                topScores.Add(orderedScores[i]);
            }

            return topScores;
        }

        public int Compare(int a, int b)
        {
            return b.CompareTo(a);
        }
    }
}