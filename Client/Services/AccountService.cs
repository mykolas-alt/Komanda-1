using Projektas.Shared.Models;
using System.Net.Http.Json;
using Projektas.Client.Interfaces;

namespace Projektas.Client.Services {
    public class AccountService : IAccountService {
		private readonly HttpClient _httpClient;
		private readonly ILocalStorageService _localStorage;

		public AccountService(HttpClient httpClient, ILocalStorageService localStorage) {
			_httpClient=httpClient;
			_localStorage=localStorage;
		}
		
		public async Task<HttpResponseMessage> CreateAccountAsync(User user) {
			return await _httpClient.PostAsJsonAsync("api/user/create_user",user);
		}

		public async Task<string?> LogInAsync(User user) {
			var response=await _httpClient.PostAsJsonAsync("api/user/login",user);

			if(response.IsSuccessStatusCode) {
				var result=await response.Content.ReadFromJsonAsync<LoginResponse>();
				return result?.Token;
			}

			return "";
		}

		public async void LogOffAsync(string username) {
			await _httpClient.DeleteAsync($"api/user/logoff?username={username}");
		}

		public async Task<List<string>> GetUsernamesAsync() {
			return await _httpClient.GetFromJsonAsync<List<string>>("api/user/usernames");
		}
	}
}