using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountScoreController : ControllerBase
    {
        private readonly AccountScoreService _accountScoreService;
        private readonly IScoreRepository<MathGameM> _mathGameRepository;
        private readonly IScoreRepository<AimTrainerM> _aimTrainerRepository;
        private readonly IScoreRepository<PairUpM> _pairUpRepository;

        public AccountScoreController(AccountScoreService accountScoreService,
                                      IScoreRepository<MathGameM> mathGameRepository,
                                      IScoreRepository<AimTrainerM> aimTrainerRepository,
                                      IScoreRepository<PairUpM> pairUpRepository)
        {
            _accountScoreService = accountScoreService;
            _mathGameRepository = mathGameRepository;
            _aimTrainerRepository = aimTrainerRepository;
            _pairUpRepository = pairUpRepository;
        }

        [HttpGet("math-game-scores")]
        public async Task<ActionResult<List<UserScoreDto>>> GetUsersMathGameScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores(user, _mathGameRepository);
            return Ok(scores);
        }

        [HttpGet("aim-trainer-scores")]
        public async Task<ActionResult<List<UserScoreDto>>> GetUsersAimTrainerScores([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores(user, _aimTrainerRepository);
            return Ok(scores);
        }

        [HttpGet("pair-up-scores")]
        public async Task<ActionResult<List<UserScoreDto>>> GetUsersPairUpScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores(user, _pairUpRepository);
            return Ok(scores);
        }

        [HttpGet("math-game-highscore")]
        public async Task<ActionResult<int?>> GetMathGameHighscore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore(user, _mathGameRepository);
            return Ok(highscore);
        }

        [HttpGet("aim-trainer-highscore-normal-mode")]
        public async Task<ActionResult<int?>> GetAimTrainerHighscoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore(user, _aimTrainerRepository, "Normal");
            return Ok(highscore);
        }

        [HttpGet("aim-trainer-highscore-hard-mode")]
        public async Task<ActionResult<int?>> GetAimTrainerHighscoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore(user, _aimTrainerRepository, "Hard");
            return Ok(highscore);
        }

        [HttpGet("pair-up-highscore-normal-mode")]
        public async Task<ActionResult<int?>> GetPairUpHighscoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore(user, _pairUpRepository, "Normal");
            return Ok(highscore);
        }

        [HttpGet("pair-up-highscore-hard-mode")]
        public async Task<ActionResult<int?>> GetPairUpHighscoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore(user, _pairUpRepository, "Hard");
            return Ok(highscore);
        }

        [HttpGet("math-game-matches-played")]
        public async Task<ActionResult<int>> GetMathGameMatchesPlayed([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed(user, _mathGameRepository);
            return Ok(totalMatches);
        }

        [HttpGet("aim-trainer-matches-played-normal-mode")]
        public async Task<ActionResult<int>> GetAimTrainerMatchesPlayedNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed(user, _aimTrainerRepository, "Normal");
            return Ok(totalMatches);
        }

        [HttpGet("aim-trainer-matches-played-hard-mode")]
        public async Task<ActionResult<int>> GetAimTrainerMatchesPlayedHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed(user, _aimTrainerRepository, "Hard");
            return Ok(totalMatches);
        }

        [HttpGet("pair-up-matches-played-normal-mode")]
        public async Task<ActionResult<int>> GetPairUpMatchesPlayedNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed(user, _pairUpRepository, "Normal");
            return Ok(totalMatches);
        }

        [HttpGet("pair-up-matches-played-hard-mode")]
        public async Task<ActionResult<int>> GetPairUpMatchesPlayedHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed(user, _pairUpRepository, "Hard");
            return Ok(totalMatches);
        }

        [HttpGet("math-game-average-score")]
        public async Task<ActionResult<int>> GetMathGameAllTimeAverageScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore(user, _mathGameRepository);
            return Ok(averageScore);
        }

        [HttpGet("aim-trainer-average-score-normal-mode")]
        public async Task<ActionResult<int>> GetAimTrainerAllTimeAverageScoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore(user, _aimTrainerRepository, "Normal");
            return Ok(averageScore);
        }

        [HttpGet("aim-trainer-average-score-hard-mode")]
        public async Task<ActionResult<int>> GetAimTrainerAllTimeAverageScoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore(user, _aimTrainerRepository, "Hard");
            return Ok(averageScore);
        }

        [HttpGet("pair-up-average-score-normal-mode")]
        public async Task<ActionResult<int>> GetPairUpAllTimeAverageScoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore(user, _pairUpRepository, "Normal");
            return Ok(averageScore);
        }

        [HttpGet("pair-up-average-score-hard-mode")]
        public async Task<ActionResult<int>> GetPairUpAllTimeAverageScoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore(user, _pairUpRepository, "Hard");
            return Ok(averageScore);
        }

        [HttpGet("math-game-average-score-last-7days")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetMathGameAverageScoreLast7Days([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days(user, _mathGameRepository);
            return Ok(averageScoreLast7days);
        }

        [HttpGet("aim-trainer-average-score-last-7days-normal-mode")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetAimTrainerAverageScoreLast7DaysNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days(user, _aimTrainerRepository, "Normal");
            return Ok(averageScoreLast7days);
        }

        [HttpGet("aim-trainer-average-score-last-7days-hard-mode")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetAimTrainerAverageScoreLast7DaysHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days(user, _aimTrainerRepository, "Hard");
            return Ok(averageScoreLast7days);
        }

        [HttpGet("pair-up-average-score-last-7days-normal-mode")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetPairUpAverageScoreLast7DaysNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days(user, _pairUpRepository, "Normal");
            return Ok(averageScoreLast7days);
        }

        [HttpGet("pair-up-average-score-last-7days-hard-mode")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetPairUpAverageScoreLast7DaysHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days(user, _pairUpRepository, "Hard");
            return Ok(averageScoreLast7days);
        }
    }
}