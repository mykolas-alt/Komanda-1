using Projektas.Shared.Interfaces;
using Projektas.Shared.Models;
using Projektas.Server.Interfaces;
using Projektas.Shared.Enums;

namespace Projektas.Server.Services
{
    public class AccountScoreService
    {
        const int lastGamesCount = 10;
        private readonly IScoreRepository repository;

        public AccountScoreService(IScoreRepository _repository)
        {
            repository = _repository;
        }

        private async Task<List<UserScoreDto<T>>> GetUserSpecificScores<T>(User user) where T : IGame
        {
            List<UserScoreDto<T>> userScores = await repository.GetAllScoresAsync<T>();
            return userScores.Where(score => score.Username == user.Username).ToList();
        }

        public async Task<List<UserScoreDto<T>>> GetUserScores<T>(User user) where T : IGame
        {
            var userSpecificScores = await GetUserSpecificScores<T>(user);
            return userSpecificScores.OrderByDescending(score => score.Timestamp)
                                     .Take(lastGamesCount)
                                     .ToList();
        }

        public async Task<int?> GetHighscore<T>(User user) where T : IGame
        {
            var userSpecificScores = await GetUserSpecificScores<T>(user);
            if (userSpecificScores.Count == 0)
            {
                return 0;
            }
            if (typeof(T) == typeof(MathGameData))
            {
                return userSpecificScores.Max(score => (int?)((MathGameData)(object)score.GameData).Scores);
            }
            return 0;
        }

        public async Task<GameScore> GetHighscore<T>(User user, GameDifficulty difficulty) where T : IGame, IGameWithDifficulty
        {
            var userSpecificScores = await GetUserSpecificScores<T>(user);
            var filteredScores = userSpecificScores.Where(score => score.GameData.Difficulty == difficulty).ToList();

            if (filteredScores.Count == 0)
            {
                return new GameScore { Scores = 0, TimeSpent = 0 };
            }

            if (typeof(T) == typeof(PairUpData))
            {
                int score = filteredScores.Min(score => ((PairUpData)(object)score.GameData).Fails);
                int? time = filteredScores.Min(score => ((PairUpData)(object)score.GameData).TimeInSeconds);
                return new GameScore { Scores = score, TimeSpent = time };
            }
            else if (typeof(T) == typeof(AimTrainerData))
            {
                int score = filteredScores.Max(score => ((AimTrainerData)(object)score.GameData).Scores);
                return new GameScore { Scores = score };
            }
            return new GameScore { Scores = 0, TimeSpent = 0 };
        }

        public async Task<int> GetAllTimeAverageScore<T>(User user) where T : IGame
        {
            var userSpecificScores = await GetUserSpecificScores<T>(user);
            if (userSpecificScores.Count == 0)
            {
                return 0;
            }
            if (typeof(T) == typeof(MathGameData))
            {
                return (int)userSpecificScores.Average(score => ((MathGameData)(object)score.GameData).Scores);
            }
            return 0;
        }

        public async Task<GameScore> GetAllTimeAverageScore<T>(User user, GameDifficulty difficulty) where T : IGame, IGameWithDifficulty
        {
            var userSpecificScores = await GetUserSpecificScores<T>(user);
            var filteredScores = userSpecificScores.Where(score => score.GameData.Difficulty == difficulty).ToList();

            if (filteredScores.Count == 0)
            {
                return new GameScore { Scores = 0, TimeSpent = 0 };
            }

            if (typeof(T) == typeof(PairUpData))
            {
                int averageScore = (int)filteredScores.Average(score => ((PairUpData)(object)score.GameData).Fails);
                int averageTime = (int)filteredScores.Average(score => ((PairUpData)(object)score.GameData).TimeInSeconds);
                return new GameScore { Scores = averageScore, TimeSpent = averageTime };
            }
            else if (typeof(T) == typeof(AimTrainerData))
            {
                int averageScore = (int)filteredScores.Average(score => ((AimTrainerData)(object)score.GameData).Scores);
                return new GameScore { Scores = averageScore };
            }
            return new GameScore { Scores = 0, TimeSpent = 0 };
        }

        public async Task<int> GetMatchesPlayed<T>(User user) where T : IGame
        {
            var userSpecificScores = await GetUserSpecificScores<T>(user);
            return userSpecificScores.Count;
        }

        public async Task<int> GetMatchesPlayed<T>(User user, GameDifficulty difficulty) where T : IGame, IGameWithDifficulty
        {
            var userSpecificScores = await GetUserSpecificScores<T>(user);
            return userSpecificScores.Count(score => score.GameData.Difficulty == difficulty);
        }

        public async Task<List<AverageScoreDto>> GetAverageScoreLast7Days<T>(User user) where T : IGame
        {
            var userSpecificScores = await GetUserSpecificScores<T>(user);
            DateTime today = DateTime.Today;
            var last7DaysScores = userSpecificScores.Where(score => score.Timestamp.Date >= today.AddDays(-6)).ToList();
            var groupedScores = last7DaysScores.GroupBy(score => score.Timestamp.Date).OrderBy(group => group.Key).ToList();

            var averageScores = new List<AverageScoreDto>();
            for (int i = 0; i < 7; i++)
            {
                DateTime date = today.AddDays(-i);
                var scoresForDate = groupedScores.FirstOrDefault(group => group.Key == date);

                if (scoresForDate != null)
                {
                    GameScore average = new GameScore();
                    if (typeof(T) == typeof(MathGameData))
                    {
                        int averageScore = (int)scoresForDate.Average(score => ((MathGameData)(object)score.GameData).Scores);
                        average = new GameScore { Scores = averageScore };
                    }
                    averageScores.Add(new AverageScoreDto { Score = average, Date = date });
                }
                else
                {
                    averageScores.Add(new AverageScoreDto { Score = new GameScore { Scores = 0, TimeSpent = 0 }, Date = date });
                }
            }

            averageScores.Reverse();
            return averageScores;
        }

        public async Task<List<AverageScoreDto>> GetAverageScoreLast7Days<T>(User user, GameDifficulty difficulty) where T : IGame, IGameWithDifficulty
        {
            var userSpecificScores = await GetUserSpecificScores<T>(user);
            DateTime today = DateTime.Today;
            var last7DaysScores = userSpecificScores.Where(score => score.GameData.Difficulty == difficulty && score.Timestamp.Date >= today.AddDays(-6)).ToList();
            var groupedScores = last7DaysScores.GroupBy(score => score.Timestamp.Date).OrderBy(group => group.Key).ToList();

            var averageScores = new List<AverageScoreDto>();
            for (int i = 0; i < 7; i++)
            {
                DateTime date = today.AddDays(-i);
                var scoresForDate = groupedScores.FirstOrDefault(group => group.Key == date);

                if (scoresForDate != null)
                {
                    GameScore average = new GameScore();
                    if (typeof(T) == typeof(PairUpData))
                    {
                        int averageScore = (int)scoresForDate.Average(score => ((PairUpData)(object)score.GameData).Fails);
                        int averageTime = (int)scoresForDate.Average(score => ((PairUpData)(object)score.GameData).TimeInSeconds);
                        average = new GameScore { Scores = averageScore, TimeSpent = averageTime };
                    }
                    else if (typeof(T) == typeof(AimTrainerData))
                    {
                        int averageScore = (int)scoresForDate.Average(score => ((AimTrainerData)(object)score.GameData).Scores);
                        average = new GameScore { Scores = averageScore };
                    }
                    averageScores.Add(new AverageScoreDto { Score = average, Date = date });

                }
                else
                {
                    averageScores.Add(new AverageScoreDto { Score = new GameScore { Scores = 0, TimeSpent = 0 }, Date = date });
                }
            }

            averageScores.Reverse();
            return averageScores;
        }
    }
}