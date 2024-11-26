using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces {
    public interface ISudokuService {
        public Task SaveScoreAsync(string username,int score);
        public Task<int> GetUserHighscore(string username);
        public Task<List<UserScoreDto>> GetTopScoresAsync(int topCount);
        Task<int[,]> GenerateSolvedSudokuAsync(int gridSize);
        Task<int[,]> HideNumbersAsync(int[,] grid,int gridSize,int difficulty);
    }

}
