using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;

namespace Projektas.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DataServiceController : ControllerBase
    {
        private readonly DataAPIService _dataService;

        public DataServiceController(DataAPIService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("save")]
        public IActionResult SaveData([FromBody] int data)
        {
            _dataService.SaveData(data);
            return Ok();
        }

        [HttpGet("load")]
        public ActionResult <List<int>> LoadData()
        {

            return _dataService.LoadData();
        }
    }
}
