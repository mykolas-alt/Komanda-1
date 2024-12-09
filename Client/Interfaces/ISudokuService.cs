using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces {
    public interface ISudokuService {
        Task<int[,]> GenerateSolvedSudokuAsync(int gridSize);
        Task<int[,]> HideNumbersAsync(int[,] grid, int gridSize, int difficulty);
        public Task SaveScoreAsync(string username, int score, GameDifficulty difficulty, GameMode gameMode);
        public Task<UserScoreDto<SudokuData>> GetUserHighscoreAsync(string username);
        public Task<List<UserScoreDto<SudokuData>>> GetTopScoresAsync(int topCount);
    }

}
