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
    }
}
