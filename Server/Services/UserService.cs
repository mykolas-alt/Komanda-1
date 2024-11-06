using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Projektas.Server.Services;
using Projektas.Shared.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Projektas.Server.Services
{
    public class UserService {
		private readonly IConfiguration _configuration;
		private readonly DatabaseService _databaseService;
		private IEnumerable<User> users;

		public UserService(IConfiguration configuration,DatabaseService databaseService) {
			_configuration=configuration;
			_databaseService=databaseService;
		}

		public async Task<bool> LogInToUser(User userInfo) {
			bool userMached = await _databaseService.ValidateUserAsync(userInfo.Username, userInfo.Password);
			return userMached;
			/*
			using (StreamReader reader = new StreamReader(_filepath)) {
				UserListSerialized=reader.ReadToEnd();
			}
			UserList=JsonSerializer.Deserialize<List<User>>(UserListSerialized);

			foreach(User user in UserList) {
				if(user.Username==userInfo.Username && user.Password==userInfo.Password) {
					userMached=true;
				}
			}
			*/

		}

		public string GenerateJwtToken(User user) {
			var claims = new[] {
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
			var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"],
				audience: _configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(30),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		
		public async Task<List<string>> GetUsernamesAsync() {
			users = await _databaseService.GetAllUsersAsync();
			List<string> usernames = new List<string>();

			foreach(User user in users) {
				usernames.Add(user.Username);
			}

			return usernames;

			/*List<string> usernames = new List<string>();
			
			string UserListSerialized;
			using (StreamReader reader = new StreamReader("Data/UsersData.txt")) {
				UserListSerialized=reader.ReadToEnd();
			}
			UserList=JsonSerializer.Deserialize<List<User>>(UserListSerialized);

			foreach(User user in UserList) {
				usernames.Add(user.Username);
			}
			
			return usernames;*/
		}
	}
}
