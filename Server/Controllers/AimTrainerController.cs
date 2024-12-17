using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;
using Projektas.Shared.Enums;

namespace Projektas.Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AimTrainerController : Controller {
        private readonly AimTrainerService _aimTrainerService;

        public AimTrainerController (AimTrainerService aimTrainerService) {
            _aimTrainerService = aimTrainerService;
        }

        [HttpPost("save-score")]
        public async Task SaveScoreAsync([FromBody] UserScoreDto<AimTrainerData> data) {
            await _aimTrainerService.AddScoreToDbAsync(data);
        }

        [HttpGet("highscore")]
        public async Task<ActionResult<UserScoreDto<AimTrainerData>?>> GetUserHighscoreAsync([FromQuery] string username, [FromQuery] GameDifficulty difficulty) {
            return await _aimTrainerService.GetUserHighscoreAsync(username, difficulty);
        }

        [HttpGet("top-score")]
        public async Task<ActionResult<List<UserScoreDto<AimTrainerData>>>> GetTopScoresAsync([FromQuery] int topCount, [FromQuery] GameDifficulty difficulty) {
            return await _aimTrainerService.GetTopScoresAsync(topCount, difficulty);
        }
    }
}
