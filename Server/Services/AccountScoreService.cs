using Projektas.Shared.Interfaces;
using Projektas.Shared.Models;
using Projektas.Server.Interfaces;

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
        public async Task<List<UserScoreDto<T>>> GetUserScores<T>(User user) where T : IGame
        {
            List<UserScoreDto<T>> userScores = await repository.GetAllScoresAsync<T>();
            return userScores.Where(score => score.Username == user.Username)
                             //.OrderByDescending(score => score.Timestamp)
                             .Take(lastGamesCount)
                             .ToList();
        }

        public async Task<int?> GetHighscore<T>(User user, string? difficulty = null) where T : IGame
        {
            List<UserScoreDto<T>> userScores = await repository.GetAllScoresAsync<T>();
            var userSpecificScores = userScores.Where(score => score.Username == user.Username) // &&
                                                               //(difficulty == null || score.Difficulty == difficulty))
                                               .ToList();

            if (userSpecificScores.Count == 0)
            {
                return 0;
            }

            /*if (typeof(T) == typeof(PairUpData))
            {
                return userSpecificScores.Min(score => (int?)score.Score);
            }
            else
            {
                return userSpecificScores.Max(score => (int?)score.Score);
            }*/
            return 0;
        }

        public async Task<int> GetAllTimeAverageScore<T>(User user, string? difficulty = null) where T : IGame
        {
            List<UserScoreDto<T>> userScores = await repository.GetAllScoresAsync<T>();
            var userSpecificScores = userScores.Where(score => score.Username == user.Username) //&&
                                                               //(difficulty == null || score.Difficulty == difficulty))
                                               .ToList();

            if (userSpecificScores.Count == 0)
            {
                return 0;
            }

            //return (int)userSpecificScores.Average(score => score.Score);

            return 0;
        }

        public async Task<int> GetMatchesPlayed<T>(User user, string? difficulty = null) where T : IGame
        {
            List<UserScoreDto<T>> userScores = await repository.GetAllScoresAsync<T>();
            var scores = userScores.Where(score => score.Username == user.Username) // &&
                                                   //(difficulty == null || score.Difficulty == difficulty))
                                   .ToList();

            return scores.Count;
        }

        public async Task<List<AverageScoreDto>> GetAverageScoreLast7Days<T>(User user, string? difficulty = null) where T : IGame
        {
            List<UserScoreDto<T>> userScores = await repository.GetAllScoresAsync<T>();
            var filteredScores = userScores.Where(score => score.Username == user.Username) // &&
                                                           //(difficulty == null || score.Difficulty == difficulty))
                                           .ToList();

            /*DateTime today = DateTime.Today;
            var last7DaysScores = filteredScores.Where(score => score.Timestamp.Date >= today.AddDays(-6)).ToList();
            var groupedScores = last7DaysScores.GroupBy(score => score.Timestamp.Date).OrderBy(group => group.Key).ToList();

            var averageScores = new List<AverageScoreDto>();
            for (int i = 0; i < 7; i++)
            {
                DateTime date = today.AddDays(-i);
                var scoresForDate = groupedScores.FirstOrDefault(group => group.Key == date);

                if (scoresForDate != null)
                {
                    int averageScore = (int)scoresForDate.Average(score => score.Score);
                    averageScores.Add(new AverageScoreDto { AverageScore = averageScore, Date = date });
                }
                else
                {
                    averageScores.Add(new AverageScoreDto { AverageScore = 0, Date = date });
                }
            }

            averageScores.Reverse();
            return averageScores;*/

            return new List<AverageScoreDto>();
        }

    }
}