using System.Net.Http.Json;

namespace Projektas.Client.Services
{
    public class ScoreboardService
    {
        private readonly HttpClient _httpClient;

        public ScoreboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<int>> GetTopScoresAsync(List<int> scores, int topCount = 10)
        {
            var url = $"api/scoreboard/top?topCount={topCount}";
            var response = await _httpClient.PostAsJsonAsync(url, scores);
            return await response.Content.ReadFromJsonAsync<List<int>>();
        }
    }
}
