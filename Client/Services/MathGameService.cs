using System.Net.Http.Json;

namespace Projektas.Client.Services
{
    public class MathGameService
    {
        private readonly HttpClient _httpClient;

        public MathGameService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetQuestionAsync()
        {
            var response = await _httpClient.GetStringAsync("api/mathgame/question");
            return response;
        }

        public async Task<List<int>> GetOptionsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<int>>("api/mathgame/options");
        }

        public async Task CheckAnswerAsync(int answer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/mathgame/check-answer", answer);
            response.EnsureSuccessStatusCode();
        }

        public async Task<int> GetScoreAsync()
        {
            return await _httpClient.GetFromJsonAsync<int>("api/mathgame/score");
        }

        public async Task<int> GetLivesAsync()
        {
            return await _httpClient.GetFromJsonAsync<int>("api/mathgame/lives");
        }

        public async Task<int> GetHighscoreAsync()
        {
            return await _httpClient.GetFromJsonAsync<int>("api/mathgame/highscore");
        }

        public async Task<int> ResetScoreAsync()
        {
            var response = await _httpClient.PostAsync("api/mathgame/reset-score", null);
            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task<int> ResetLivesAsync()
        {
            var response = await _httpClient.PostAsync("api/mathgame/reset-lives", null);
            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
}