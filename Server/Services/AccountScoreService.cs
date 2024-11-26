using Projektas.Shared.Models;

namespace Projektas.Server.Services
{
    public class AccountScoreService
    {
        private readonly IScoreRepository<MathGameM> _mathGameScoreRepository;
        private readonly IScoreRepository<AimTrainerM> _aimTrainerScoreRepository;
        private readonly IScoreRepository<PairUpM> _pairUpScoreRepository;

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
            List<UserScoreDto> mathGameScores = new List<UserScoreDto>();

            foreach (var score in userScores)
            {
                if (user.Username == score.Username)
                {
                    mathGameScores.Add(score);
                }
            }
            return mathGameScores;
        }

        public async Task<List<UserScoreDto>> GetAimTrainerUserScores(User user)
        {
            List<UserScoreDto> userScores = await _aimTrainerScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> aimTrainerScores = new List<UserScoreDto>();

            foreach (var score in userScores)
            {
                if (user.Username == score.Username)
                {
                    aimTrainerScores.Add(score);
                }
            }
            return aimTrainerScores;
        }

        public async Task<List<UserScoreDto>> GetPairUpUserScores(User user)
        {
            List<UserScoreDto> userScores = await _pairUpScoreRepository.GetAllScoresAsync();
            List<UserScoreDto> pairUpScores = new List<UserScoreDto>();

            foreach (var score in userScores)
            {
                if (user.Username == score.Username)
                {
                    pairUpScores.Add(score);
                }
            }
            return pairUpScores;
        }
    }
}