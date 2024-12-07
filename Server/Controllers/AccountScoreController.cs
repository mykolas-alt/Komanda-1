using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;
using Projektas.Server.Interfaces;

namespace Projektas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountScoreController : ControllerBase
    {
        private readonly AccountScoreService _accountScoreService;

        public AccountScoreController(AccountScoreService accountScoreService)
        {
            _accountScoreService = accountScoreService;
        }

        [HttpGet("math-game-scores")]
        public async Task<ActionResult<List<UserScoreDto<MathGameData>>>> GetUsersMathGameScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores<MathGameData>(user);
            return Ok(scores);
        }

        [HttpGet("aim-trainer-scores")]
        public async Task<ActionResult<List<UserScoreDto<AimTrainerData>>>> GetUsersAimTrainerScores([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores<AimTrainerData>(user);
            return Ok(scores);
        }

        [HttpGet("pair-up-scores")]
        public async Task<ActionResult<List<UserScoreDto<PairUpData>>>> GetUsersPairUpScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetUserScores<PairUpData>(user);
            return Ok(scores);
        }

        [HttpGet("math-game-highscore")]
        public async Task<ActionResult<int?>> GetMathGameHighscore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore<MathGameData>(user);
            return Ok(highscore);
        }

        [HttpGet("aim-trainer-highscore-normal-mode")]
        public async Task<ActionResult<int?>> GetAimTrainerHighscoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore<AimTrainerData>(user, "Normal");
            return Ok(highscore);
        }

        [HttpGet("aim-trainer-highscore-hard-mode")]
        public async Task<ActionResult<int?>> GetAimTrainerHighscoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore<AimTrainerData>(user, "Hard");
            return Ok(highscore);
        }

        [HttpGet("pair-up-highscore-normal-mode")]
        public async Task<ActionResult<int?>> GetPairUpHighscoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore<PairUpData>(user, "Normal");
            return Ok(highscore);
        }

        [HttpGet("pair-up-highscore-hard-mode")]
        public async Task<ActionResult<int?>> GetPairUpHighscoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetHighscore<PairUpData>(user, "Hard");
            return Ok(highscore);
        }

        [HttpGet("math-game-matches-played")]
        public async Task<ActionResult<int>> GetMathGameMatchesPlayed([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed<MathGameData>(user);
            return Ok(totalMatches);
        }

        [HttpGet("aim-trainer-matches-played-normal-mode")]
        public async Task<ActionResult<int>> GetAimTrainerMatchesPlayedNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed<AimTrainerData>(user, "Normal");
            return Ok(totalMatches);
        }

        [HttpGet("aim-trainer-matches-played-hard-mode")]
        public async Task<ActionResult<int>> GetAimTrainerMatchesPlayedHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed<AimTrainerData>(user, "Hard");
            return Ok(totalMatches);
        }

        [HttpGet("pair-up-matches-played-normal-mode")]
        public async Task<ActionResult<int>> GetPairUpMatchesPlayedNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed<PairUpData>(user, "Normal");
            return Ok(totalMatches);
        }

        [HttpGet("pair-up-matches-played-hard-mode")]
        public async Task<ActionResult<int>> GetPairUpMatchesPlayedHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMatchesPlayed<PairUpData>(user, "Hard");
            return Ok(totalMatches);
        }

        [HttpGet("math-game-average-score")]
        public async Task<ActionResult<int>> GetMathGameAllTimeAverageScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore<MathGameData>(user);
            return Ok(averageScore);
        }

        [HttpGet("aim-trainer-average-score-normal-mode")]
        public async Task<ActionResult<int>> GetAimTrainerAllTimeAverageScoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore<AimTrainerData>(user, "Normal");
            return Ok(averageScore);
        }

        [HttpGet("aim-trainer-average-score-hard-mode")]
        public async Task<ActionResult<int>> GetAimTrainerAllTimeAverageScoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore<AimTrainerData>(user, "Hard");
            return Ok(averageScore);
        }

        [HttpGet("pair-up-average-score-normal-mode")]
        public async Task<ActionResult<int>> GetPairUpAllTimeAverageScoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore<PairUpData>(user, "Normal");
            return Ok(averageScore);
        }

        [HttpGet("pair-up-average-score-hard-mode")]
        public async Task<ActionResult<int>> GetPairUpAllTimeAverageScoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAllTimeAverageScore<PairUpData>(user, "Hard");
            return Ok(averageScore);
        }

        [HttpGet("math-game-average-score-last-7days")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetMathGameAverageScoreLast7Days([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days<MathGameData>(user);
            return Ok(averageScoreLast7days);
        }

        [HttpGet("aim-trainer-average-score-last-7days-normal-mode")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetAimTrainerAverageScoreLast7DaysNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days<AimTrainerData>(user, "Normal");
            return Ok(averageScoreLast7days);
        }

        [HttpGet("aim-trainer-average-score-last-7days-hard-mode")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetAimTrainerAverageScoreLast7DaysHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days<AimTrainerData>(user, "Hard");
            return Ok(averageScoreLast7days);
        }

        [HttpGet("pair-up-average-score-last-7days-normal-mode")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetPairUpAverageScoreLast7DaysNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days<PairUpData>(user, "Normal");
            return Ok(averageScoreLast7days);
        }

        [HttpGet("pair-up-average-score-last-7days-hard-mode")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetPairUpAverageScoreLast7DaysHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetAverageScoreLast7Days<PairUpData>(user, "Hard");
            return Ok(averageScoreLast7days);
        }
    }
}