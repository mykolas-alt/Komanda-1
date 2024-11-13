using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers {
    [ApiController]
	[Route("api/[controller]")]
	public class UserController:ControllerBase {
		private readonly UserService _userService;

		public UserController(UserService userService) {
			_userService=userService;
		}
		
		[HttpPost("create_user")]
		public async Task<IActionResult> CreateUser([FromBody] User newUser) {
			await _userService.CreateUser(newUser);
			return Ok();
		}

		[HttpPost("login")]
		public async Task<IActionResult> LogIn([FromBody]User user) {
			var response=_userService.LogInToUser(user);

			if(response.Result) {
				var token=_userService.GenerateJwtToken(user);
				return Ok(new {Token=token});
			}

			return Unauthorized();
		}

		[HttpGet("usernames")]
		public async Task<ActionResult<List<string>>> GetUsernames() {
			return await _userService.GetUsernamesAsync();
		}
	}
}