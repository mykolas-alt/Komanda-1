using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Projektas.Server.Interfaces.MathGame;
using Projektas.Shared.Models;
using System.Net.Http.Json;

namespace Projektas.Tests.Controllers
{
	public class MathGameControllerTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly HttpClient _client;
		private readonly WebApplicationFactory<Program> _factory;
		private readonly Mock<IMathGameService> _mockMathGameService;
		private readonly Mock<IMathGameScoreboardService> _mockMathGameScoreboardService;

		public MathGameControllerTests(WebApplicationFactory<Program> factory)
		{
			_factory = factory;

			// creating mock services
			_mockMathGameService = new Mock<IMathGameService>();
			_mockMathGameService.Setup(m => m.GenerateQuestion(It.IsAny<int>())).Returns("2 + 2");
			_mockMathGameService.Setup(m => m.CheckAnswer(4)).Returns(true);
			_mockMathGameService.Setup(m => m.GenerateOptions()).Returns(new List<int> { 1, 2, 3, 4 });

			_mockMathGameScoreboardService = new Mock<IMathGameScoreboardService>();
			_mockMathGameScoreboardService.Setup(m => m.GetTopScores(3)).ReturnsAsync(new List<UserScoreDto>
			{
				new UserScoreDto { Username = "User1", Score = 3 },
				new UserScoreDto { Username = "User2", Score = 2 },
				new UserScoreDto { Username = "User3", Score = 1 }
			});
			_mockMathGameScoreboardService.Setup(m => m.GetUserHighscore("testuser")).ReturnsAsync(10);
			_mockMathGameScoreboardService.Setup(m => m.AddScoreToDb(It.IsAny<UserScoreDto>())).Returns(Task.CompletedTask);

			_client = _factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddSingleton(_mockMathGameService.Object);
					services.AddSingleton(_mockMathGameScoreboardService.Object);
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
		public async Task SaveScore_SavesScoreSuccessfully()
		{
			var data = new UserScoreDto { Username = "testuser", Score = 5 };

			var response = await _client.PostAsJsonAsync("/api/mathgame/save-score", data);

			response.EnsureSuccessStatusCode();
			_mockMathGameScoreboardService.Verify(m => m.AddScoreToDb(It.Is<UserScoreDto>(d => d.Username == "testuser" && d.Score == 5)), Times.Once);
		}

		[Fact]
		public async Task GetUserHighscore_ReturnsHighscore()
		{
			string username = "testuser";

			var response = await _client.GetAsync($"/api/mathgame/highscore?username={username}");
			response.EnsureSuccessStatusCode();
			int highscore = await response.Content.ReadFromJsonAsync<int>();

			Assert.Equal(10, highscore);
		}

		[Fact]
		public async Task GetTopScores_ReturnsTopList()
		{
			int topCount = 3;

			var response = await _client.GetAsync($"/api/mathgame/top-score?topCount={topCount}");
			response.EnsureSuccessStatusCode();
			List<UserScoreDto>? topScores = await response.Content.ReadFromJsonAsync<List<UserScoreDto>>();

			Assert.NotNull(topScores);
			Assert.Equal(topCount, topScores.Count);
			Assert.Equal(3, topScores[0].Score);
			Assert.Equal(2, topScores[1].Score);
			Assert.Equal(1, topScores[2].Score);
		}
	}
}
