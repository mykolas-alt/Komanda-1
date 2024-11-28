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

        [HttpGet("aim-trainer-highscore")]
        public async Task<ActionResult<int?>> GetAimTrainerHighscore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetAimTrainerHighscore(user);
            return Ok(highscore);
        }

        [HttpGet("pair-up-highscore")]
        public async Task<ActionResult<int?>> GetPairUpHighscore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var highscore = await _accountScoreService.GetPairUpHighscore(user);
            return Ok(highscore);
        }

        [HttpGet("math-game-average-score")]
        public async Task<ActionResult<int>> GetMathGameAllTimeAverageScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetMathGameAllTimeAverageScore(user);
            return Ok(averageScore);
        }

        [HttpGet("aim-trainer-average-score")]
        public async Task<ActionResult<int>> GetAimTrainerAllTimeAverageScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetAimTrainerAllTimeAverageScore(user);
            return Ok(averageScore);
        }

        [HttpGet("pair-up-average-score")]
        public async Task<ActionResult<int>> GetPairUpAllTimeAverageScore([FromQuery] string username)
        {
            var user = new User { Username = username };
            var averageScore = await _accountScoreService.GetPairUpAllTimeAverageScore(user);
            return Ok(averageScore);
        }
    }
}