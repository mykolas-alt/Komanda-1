using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Projektas.Server.Interfaces.MathGame;
using System.Net.Http.Json;

namespace Projektas.Tests.Controllers
{
    public class MathGameControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IMathGameDataService> _mockMathGameDataService;
        public MathGameControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            // creating mock services
            _mockMathGameDataService = new Mock<IMathGameDataService>();
            _mockMathGameDataService.Setup(m => m.SaveData(It.IsAny<int>()));
            _mockMathGameDataService.Setup(m => m.LoadData()).Returns(new List<int> { 1, 2, 3 });

            var _mockMathGameService = new Mock<IMathGameService>();
            _mockMathGameService.Setup(m => m.GenerateQuestion(It.IsAny<int>())).Returns("2 + 2");
            _mockMathGameService.Setup(m => m.CheckAnswer(4)).Returns(true);
            _mockMathGameService.Setup(m => m.GenerateOptions()).Returns(new List<int> { 1, 2, 3, 4 });

            var _mockMathGameScoreboardService = new Mock<IMathGameScoreboardService>();
            _mockMathGameScoreboardService.Setup(m => m.GetTopScores(3)).Returns(new List<int> { 3, 2, 1 });

            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(_mockMathGameService.Object);
                    services.AddSingleton(_mockMathGameScoreboardService.Object);
                    services.AddSingleton(_mockMathGameDataService.Object);
                });
            })
            .CreateClient();
        }

        [Fact]
        public async Task GetQuestion_ReturnsQuestion()
        {
            int score = 10;

            var response = await _client.GetAsync($"/api/mathgame/question?score={score}");
            response.EnsureSuccessStatusCode();
            string question = await response.Content.ReadAsStringAsync();

            Assert.False(string.IsNullOrEmpty(question));
            Assert.Equal("2 + 2", question);
        }

        [Fact]
        public async Task GetOptions_ReturnsAlistOfFourIntegers()
        {
            var response = await _client.GetAsync("/api/mathgame/options");
            response.EnsureSuccessStatusCode();
            List<int>? options = await response.Content.ReadFromJsonAsync<List<int>>();

            Assert.NotNull(options);
            Assert.Equal(4, options.Count);
            Assert.Equal(new List<int> { 1, 2, 3, 4 }, options);
        }

        [Fact]
        public async Task CheckAnswer_ReturnsTrue()
        {
            int answer = 4;

            var response = await _client.PostAsJsonAsync("/api/mathgame/check-answer", answer);
            response.EnsureSuccessStatusCode();
            bool result = await response.Content.ReadFromJsonAsync<bool>();

            Assert.True(result);
        }

        [Fact]
        public async Task GetTopScores_ReturnsTopList()
        {
            int topCount = 3;

            var response = await _client.GetAsync($"/api/mathgame/top?topcount={topCount}");
            response.EnsureSuccessStatusCode();
            List<int>? topScores = await response.Content.ReadFromJsonAsync<List<int>>();

            Assert.NotNull(topScores);
            Assert.Equal(topCount, topScores.Count);
            Assert.Equal(new List<int> { 3, 2, 1 }, topScores);
        }

        [Fact]
        public async Task SaveData_SavesDataSuccessfully()
        {
            int data = 5;

            var response = await _client.PostAsJsonAsync("/api/mathgame/save", data);

            response.EnsureSuccessStatusCode();
            _mockMathGameDataService.Verify(m => m.SaveData(data), Times.Once);
        }

        [Fact]
        public async Task LoadData_ReturnsListOfIntegers()
        {
            var response = await _client.GetAsync("/api/mathgame/load");
            response.EnsureSuccessStatusCode();
            List<int>? data = await response.Content.ReadFromJsonAsync<List<int>>();

            Assert.NotNull(data);
            Assert.Equal(new List<int> { 1, 2, 3 }, data);
        }
    }
}
