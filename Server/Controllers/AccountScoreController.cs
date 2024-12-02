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

        public AccountScoreController(AccountScoreService accountScoreService)
        {
            _accountScoreService = accountScoreService;
        }

        [HttpGet("math-game-scores")]
        public async Task<ActionResult<List<UserScoreDto>>> GetUsersMathGameScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetMathGameUserScores(user);
            return Ok(scores);
        }

        [HttpGet("aim-trainer-scores")]
        public async Task<ActionResult<List<UserScoreDto>>> GetUsersAimTrainerScores([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetAimTrainerUserScores(user);
            return Ok(scores);
        }

        [HttpGet("pair-up-scores")]
        public async Task<ActionResult<List<UserScoreDto>>> GetUsersPairUpScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var scores = await _accountScoreService.GetPairUpUserScores(user);
            return Ok(scores);
        }

        [HttpGet("math-game-highscore")]
        public async Task<ActionResult<int?>> GetMathGameHighscore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetMathGameHighscore(user);
            return Ok(highscore);
        }

        [HttpGet("aim-trainer-highscore-normal-mode")]
        public async Task<ActionResult<int?>> GetAimTrainerHighscoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetAimTrainerHighscoreNormalMode(user);
            return Ok(highscore);
        }
        [HttpGet("aim-trainer-highscore-hard-mode")]
        public async Task<ActionResult<int?>> GetAimTrainerHighscoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetAimTrainerHighscoreHardMode(user);
            return Ok(highscore);
        }

        [HttpGet("pair-up-highscore-normal-mode")]
        public async Task<ActionResult<int?>> GetPairUpHighscoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetPairUpHighscoreNormalMode(user);
            return Ok(highscore);
        }

        [HttpGet("pair-up-highscore-hard-mode")]
        public async Task<ActionResult<int?>> GetPairUpHighscoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetPairUpHighscoreHardMode(user);
            return Ok(highscore);
        }

        [HttpGet("math-game-matches-played")]
        public async Task<ActionResult<int>> GetMathGameMatchesPlayed([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetMathGameMatchesPlayed(user);
            return Ok(totalMatches);
        }
        [HttpGet("aim-trainer-matches-played")]
        public async Task<ActionResult<int>> GetTotalAimTrainerMatchesPlayed([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetTotalAimTrainerMatchesPlayed(user);
            return Ok(totalMatches);
        }
        [HttpGet("aim-trainer-matches-played-normal-mode")]
        public async Task<ActionResult<int>> GetAimTrainerMatchesPlayedNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetAimTrainerMatchesPlayedNormalMode(user);
            return Ok(totalMatches);
        }

        [HttpGet("aim-trainer-matches-played-hard-mode")]
        public async Task<ActionResult<int>> GetAimTrainerMatchesPlayedHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetAimTrainerMatchesPlayedHardMode(user);
            return Ok(totalMatches);
        }

        [HttpGet("pair-up-matches-played")]
        public async Task<ActionResult<int>> GetTotalPairUpMatchesPlayed([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetTotalPairUpMatchesPlayed(user);
            return Ok(totalMatches);
        }

        [HttpGet("pair-up-matches-played-normal-mode")]
        public async Task<ActionResult<int>> GetPairUpMatchesPlayedNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetPairUpMatchesPlayedNormalMode(user);
            return Ok(totalMatches);
        }

        [HttpGet("pair-up-matches-played-hard-mode")]
        public async Task<ActionResult<int>> GetPairUpMatchesPlayedHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var totalMatches = await _accountScoreService.GetPairUpMatchesPlayedHardMode(user);
            return Ok(totalMatches);
        }

        [HttpGet("math-game-average-score")]
        public async Task<ActionResult<int>> GetMathGameAllTimeAverageScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetMathGameAllTimeAverageScore(user);
            return Ok(averageScore);
        }

        [HttpGet("aim-trainer-average-score-normal-mode")]
        public async Task<ActionResult<int>> GetAimTrainerAllTimeAverageScoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAimTrainerAllTimeAverageScoreNormalMode(user);
            return Ok(averageScore);
        }
        [HttpGet("aim-trainer-average-score-hard-mode")]
        public async Task<ActionResult<int>> GetAimTrainerAllTimeAverageScoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAimTrainerAllTimeAverageScoreHardMode(user);
            return Ok(averageScore);
        }

        [HttpGet("pair-up-average-score-normal-mode")]
        public async Task<ActionResult<int>> GetPairUpAllTimeAverageScoreNormalMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetPairUpAllTimeAverageScoreNormalMode(user);
            return Ok(averageScore);
        }
        [HttpGet("pair-up-average-score-hard-mode")]
        public async Task<ActionResult<int>> GetPairUpAllTimeAverageScoreHardMode([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetPairUpAllTimeAverageScoreHardMode(user);
            return Ok(averageScore);
        }

        [HttpGet("math-game-average-score-last-7days")]
        public async Task<ActionResult<List<AverageScoreDto>>> GetMathGameAverageScoreLast7Days([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScoreLast7days = await _accountScoreService.GetMathGameAverageScoreLast7Days(user);
            return Ok(averageScoreLast7days);
        }
    }
}