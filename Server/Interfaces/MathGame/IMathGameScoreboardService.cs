using Projektas.Shared.Models;

namespace Projektas.Server.Interfaces.MathGame
{
    public interface IMathGameScoreboardService
    {
        public Task<List<UserScoreDto>> GetTopScores(int topCount);
        public Task AddScoreToDb(UserScoreDto data);
        public Task<int?> GetUserHighscore(string username);

	}
}
