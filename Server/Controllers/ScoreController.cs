using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;

namespace Projektas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScoreController : ControllerBase
    {
        private readonly ScoreService _scoreService;
        
        public ScoreController(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpPost("top")] 
        public ActionResult<List<int>> GetTopScores([FromBody] List<int> scores)
        {
            return _scoreService.GetTopScores(scores);
        }
    }
}
