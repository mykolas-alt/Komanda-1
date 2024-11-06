using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Services;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class UserController:ControllerBase {
		private readonly UserService _userService;
		private readonly DatabaseService _databaseService;

		public UserController(UserService userService, DatabaseService databaseService) {
			_userService=userService;
			_databaseService=databaseService;
		}
		
		[HttpPost("create_user")]
		public async Task<IActionResult> CreateUser([FromBody] User newUser) {
			await _databaseService.CreateUserAsync(newUser);
			return Ok();
		}

		[HttpPost("login")]
		public IActionResult LogIn([FromBody]User user) {
			var response=_userService.LogInToUser(user);

			if(response) {
				var token = _userService.GenerateJwtToken(user);
				return Ok(new { Token = token });
			}

			return Unauthorized();
		}

		[HttpGet("get_usernames")]
		public async Task<ActionResult<List<string>>> GetUsernames() {
			return await _userService.GetUsernamesAsync();
		}
	}
}