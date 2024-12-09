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
        Task<GameScore> GetMathGameHighscoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<GameScore> GetAimTrainerHighscoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<GameScore> GetPairUpHighscoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<GameScore> GetSudokuHighscoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<GameScore> GetMathGameAverageScoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<GameScore> GetAimTrainerAverageScoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<GameScore> GetPairUpAverageScoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<GameScore> GetSudokuAverageScoreAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<int> GetMathGameMatchesPlayedAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<int> GetAimTrainerMatchesPlayedAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<int> GetPairUpMatchesPlayedAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<int> GetSudokuMatchesPlayedAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<List<AverageScoreDto>> GetMathGameAverageScoreLast7DaysAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<List<AverageScoreDto>> GetAimTrainerAverageScoreLast7DaysAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<List<AverageScoreDto>> GetPairUpAverageScoreLast7DaysAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
        Task<List<AverageScoreDto>> GetSudokuAverageScoreLast7DaysAsync(string username, GameDifficulty? difficulty = null, GameMode? mode = null);
    }
}