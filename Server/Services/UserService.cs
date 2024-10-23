using Projektas.Client.Pages;
using System.Text.Json;
using Projektas.Shared.Models;

namespace Projektas.Server.Services
{
    public class UserService {
		private readonly string _filepath;
		private List<User>? UserList=new List<User>();

		public UserService(string filepath) {
			_filepath=filepath;
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
