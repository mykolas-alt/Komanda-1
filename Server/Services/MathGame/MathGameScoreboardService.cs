using Projektas.Shared.Models;

namespace Projektas.Server.Services.MathGame
{
    public class MathGameScoreboardService : IComparer<int>
    {
        private readonly MathGameDataService _dataService;
        private readonly UserRepository _userRepository;

        public MathGameScoreboardService(MathGameDataService dataService, UserRepository userRepository)
        {
            _dataService = dataService;
            _userRepository = userRepository;
        }

        public async Task<List<UserScoreDto>> GetTopScores(int topCount)
        {
            List<UserScoreDto> userScores = await _userRepository.GetAllScoresAsync();

            List<UserScoreDto> topScores = new List<UserScoreDto>();
            
            for (int i = 0; i < topCount && i < userScores.Count; i++)
            {
                topScores.Add(userScores[i]);
            }
            
            return topScores;
        }

        public int Compare(int a, int b)
        {
            return b.CompareTo(a);
        }
    }
}
