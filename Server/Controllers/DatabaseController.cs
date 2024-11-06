using Microsoft.AspNetCore.Mvc;
using Projektas.Server.Database;
using Projektas.Server.Interface;
using Projektas.Shared.Models;

namespace Projektas.Server.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class DatabaseController : ControllerBase {
		private readonly IUserRepository _userRepository;
		private readonly AppDbContext _context;

		public DatabaseController(IUserRepository userRepository,AppDbContext context) {
			_userRepository=userRepository;
			_context=context;
		}

		[HttpGet("get_users")]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers() {
			return Ok(await _userRepository.GetAllUsersAsync());
		}

		[HttpGet("get_user/{id}")]
		public async Task<ActionResult<User>> GetUser(int id) {
			var user = await _userRepository.GetUserByIdAsync(id);
			if(user == null)
				return NotFound();

			return Ok(user);
		}

		[HttpPost("create_user")]
		public async Task<ActionResult<User>> CreateUser(User user) {
			var id = await _userRepository.CreateUserAsync(user);
			user.Id = id;
			return CreatedAtAction(nameof(GetUser), new {id=user.Id}, user);
		}
	}
}
