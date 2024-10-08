using System.Net.Http.Json;

namespace Projektas.Client.Services
{
    public class ScoreService
    {
        private readonly HttpClient _httpClient;

        public ScoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<int>> GetTopScoresAsync(List<int> scores)
        {
            var response = await _httpClient.PostAsJsonAsync("api/score/top", scores);
            return await response.Content.ReadFromJsonAsync<List<int>>();
        }
    }
}
