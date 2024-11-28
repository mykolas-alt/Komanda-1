using Projektas.Shared.Models;
using System.Net.Http.Json;
using Projektas.Client.Interfaces;

namespace Projektas.Client.Services {
    public class AimTrainerService : IAimTrainerService {
        private readonly HttpClient _httpClient;

        public AimTrainerService(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        public async Task SaveScoreAsync(string username,int score) {
            var data=new UserScoreDto {
                Username=username,
                Data=score
            };
            await _httpClient.PostAsJsonAsync("api/aimtrainer/save-score",data);
        }

        public async Task<int> GetUserHighscore(string username) {
            var url=$"api/aimtrainer/highscore?username={username}";
            return await _httpClient.GetFromJsonAsync<int>(url);
        }

        public async Task<List<UserScoreDto>> GetTopScoresAsync(int topCount=10)  {
            var url=$"api/aimtrainer/top-score?topCount={topCount}";
            return await _httpClient.GetFromJsonAsync<List<UserScoreDto>>(url);
        }
    }
}
