using Projektas.Shared.Models;

namespace Projektas.Server.Services
{
    public class AccountScoreService
    {
        private readonly IScoreRepository<MathGameM> _mathGameScoreRepository;
        private readonly IScoreRepository<AimTrainerM> _aimTrainerScoreRepository;
        private readonly IScoreRepository<PairUpM> _pairUpScoreRepository;
        const int lastGamesCount = 10;

        public AccountScoreService(
            IScoreRepository<MathGameM> mathGameScoreRepository,
            IScoreRepository<AimTrainerM> aimTrainerScoreRepository,
            IScoreRepository<PairUpM> pairUpScoreRepository)
        {
            _mathGameScoreRepository = mathGameScoreRepository;
            _aimTrainerScoreRepository = aimTrainerScoreRepository;
            _pairUpScoreRepository = pairUpScoreRepository;
        }

        public async Task<List<UserScoreDto>> GetMathGameUserScores(User user) 
        {
            List<UserScoreDto> userScores = await _mathGameScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> mathGameScores = GetUserScores(userScores, user);
            return mathGameScores;
        }

        public async Task<List<UserScoreDto>> GetAimTrainerUserScores(User user)
        {
            List<UserScoreDto> userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> aimTrainerScores = GetUserScores(userScores, user);
            return aimTrainerScores;
        }

        public async Task<List<UserScoreDto>> GetPairUpUserScores(User user)
        {
            List<UserScoreDto> userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> pairUpScores = GetUserScores(userScores, user);
            return pairUpScores;
        }

        public async Task<int?> GetMathGameHighscore(User user)
        {
            int? highscore = await _mathGameScoreRepository.GetHighscoreFromUserAsync(user.Username);
            if (highscore != null)
            {
                return highscore;
            }
            else return 0;
        }

        public async Task<int?> GetAimTrainerHighscoreNormalMode(User user)
        {
            var userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            var userSpecificScores = userScores.Where(score => score.Username == user.Username &&
                                                      score.Difficulty == "Normal").ToList();

            if (userSpecificScores.Count == 0)
            {
                return 0;
            }

            return userSpecificScores.Max(score => (int?)score.Score);
        }

        public async Task<int?> GetAimTrainerHighscoreHardMode(User user)
        {
            var userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            var userSpecificScores = userScores.Where(score => score.Username == user.Username &&
                                                      score.Difficulty == "Hard").ToList();

            if (userSpecificScores.Count == 0)
            {
                return 0;
            }

            return userSpecificScores.Max(score => (int?)score.Score);
        }

        public async Task<int?> GetPairUpHighscoreNormalMode(User user)
        {
            var userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            var userSpecificScores = userScores.Where(score => score.Username == user.Username &&
                                                      score.Difficulty == "Normal").ToList();

            if (userSpecificScores.Count == 0)
            {
                return 0;
            }

            return userSpecificScores.Min(score => (int?)score.Score);
        }

        public async Task<int?> GetPairUpHighscoreHardMode(User user)
        {
            var userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            var userSpecificScores = userScores.Where(score => score.Username == user.Username &&
                                                score.Difficulty == "Hard").ToList();

            if (userSpecificScores.Count == 0)
            {
                return 0;
            }

            return userSpecificScores.Min(score => (int?)score.Score);
        }

        public async Task<int> GetMathGameAllTimeAverageScore(User user)
        {
            List<UserScoreDto> userScores = await _mathGameScoreRepository.GetAllScoresAsync();
            int average = GetAllTimeAverageScore(userScores, user);

            return average;
        }

        public async Task<int> GetAimTrainerAllTimeAverageScoreNormalMode(User user)
        {
            List<UserScoreDto> userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            int average = GetAllTimeAverageScoreNormalMode(userScores, user);

            return average;
        }
        public async Task<int> GetAimTrainerAllTimeAverageScoreHardMode(User user)
        {
            List<UserScoreDto> userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            int average = GetAllTimeAverageScoreHardMode(userScores, user);

            return average;
        }

        public async Task<int> GetPairUpAllTimeAverageScoreNormalMode(User user)
        {
            List<UserScoreDto> userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            int average = GetAllTimeAverageScoreNormalMode(userScores, user);

            return average;
        }
        
        public async Task<int> GetPairUpAllTimeAverageScoreHardMode(User user)
        {
            List<UserScoreDto> userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            int average = GetAllTimeAverageScoreHardMode(userScores, user);

            return average;
        }


        public async Task<int> GetMathGameMatchesPlayed(User user)
        {
            List<UserScoreDto> userScores = await _mathGameScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> scores = userScores.Where(score => score.Username == user.Username)
                                                  .ToList();
            if (scores != null)
            {
                return scores.Count();
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> GetTotalAimTrainerMatchesPlayed(User user)
        {
            List<UserScoreDto> userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> scores = userScores.Where(score => score.Username == user.Username)
                                                  .ToList();
            if (scores != null)
            {
                return scores.Count();
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> GetAimTrainerMatchesPlayedNormalMode(User user)
        {
            List<UserScoreDto> userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> scores = userScores.Where(score => score.Username == user.Username &&
                                                         score.Difficulty == "Normal")
                                                  .ToList();
            if (scores != null)
            {
                return scores.Count();
            }
            else
            {
                return 0;
            }
        }
        public async Task<int> GetAimTrainerMatchesPlayedHardMode(User user)
        {
            List<UserScoreDto> userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> scores = userScores.Where(score => score.Username == user.Username &&
                                                         score.Difficulty == "Hard")
                                                  .ToList();
            if (scores != null)
            {
                return scores.Count();
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> GetTotalPairUpMatchesPlayed(User user)
        {
            List<UserScoreDto> userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> scores = userScores.Where(score => score.Username == user.Username)
                                                  .ToList();
            if (scores != null)
            {
                return scores.Count();
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> GetPairUpMatchesPlayedNormalMode(User user)
        {
            List<UserScoreDto> userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> scores = userScores.Where(score => score.Username == user.Username &&
                                                         score.Difficulty == "Normal")
                                                  .ToList();
            if (scores != null)
            {
                return scores.Count();
            }
            else
            {
                return 0;
            }
        }
        public async Task<int> GetPairUpMatchesPlayedHardMode(User user)
        {
            List<UserScoreDto> userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> scores = userScores.Where(score => score.Username == user.Username &&
                                                         score.Difficulty == "Hard")
                                                  .ToList();
            if (scores != null)
            {
                return scores.Count();
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<AverageScoreDto>> GetMathGameAverageScoreLast7Days(User user)
        {
            List<UserScoreDto> userScores = await _mathGameScoreRepository.GetAllScoresAsync();
            List<AverageScoreDto> averageLast7Days = GetAverageScoresLast7Days(userScores, user);
            return averageLast7Days;
        }

        private List<UserScoreDto> GetUserScores(List<UserScoreDto> userScores, User user)
        {
            List<UserScoreDto> scores = userScores.Where(score => score.Username == user.Username)
                                                          .OrderByDescending(score => score.Timestamp)
                                                          .Take(lastGamesCount)
                                                          .ToList();
            return scores;
        }
        private int GetAllTimeAverageScore(List<UserScoreDto> userScores, User user)
        {
            var userSpecificScores = userScores.Where(score => score.Username == user.Username).ToList();
            if (userSpecificScores.Count == 0)
            {
                return 0;
            }

            int average = (int)userSpecificScores.Average(score => score.Score);
            return average;
        }
        private int GetAllTimeAverageScoreNormalMode(List<UserScoreDto> userScores, User user)
        {
            var userSpecificScores = userScores.Where(score => score.Username == user.Username 
                                                      && score.Difficulty == "Normal")
                                               .ToList();
            if (userSpecificScores.Count == 0)
            {
                return 0;
            }

            int average = (int)userSpecificScores.Average(score => score.Score);
            return average;
        }
        private int GetAllTimeAverageScoreHardMode(List<UserScoreDto> userScores, User user)
        {
            var userSpecificScores = userScores.Where(score => score.Username == user.Username
                                                      && score.Difficulty == "Hard")
                                               .ToList();
            if (userSpecificScores.Count == 0)
            {
                return 0;
            }

            int average = (int)userSpecificScores.Average(score => score.Score);
            return average;
        }

        private List<AverageScoreDto> GetAverageScoresLast7Days(List<UserScoreDto> userScores, User user)
        {
            DateTime today = DateTime.Today;

            var last7DaysScores = userScores
                .Where(score => score.Username == user.Username && score.Timestamp.Date >= today.AddDays(-6))
                .ToList();

            var groupedScores = last7DaysScores
                .GroupBy(score => score.Timestamp.Date)
                .OrderBy(group => group.Key)
                .ToList();

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
            return averageScores;
        }

    }
}