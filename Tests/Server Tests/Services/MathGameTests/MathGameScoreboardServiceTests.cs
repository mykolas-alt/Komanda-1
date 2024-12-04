﻿using Moq;
using Projektas.Server.Services;
using Projektas.Server.Services.MathGame;
using Projektas.Shared.Models;

namespace Projektas.Tests.Services.MathGameTests {
    public class MathGameScoreboardServiceTests {
		private readonly Mock<IScoreRepository> _mockScoreRepository;
		private readonly MathGameScoreboardService _mathGameScoreboardService;

        public MathGameScoreboardServiceTests() {
			_mockScoreRepository=new Mock<IScoreRepository>();
			_mathGameScoreboardService=new MathGameScoreboardService(_mockScoreRepository.Object);
        }

		[Fact]
		public async Task AddScoreToDb_ShouldCallAddMathGameScoreToUserAsync() {
			var userScore=new UserScoreDto {Username="testuser",Data=100};

			await _mathGameScoreboardService.AddScoreToDb(userScore);

			_mockScoreRepository.Verify(
				repo => repo.AddScoreToUserAsync(
					userScore.Username,
					It.Is<MathGameData>(m => m.UserScores==userScore.Data),
					userScore.Data
					),
				Times.Once
				);
		}

		[Fact]
		public async Task GetUserHighscore_ShouldReturnHighscore() {
			var username="testuser";
			var expectedHighscore=200;
			_mockScoreRepository.Setup(repo => repo.GetHighscoreFromUserAsync<MathGameData>(username)).ReturnsAsync(expectedHighscore);

			var result=await _mathGameScoreboardService.GetUserHighscore(username);

			Assert.Equal(expectedHighscore,result);
		}

		[Fact]
		public async Task GetTopScores_ShouldReturnTopScores() {
			var topCount=3;
			var userScores=new List<UserScoreDto> {
				new UserScoreDto {Username="user1",Data=300},
				new UserScoreDto {Username="user2",Data=250},
				new UserScoreDto {Username="user3",Data=200},
				new UserScoreDto {Username="user4",Data=150}
			};
			_mockScoreRepository.Setup(repo => repo.GetAllScoresAsync<MathGameData>()).ReturnsAsync(userScores);

			var result=await _mathGameScoreboardService.GetTopScores(topCount);

			Assert.Equal(topCount,result.Count);
			Assert.Equal(userScores[0],result[0]);
			Assert.Equal(userScores[1],result[1]);
			Assert.Equal(userScores[2],result[2]);
		}
	}
}
