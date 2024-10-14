using System.Net.Http.Json;

namespace Projektas.Client.Services {
	public class AccountServices {
		private readonly HttpClient _httpClient;

		public AccountServices(HttpClient httpClient) {
			_httpClient = httpClient;
		}

		public async Task<string> GetTestAsync() {
			return await _httpClient.GetStringAsync("api/account/test");
		}
	}
}