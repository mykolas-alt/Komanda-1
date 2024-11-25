using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AimTrainerController : Controller {
        private readonly AimTrainerService _aimTrainerService;

        public AimTrainerController (AimTrainerService aimTrainerService) {
            _aimTrainerService=aimTrainerService;
        }

        [HttpPost("save-score")]
        public async Task SaveScore([FromBody] UserScoreDto data) {
            await _aimTrainerService.AddScoreToDb(data);
        }

        [HttpGet("highscore")]
        public async Task<ActionResult<int>> GetUserHighscore([FromQuery] string username) {
            return await _aimTrainerService.GetUserHighscore(username);
        }

        [HttpGet("top-score")]
        public async Task<ActionResult<List<UserScoreDto>>> GetTopScores([FromQuery] int topCount) {
            return await _aimTrainerService.GetTopScores(topCount);
        }
    }
}
