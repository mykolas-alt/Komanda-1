using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Projektas.Shared.Models;

namespace Projektas.Client.Services {
	public class DatabaseService {
		private readonly HttpClient _httpClient;

		public DatabaseService(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task<IEnumerable<User>> GetUsersAsync() {
			return await _httpClient.GetFromJsonAsync<IEnumerable<User>>("api/database/get_users");
		}

		public async Task<User> GetUserByIdAsync(int id) {
			return await _httpClient.GetFromJsonAsync<User>($"api/database/get_user/{id}");
		}

		public async Task<HttpResponseMessage> CreateUserAsync(User user) {
			return await _httpClient.PostAsJsonAsync("api/database/create_user",user);
		}
	}
}