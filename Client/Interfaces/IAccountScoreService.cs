using Projektas.Shared.Enums;
using Projektas.Shared.Models;

namespace Projektas.Client.Interfaces
{
    public interface IAccountScoreService
    {
        Task<List<UserScoreDto<MathGameData>>> GetMathGameScoresAsync(string username);
        Task<List<UserScoreDto<AimTrainerData>>> GetAimTrainerScoresAsync(string username);
        Task<List<UserScoreDto<PairUpData>>> GetPairUpScoresAsync(string username);
        Task<List<UserScoreDto<SudokuData>>> GetSudokuScoresAsync(string username);
        Task<GameScore> GetMathGameHighscoreAsync(string username);
        Task<GameScore> GetAimTrainerHighscoreAsync(string username, GameDifficulty difficulty);
        Task<GameScore> GetPairUpHighscoreAsync(string username, GameDifficulty difficulty);
        Task<GameScore> GetSudokuHighscoreAsync(string username, GameDifficulty difficulty, GameMode mode);
        Task<GameScore> GetMathGameAverageScoreAsync(string username);
        Task<GameScore> GetAimTrainerAverageScoreAsync(string username, GameDifficulty difficulty);
        Task<GameScore> GetPairUpAverageScoreAsync(string username, GameDifficulty difficulty);
        Task<GameScore> GetSudokuAverageScoreAsync(string username, GameDifficulty difficulty, GameMode mode);
        Task<int> GetMathGameMatchesPlayedAsync(string username);
        Task<int> GetAimTrainerMatchesPlayedAsync(string username, GameDifficulty difficulty);
        Task<int> GetPairUpMatchesPlayedAsync(string username, GameDifficulty difficulty);
        Task<int> GetSudokuMatchesPlayedAsync(string username, GameDifficulty difficulty, GameMode mode);
        Task<List<AverageScoreDto>> GetMathGameAverageScoreLast7DaysAsync(string username);
        Task<List<AverageScoreDto>> GetAimTrainerAverageScoreLast7DaysAsync(string username, GameDifficulty difficulty);
        Task<List<AverageScoreDto>> GetPairUpAverageScoreLast7DaysAsync(string username, GameDifficulty difficulty);
        Task<List<AverageScoreDto>> GetSudokuAverageScoreLast7DaysAsync(string username, GameDifficulty difficulty, GameMode mode);
    }
}