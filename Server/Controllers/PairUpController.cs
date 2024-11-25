using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class PairUpController : Controller {
        private readonly PairUpService _pairUpService;

        public PairUpController (PairUpService pairUpService) {
            _pairUpService=pairUpService;
        }

        [HttpPost("save-score")]
        public async Task SaveScore([FromBody] UserScoreDto data) {
            await _pairUpService.AddScoreToDb(data);
        }

        [HttpGet("highscore")]
        public async Task<ActionResult<int>> GetUserHighscore([FromQuery] string username) {
            return await _pairUpService.GetUserHighscore(username);
        }

        [HttpGet("top-score")]
        public async Task<ActionResult<List<UserScoreDto>>> GetTopScores([FromQuery] int topCount) {
            return await _pairUpService.GetTopScores(topCount);
        }
    }
}
