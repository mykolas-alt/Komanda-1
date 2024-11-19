using Projektas.Shared.Models;
using System.Net.Http.Json;
using Projektas.Client.Interfaces;

namespace Projektas.Client.Services {
    public class MathGameService : IMathGameService {
        private readonly HttpClient _httpClient;

        public MathGameService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task<string> GetQuestionAsync(int score) {
            var url=$"api/mathgame/question?score={score}";
            var response=await _httpClient.GetStringAsync(url);
            return response;
        }

        public async Task<List<int>> GetOptionsAsync() {
            return await _httpClient.GetFromJsonAsync<List<int>>("api/mathgame/options");
        }

        public async Task<bool> CheckAnswerAsync(int answer) {
            var response=await _httpClient.PostAsJsonAsync("api/mathgame/check-answer",answer);
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task SaveScoreAsync(string username,int score) {
            var data=new UserScoreDto {
                Username=username,
                Score=score
            };
            await _httpClient.PostAsJsonAsync("api/mathgame/save-score",data);
        }

        public async Task<int> GetUserHighscore(string username) {
            var url=$"api/mathgame/highscore?username={username}";
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<List<UserScoreDto>> GetTopScoresAsync(int topCount=10)  {
            var url=$"api/mathgame/top-score?topCount={topCount}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto>>(url);
        }
    }
}