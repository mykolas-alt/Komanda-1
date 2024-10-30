using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Projektas.Shared.Models;

namespace Projektas.Server.Services
{
    public class UserService {
		private readonly string _filepath;
		private readonly IConfiguration _configuration;
		private List<User>? UserList=new List<User>();

		public UserService(string filepath, IConfiguration configuration) {
			_filepath=filepath;
			_configuration=configuration;
		}

		public bool LogInToUser(User userInfo) {
			string UserListSerialized;
			bool userMached=false;
			using (StreamReader reader = new StreamReader(_filepath)) {
				UserListSerialized=reader.ReadToEnd();
			}
			UserList=JsonSerializer.Deserialize<List<User>>(UserListSerialized);

			foreach(User user in UserList) {
				if(user.Username==userInfo.Username && user.Password==userInfo.Password) {
					userMached=true;
				}
			}

			return userMached;
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
		
		public void CreateUser(User newUser) {
			string UserListSerialized;
			using (StreamReader reader = new StreamReader(_filepath)) {
				UserListSerialized=reader.ReadToEnd();
			}
			UserList=JsonSerializer.Deserialize<List<User>>(UserListSerialized);
			
			UserList.Add(new User(){Name=newUser.Name,Surname=newUser.Surname,Username=newUser.Username,Password=newUser.Password});
			
			UserListSerialized=JsonSerializer.Serialize(UserList);

			using (StreamWriter writer = new StreamWriter(_filepath, append: false)) {
				writer.Write(UserListSerialized);
			}
		}

		public List<string> GetUsernames() {
			List<string> usernames = new List<string>();

			string UserListSerialized;
			using (StreamReader reader = new StreamReader(_filepath)) {
				UserListSerialized=reader.ReadToEnd();
			}
			UserList=JsonSerializer.Deserialize<List<User>>(UserListSerialized);

			foreach(User user in UserList) {
				usernames.Add(user.Username);
			}

			return usernames;
		}
	}
}
