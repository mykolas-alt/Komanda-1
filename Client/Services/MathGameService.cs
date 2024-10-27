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

        public async Task<string> GetQuestionAsync(int score)
        {
            var url = $"api/mathgame/question?score={score}";
            var response = await _httpClient.GetStringAsync(url);
            return response;
        }

        public async Task<List<int>> GetOptionsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<int>>("api/mathgame/options");
        }

        public async Task<bool> CheckAnswerAsync(int answer)
        {
            var response = await _httpClient.PostAsJsonAsync("api/mathgame/check-answer", answer);
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task SaveDataAsync(int data)
        {
            await _httpClient.PostAsJsonAsync("api/mathgame/save", data);
        }

        public async Task<List<int>> LoadDataAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<int>>("api/mathgame/load");
        }

        public async Task<List<int>> GetTopScoresAsync(int topCount = 10)
        {
            var url = $"api/mathgame/top?topCount={topCount}";
            return await _httpClient.GetFromJsonAsync<List<int>>(url);
        }
    }
}