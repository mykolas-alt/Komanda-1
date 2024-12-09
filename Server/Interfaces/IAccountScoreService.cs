using Projektas.Shared.Models;
using Projektas.Shared.Enums;
using Projektas.Shared.Interfaces;

namespace Projektas.Server.Interfaces
{
    public interface IAccountScoreService
    {
        Task<List<UserScoreDto<T>>> GetUserScores<T>(User user) where T : IGame;
        Task<GameScore> GetHighscore<T>(User user) where T : IGame;
        Task<GameScore> GetHighscore<T>(User user, GameDifficulty difficulty) where T : IGame, IGameWithDifficulty;
        Task<GameScore> GetHighscore<T>(User user, GameDifficulty difficulty, GameMode mode) where T : IGame, IGameWithDifficulty, IGameWithModes;
        Task<GameScore> GetAllTimeAverageScore<T>(User user) where T : IGame;
        Task<GameScore> GetAllTimeAverageScore<T>(User user, GameDifficulty difficulty) where T : IGame, IGameWithDifficulty;
        Task<GameScore> GetAllTimeAverageScore<T>(User user, GameDifficulty difficulty, GameMode mode) where T : IGame, IGameWithDifficulty, IGameWithModes;
        Task<int> GetMatchesPlayed<T>(User user) where T : IGame;
        Task<int> GetMatchesPlayed<T>(User user, GameDifficulty difficulty) where T : IGame, IGameWithDifficulty;
        Task<int> GetMatchesPlayed<T>(User user, GameDifficulty difficulty, GameMode mode) where T : IGame, IGameWithDifficulty, IGameWithModes;
        Task<List<AverageScoreDto>> GetAverageScoreLast7Days<T>(User user) where T : IGame;
        Task<List<AverageScoreDto>> GetAverageScoreLast7Days<T>(User user, GameDifficulty difficulty) where T : IGame, IGameWithDifficulty;
        Task<List<AverageScoreDto>> GetAverageScoreLast7Days<T>(User user, GameDifficulty difficulty, GameMode mode) where T : IGame, IGameWithDifficulty, IGameWithModes;
    }
}