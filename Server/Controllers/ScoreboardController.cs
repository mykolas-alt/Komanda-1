using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;

namespace Projektas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreboardController : ControllerBase
    {
        private readonly ScoreboardAPIService _scoreboardService;
        
        public ScoreboardController(ScoreboardAPIService scoreboardService)
        {
            _scoreboardService = scoreboardService;
        }

        [HttpPost("top")] 
        public ActionResult<List<int>> GetTopScores([FromBody] List<int> scores, [FromQuery] int topCount = 10)
        {
            return _scoreboardService.GetTopScores(scores, topCount);
        }
    }
}
