using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Projektas.Server.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Server.Services {
    public class UserService : IUserService {
		private readonly IConfiguration _configuration;
		private readonly IUserRepository _userRepository;
		private readonly UserTrackingService _userTrackingService;

		public UserService(IConfiguration configuration,IUserRepository userRepository,UserTrackingService userTrackingService) {
			_configuration=configuration;
			_userRepository=userRepository;
			_userTrackingService=userTrackingService;
		}

		public async Task CreateUser(User newUser) {
			await _userRepository.CreateUserAsync(newUser);
		}

		public async Task<bool> LogInToUser(User userInfo) {
			bool userMached=await _userRepository.ValidateUserAsync(userInfo.Username,userInfo.Password);
			if(userMached) {
				_userTrackingService.AddOrUpdateUser(userInfo.Username);
			}
			return userMached;
		}

		public void LogOffFromUser(string username) {
			_userTrackingService.AddOrUpdateUser(username);
		}

		public string GenerateJwtToken(User user) {
			var claims=new[] {
				new Claim(ClaimTypes.Name,user.Username),
				new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
			};

			var key=Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
			var creds=new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256);

			var token=new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		
		public async Task<List<string>> GetUsernamesAsync() {
			IEnumerable<User> users=await _userRepository.GetAllUsersAsync();
			List<string> usernames=new List<string>();

			foreach(User user in users) {
				usernames.Add(user.Username);
			}

			return usernames;
		}
	}
}
