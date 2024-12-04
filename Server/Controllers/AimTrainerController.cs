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
        public async Task SaveScoreAsync([FromBody] UserScoreDto<AimTrainerData> data) {
            await _aimTrainerService.AddScoreToDbAsync(data);
        }

        [HttpGet("highscore")]
        public async Task<ActionResult<UserScoreDto<AimTrainerData>?>> GetUserHighscoreAsync([FromQuery] string username) {
            return await _aimTrainerService.GetUserHighscoreAsync(username);
        }

        [HttpGet("top-score")]
        public async Task<ActionResult<List<UserScoreDto<AimTrainerData>>>> GetTopScoresAsync([FromQuery] int topCount) {
            return await _aimTrainerService.GetTopScoresAsync(topCount);
        }
    }
}
