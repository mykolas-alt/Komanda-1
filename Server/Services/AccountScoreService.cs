using Projektas.Shared.Interfaces;
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

        public async Task<int?> GetAimTrainerHighscore(User user)
        {
            int? highscore = await _aimTrainerScoreRepository.GetHighscoreFromUserAsync(user.Username);
            if (highscore != null)
            {
                return highscore;
            }
            else return 0;
        }

        public async Task<int?> GetPairUpHighscore(User user)
        {
            var userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            var userSpecificScores = userScores.Where(score => score.Username == user.Username).ToList();

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

        public async Task<int> GetAimTrainerAllTimeAverageScore(User user)
        {
            List<UserScoreDto> userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            int average = GetAllTimeAverageScore(userScores, user);

            return average;
        }
        public async Task<int> GetPairUpAllTimeAverageScore(User user)
        {
            List<UserScoreDto> userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            int average = GetAllTimeAverageScore(userScores, user);

            return average;
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
    }
}