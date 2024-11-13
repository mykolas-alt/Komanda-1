using Projektas.Shared.Models;
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

        public async Task SaveScoreAsync(string username,int score)
        {
            var data = new UserScoreDto {
                Username = username,
                Score = score
            };
            await _httpClient.PostAsJsonAsync("api/mathgame/save-score", data);
        }

        public async Task<List<UserScoreDto>> GetTopScoresAsync(int topCount = 10)
        {
            var url = $"api/mathgame/top?topCount={topCount}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto>>(url);
        }
    }
}