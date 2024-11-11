using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Interfaces.MathGame;

namespace Projektas.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MathGameController : ControllerBase
    {
        private readonly IMathGameService _mathGameService;
        private readonly IMathGameDataService _dataService;
        private readonly IMathGameScoreboardService _scoreboardService;

        public MathGameController(IMathGameService mathGameService, IMathGameDataService mathGameDataService,
               IMathGameScoreboardService mathGameScoreboardService)
        {
            _mathGameService = mathGameService;
            _dataService = mathGameDataService;
            _scoreboardService = mathGameScoreboardService;
        }

        [HttpGet("question")]
        public ActionResult<string> GetQuestion([FromQuery] int score)
        {
            return _mathGameService.GenerateQuestion(score);
        }

        [HttpGet("options")]
        public ActionResult<List<int>> GetOptions()
        {
            return _mathGameService.GenerateOptions();
        }

        [HttpPost("check-answer")]
        public ActionResult<bool> CheckAnswer([FromBody] int answer)
        {
            return _mathGameService.CheckAnswer(answer);
        }

        [HttpPost("save")]
        public IActionResult SaveData([FromBody] int data)
        {
            _dataService.SaveData(data);
            return Ok();
        }

        [HttpGet("load")]
        public ActionResult<List<int>> LoadData()
        {

            return _dataService.LoadData();
        }

        [HttpGet("top")]
        public ActionResult<List<int>> GetTopScores([FromQuery] int topCount)
        {
            return _scoreboardService.GetTopScores(topCount);
        }

    }
}