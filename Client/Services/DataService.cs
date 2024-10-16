using System.Net.Http.Json;

namespace Projektas.Client.Services
{
    public class DataService
    {
        private readonly HttpClient _httpClient;

        public DataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SaveDataAsync(int data)
        {
            await _httpClient.PostAsJsonAsync("api/dataservice/save", data);
        }

        public async Task<List<int>> LoadDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<int>>("api/dataservice/load");
        }
    }
}
