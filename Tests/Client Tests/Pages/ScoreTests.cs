using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Projektas.Client.Interfaces;
using Projektas.Client.Pages;
using Projektas.Shared.Models;
using Projektas.Shared.Enums;

namespace Projektas.Tests.Client_Tests.Pages
{
    public class ScoreTests : TestContext
    {
        private readonly Mock<IAccountScoreService> _mockAccountScoreService;
        private readonly Mock<IAccountAuthStateProvider> _mockAccountAuthStateProvider;

        string username = "testuser";

        List<UserScoreDto<AimTrainerData>> AimTrainerScores { get; set; }
        List<UserScoreDto<MathGameData>> MathGameScores { get; set; }
        List<UserScoreDto<PairUpData>> PairUpScores { get; set; }
        List<UserScoreDto<SudokuData>> SudokuScores { get; set; }

        int MathGame_Played { get; set; }

        int AimTrainer_Played_Normal { get; set; }
        int AimTrainer_Played_Hard { get; set; }

        int PairUp_Played_Easy { get; set; }
        int PairUp_Played_Normal { get; set; }
        int PairUp_Played_Hard { get; set; }

        int Sudoku_Played_Easy_4x4 { get; set; }
        int Sudoku_Played_Normal_4x4 { get; set; }
        int Sudoku_Played_Hard_4x4 { get; set; }

        int Sudoku_Played_Easy_9x9 { get; set; }
        int Sudoku_Played_Normal_9x9 { get; set; }
        int Sudoku_Played_Hard_9x9 { get; set; }

        int Sudoku_Played_Easy_16x16 { get; set; }
        int Sudoku_Played_Normal_16x16 { get; set; }
        int Sudoku_Played_Hard_16x16 { get; set; }


        GameScore MathGame_Highscore { get; set; }

        GameScore AimTrainer_Highscore_Normal { get; set; }
        GameScore AimTrainer_Highscore_Hard { get; set; }

        GameScore PairUp_Highscore_Easy { get; set; }
        GameScore PairUp_Highscore_Normal { get; set; }
        GameScore PairUp_Highscore_Hard { get; set; }

        GameScore Sudoku_Highscore_Easy_4x4 { get; set; }
        GameScore Sudoku_Highscore_Normal_4x4 { get; set; }
        GameScore Sudoku_Highscore_Hard_4x4 { get; set; }

        GameScore Sudoku_Highscore_Easy_9x9 { get; set; }
        GameScore Sudoku_Highscore_Normal_9x9 { get; set; }
        GameScore Sudoku_Highscore_Hard_9x9 { get; set; }

        GameScore Sudoku_Highscore_Easy_16x16 { get; set; }
        GameScore Sudoku_Highscore_Normal_16x16 { get; set; }
        GameScore Sudoku_Highscore_Hard_16x16 { get; set; }


        GameScore MathGame_AllTimeAverage { get; set; }

        GameScore AimTrainer_AllTimeAverage_Normal { get; set; }
        GameScore AimTrainer_AllTimeAverage_Hard { get; set; }

        GameScore PairUp_AllTimeAverage_Easy { get; set; }
        GameScore PairUp_AllTimeAverage_Normal { get; set; }
        GameScore PairUp_AllTimeAverage_Hard { get; set; }

        GameScore Sudoku_AllTimeAverage_Easy_4x4 { get; set; }
        GameScore Sudoku_AllTimeAverage_Normal_4x4 { get; set; }
        GameScore Sudoku_AllTimeAverage_Hard_4x4 { get; set; }

        GameScore Sudoku_AllTimeAverage_Easy_9x9 { get; set; }
        GameScore Sudoku_AllTimeAverage_Normal_9x9 { get; set; }
        GameScore Sudoku_AllTimeAverage_Hard_9x9 { get; set; }

        GameScore Sudoku_AllTimeAverage_Easy_16x16 { get; set; }
        GameScore Sudoku_AllTimeAverage_Normal_16x16 { get; set; }
        GameScore Sudoku_AllTimeAverage_Hard_16x16 { get; set; }


        List<AverageScoreDto> MathGame_Average_Last7Days_Easy { get; set; }
        List<AverageScoreDto> MathGame_Average_Last7Days_Normal { get; set; }
        List<AverageScoreDto> MathGame_Average_Last7Days_Hard { get; set; }


        List<AverageScoreDto> AimTrainer_Average_Last7Days_Normal { get; set; }
        List<AverageScoreDto> AimTrainer_Average_Last7Days_Hard { get; set; }

        List<AverageScoreDto> PairUp_Average_Last7Days_Easy { get; set; }
        List<AverageScoreDto> PairUp_Average_Last7Days_Normal { get; set; }
        List<AverageScoreDto> PairUp_Average_Last7Days_Hard { get; set; }

        List<AverageScoreDto> Sudoku_Average_Last7Days_Easy_4x4 { get; set; }
        List<AverageScoreDto> Sudoku_Average_Last7Days_Normal_4x4 { get; set; }
        List<AverageScoreDto> Sudoku_Average_Last7Days_Hard_4x4 { get; set; }

        List<AverageScoreDto> Sudoku_Average_Last7Days_Easy_9x9 { get; set; }
        List<AverageScoreDto> Sudoku_Average_Last7Days_Normal_9x9 { get; set; }
        List<AverageScoreDto> Sudoku_Average_Last7Days_Hard_9x9 { get; set; }

        List<AverageScoreDto> Sudoku_Average_Last7Days_Easy_16x16 { get; set; }
        List<AverageScoreDto> Sudoku_Average_Last7Days_Normal_16x16 { get; set; }
        List<AverageScoreDto> Sudoku_Average_Last7Days_Hard_16x16 { get; set; }

        public ScoreTests()
        {
            _mockAccountScoreService = new Mock<IAccountScoreService>();
            _mockAccountAuthStateProvider = new Mock<IAccountAuthStateProvider>();

            Services.AddSingleton(_mockAccountScoreService.Object);
            Services.AddSingleton(_mockAccountAuthStateProvider.Object);

            MathGameSetup();
            AimTrainerSetup();
            PairUpSetup();
            SudokuSetup();

        }

        [Fact]
        public async Task LoadMathGameScoresAsync_ShouldSetPropertiesCorrectly()
        {
            var component = RenderComponent<Score>();

            await component.Instance.LoadMathGameScoresAsync();

            Assert.Equal(MathGameScores, component.Instance.MathGameScores);
            Assert.Equal(MathGame_Played, component.Instance.MathGame_Played_Easy);
            Assert.Equal(MathGame_Highscore, component.Instance.MathGame_Highscore_Easy);
            Assert.Equal(MathGame_AllTimeAverage, component.Instance.MathGame_AllTimeAverage_Easy);
            Assert.Equal(MathGame_Average_Last7Days_Easy, component.Instance.MathGame_Average_Last7Days_Easy);
        }

        [Fact]
        public async Task LoadAimTrainerScoresAsync_ShouldSetPropertiesCorrectly()
        {
            var component = RenderComponent<Score>();

            await component.Instance.LoadAimTrainerScoresAsync();

            Assert.Equal(AimTrainerScores, component.Instance.AimTrainerScores);
            Assert.Equal(AimTrainer_Played_Normal, component.Instance.AimTrainer_Played_Normal);
            Assert.Equal(AimTrainer_Played_Hard, component.Instance.AimTrainer_Played_Hard);
            Assert.Equal(AimTrainer_Highscore_Normal, component.Instance.AimTrainer_Highscore_Normal);
            Assert.Equal(AimTrainer_Highscore_Hard, component.Instance.AimTrainer_Highscore_Hard);
            Assert.Equal(AimTrainer_AllTimeAverage_Normal, component.Instance.AimTrainer_AllTimeAverage_Normal);
            Assert.Equal(AimTrainer_AllTimeAverage_Hard, component.Instance.AimTrainer_AllTimeAverage_Hard);
            Assert.Equal(AimTrainer_Average_Last7Days_Normal, component.Instance.AimTrainer_Average_Last7Days_Normal);
            Assert.Equal(AimTrainer_Average_Last7Days_Hard, component.Instance.AimTrainer_Average_Last7Days_Hard);
        }

        [Fact]
        public async Task LoadPairUpScoresAsync_ShouldSetPropertiesCorrectly()
        {
            var component = RenderComponent<Score>();

            await component.Instance.LoadPairUpScoresAsync();

            Assert.Equal(PairUpScores, component.Instance.PairUpScores);
            Assert.Equal(PairUp_Played_Easy, component.Instance.PairUp_Played_Easy);
            Assert.Equal(PairUp_Played_Normal, component.Instance.PairUp_Played_Normal);
            Assert.Equal(PairUp_Played_Hard, component.Instance.PairUp_Played_Hard);
            Assert.Equal(PairUp_Highscore_Easy, component.Instance.PairUp_Highscore_Easy);
            Assert.Equal(PairUp_Highscore_Normal, component.Instance.PairUp_Highscore_Normal);
            Assert.Equal(PairUp_Highscore_Hard, component.Instance.PairUp_Highscore_Hard);
            Assert.Equal(PairUp_AllTimeAverage_Easy, component.Instance.PairUp_AllTimeAverage_Easy);
            Assert.Equal(PairUp_AllTimeAverage_Normal, component.Instance.PairUp_AllTimeAverage_Normal);
            Assert.Equal(PairUp_AllTimeAverage_Hard, component.Instance.PairUp_AllTimeAverage_Hard);
            Assert.Equal(PairUp_Average_Last7Days_Easy, component.Instance.PairUp_Average_Last7Days_Easy);
            Assert.Equal(PairUp_Average_Last7Days_Normal, component.Instance.PairUp_Average_Last7Days_Normal);
            Assert.Equal(PairUp_Average_Last7Days_Hard, component.Instance.PairUp_Average_Last7Days_Hard);
        }

        [Fact]
        public async Task LoadSudokuScoresAsync_ShouldSetPropertiesCorrectly()
        {
            var component = RenderComponent<Score>();

            await component.Instance.LoadSudokuScoresAsync();

            Assert.Equal(SudokuScores, component.Instance.SudokuScores);
            Assert.Equal(Sudoku_Played_Easy_4x4, component.Instance.Sudoku_Played_Easy_4x4);
            Assert.Equal(Sudoku_Played_Normal_4x4, component.Instance.Sudoku_Played_Normal_4x4);
            Assert.Equal(Sudoku_Played_Hard_4x4, component.Instance.Sudoku_Played_Hard_4x4);
            Assert.Equal(Sudoku_Played_Easy_9x9, component.Instance.Sudoku_Played_Easy_9x9);
            Assert.Equal(Sudoku_Played_Normal_9x9, component.Instance.Sudoku_Played_Normal_9x9);
            Assert.Equal(Sudoku_Played_Hard_9x9, component.Instance.Sudoku_Played_Hard_9x9);
            Assert.Equal(Sudoku_Played_Easy_16x16, component.Instance.Sudoku_Played_Easy_16x16);
            Assert.Equal(Sudoku_Played_Normal_16x16, component.Instance.Sudoku_Played_Normal_16x16);
            Assert.Equal(Sudoku_Played_Hard_16x16, component.Instance.Sudoku_Played_Hard_16x16);
            Assert.Equal(Sudoku_Highscore_Easy_4x4, component.Instance.Sudoku_Highscore_Easy_4x4);
            Assert.Equal(Sudoku_Highscore_Normal_4x4, component.Instance.Sudoku_Highscore_Normal_4x4);
            Assert.Equal(Sudoku_Highscore_Hard_4x4, component.Instance.Sudoku_Highscore_Hard_4x4);
            Assert.Equal(Sudoku_Highscore_Easy_9x9, component.Instance.Sudoku_Highscore_Easy_9x9);
            Assert.Equal(Sudoku_Highscore_Normal_9x9, component.Instance.Sudoku_Highscore_Normal_9x9);
            Assert.Equal(Sudoku_Highscore_Hard_9x9, component.Instance.Sudoku_Highscore_Hard_9x9);
            Assert.Equal(Sudoku_Highscore_Easy_16x16, component.Instance.Sudoku_Highscore_Easy_16x16);
            Assert.Equal(Sudoku_Highscore_Normal_16x16, component.Instance.Sudoku_Highscore_Normal_16x16);
            Assert.Equal(Sudoku_Highscore_Hard_16x16, component.Instance.Sudoku_Highscore_Hard_16x16);
            Assert.Equal(Sudoku_AllTimeAverage_Easy_4x4, component.Instance.Sudoku_AllTimeAverage_Easy_4x4);
            Assert.Equal(Sudoku_AllTimeAverage_Normal_4x4, component.Instance.Sudoku_AllTimeAverage_Normal_4x4);
            Assert.Equal(Sudoku_AllTimeAverage_Hard_4x4, component.Instance.Sudoku_AllTimeAverage_Hard_4x4);
            Assert.Equal(Sudoku_AllTimeAverage_Easy_9x9, component.Instance.Sudoku_AllTimeAverage_Easy_9x9);
            Assert.Equal(Sudoku_AllTimeAverage_Normal_9x9, component.Instance.Sudoku_AllTimeAverage_Normal_9x9);
            Assert.Equal(Sudoku_AllTimeAverage_Hard_9x9, component.Instance.Sudoku_AllTimeAverage_Hard_9x9);
            Assert.Equal(Sudoku_AllTimeAverage_Easy_16x16, component.Instance.Sudoku_AllTimeAverage_Easy_16x16);
            Assert.Equal(Sudoku_AllTimeAverage_Normal_16x16, component.Instance.Sudoku_AllTimeAverage_Normal_16x16);
            Assert.Equal(Sudoku_AllTimeAverage_Hard_16x16, component.Instance.Sudoku_AllTimeAverage_Hard_16x16);
            Assert.Equal(Sudoku_Average_Last7Days_Easy_4x4, component.Instance.Sudoku_Average_Last7Days_Easy_4x4);
            Assert.Equal(Sudoku_Average_Last7Days_Normal_4x4, component.Instance.Sudoku_Average_Last7Days_Normal_4x4);
            Assert.Equal(Sudoku_Average_Last7Days_Hard_4x4, component.Instance.Sudoku_Average_Last7Days_Hard_4x4);
            Assert.Equal(Sudoku_Average_Last7Days_Easy_9x9, component.Instance.Sudoku_Average_Last7Days_Easy_9x9);
            Assert.Equal(Sudoku_Average_Last7Days_Normal_9x9, component.Instance.Sudoku_Average_Last7Days_Normal_9x9);
            Assert.Equal(Sudoku_Average_Last7Days_Hard_9x9, component.Instance.Sudoku_Average_Last7Days_Hard_9x9);
            Assert.Equal(Sudoku_Average_Last7Days_Easy_16x16, component.Instance.Sudoku_Average_Last7Days_Easy_16x16);
            Assert.Equal(Sudoku_Average_Last7Days_Normal_16x16, component.Instance.Sudoku_Average_Last7Days_Normal_16x16);
            Assert.Equal(Sudoku_Average_Last7Days_Hard_16x16, component.Instance.Sudoku_Average_Last7Days_Hard_16x16);
        }

        [Fact]
        public async Task LoadAimTrainerDatasets_ShouldLoadCorrectData()
        {
            var component = RenderComponent<Score>();
            await component.Instance.LoadAimTrainerScoresAsync();

            component.Instance.LoadAimTrainerDatasets();
            Assert.NotNull(component.Instance.AimTrainer_Average_Last7Days_Dataset);
            Assert.Equal(2, component.Instance.AimTrainer_Average_Last7Days_Dataset.Length);

            Assert.Equal("Normal difficulty", component.Instance.AimTrainer_Average_Last7Days_Dataset[0].Label);
            Assert.Equal(new[] { 24, 25, 26, 27, 28, 29, 30 }, component.Instance.AimTrainer_Average_Last7Days_Dataset[0].Data);
            Assert.Equal("rgba(54, 162, 235, 1)", component.Instance.AimTrainer_Average_Last7Days_Dataset[0].BorderColor);

            Assert.Equal("Hard difficulty", component.Instance.AimTrainer_Average_Last7Days_Dataset[1].Label);
            Assert.Equal(new[] { 29, 30, 31, 32, 33, 34, 35 }, component.Instance.AimTrainer_Average_Last7Days_Dataset[1].Data);
            Assert.Equal("rgba(255, 99, 132, 1)", component.Instance.AimTrainer_Average_Last7Days_Dataset[1].BorderColor);
        }

        [Fact]
        public async Task LoadMathGameDatasets_ShouldLoadCorrectData()
        {
            
            var component = RenderComponent<Score>();
            await component.Instance.LoadMathGameScoresAsync();

            component.Instance.LoadMathGameDatasets();

            Assert.NotNull(component.Instance.MathGame_Average_Last7Days_Dataset);

            var dataset = component.Instance.MathGame_Average_Last7Days_Dataset[0];
            Assert.Equal("Easy difficulty", dataset.Label);
            Assert.Equal(new[] { 10, 12, 13, 14, 15, 16, 17 }, dataset.Data);
            Assert.Equal("rgba(75, 192, 192, 1)", dataset.BorderColor);
        }

        [Fact]
        public async Task LoadPairUpDatasets_ShouldLoadCorrectData()
        {
            var component = RenderComponent<Score>();
            await component.Instance.LoadPairUpScoresAsync();

            component.Instance.LoadPairUpDatasets();

            Assert.NotNull(component.Instance.PairUp_Average_Score_Last7Days_Dataset);
            Assert.Equal(3, component.Instance.PairUp_Average_Score_Last7Days_Dataset.Length);

            Assert.Equal("Easy difficulty", component.Instance.PairUp_Average_Score_Last7Days_Dataset[0].Label);
            Assert.Equal(new[] { 4, 5, 6, 7, 8, 9, 10 }, component.Instance.PairUp_Average_Score_Last7Days_Dataset[0].Data);
            Assert.Equal("rgba(255, 206, 86, 1)", component.Instance.PairUp_Average_Score_Last7Days_Dataset[0].BorderColor);

            Assert.Equal("Normal difficulty", component.Instance.PairUp_Average_Score_Last7Days_Dataset[1].Label);
            Assert.Equal(new[] { 14, 15, 16, 17, 18, 19, 20 }, component.Instance.PairUp_Average_Score_Last7Days_Dataset[1].Data);
            Assert.Equal("rgba(153, 102, 255, 1)", component.Instance.PairUp_Average_Score_Last7Days_Dataset[1].BorderColor);

            Assert.Equal("Hard difficulty", component.Instance.PairUp_Average_Score_Last7Days_Dataset[2].Label);
            Assert.Equal(new[] { 24, 25, 26, 27, 28, 29, 30 }, component.Instance.PairUp_Average_Score_Last7Days_Dataset[2].Data);
            Assert.Equal("rgba(255, 159, 64, 1)", component.Instance.PairUp_Average_Score_Last7Days_Dataset[2].BorderColor);

            Assert.NotNull(component.Instance.PairUp_Average_Time_Last7Days_Dataset);
            Assert.Equal(3, component.Instance.PairUp_Average_Time_Last7Days_Dataset.Length);

            Assert.Equal("Easy difficulty", component.Instance.PairUp_Average_Time_Last7Days_Dataset[0].Label);
            Assert.Equal(new[] { 28, 27, 26, 25, 24, 23, 22 }, component.Instance.PairUp_Average_Time_Last7Days_Dataset[0].Data);
            Assert.Equal("rgba(75, 192, 192, 1)", component.Instance.PairUp_Average_Time_Last7Days_Dataset[0].BorderColor);

            Assert.Equal("Normal difficulty", component.Instance.PairUp_Average_Time_Last7Days_Dataset[1].Label);
            Assert.Equal(new[] { 18, 17, 16, 15, 14, 13, 12 }, component.Instance.PairUp_Average_Time_Last7Days_Dataset[1].Data);
            Assert.Equal("rgba(255, 99, 132, 1)", component.Instance.PairUp_Average_Time_Last7Days_Dataset[1].BorderColor);

            Assert.Equal("Hard difficulty", component.Instance.PairUp_Average_Time_Last7Days_Dataset[2].Label);
            Assert.Equal(new[] { 8, 7, 6, 5, 4, 3, 2 }, component.Instance.PairUp_Average_Time_Last7Days_Dataset[2].Data);
            Assert.Equal("rgba(255, 159, 64, 1)", component.Instance.PairUp_Average_Time_Last7Days_Dataset[2].BorderColor);
        }

        [Fact]
        public async Task LoadSudokuDatasets_ShouldLoadCorrectData()
        {
            var component = RenderComponent<Score>();
            await component.Instance.LoadSudokuScoresAsync();

            component.Instance.LoadSudokuDatasets();

            Assert.NotNull(component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset);
            Assert.Equal(3, component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset.Length);

            Assert.Equal("Easy difficulty", component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset[0].Label);
            Assert.Equal(new[] { 210, 220, 230, 240, 250, 260, 270 }, component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset[0].Data);
            Assert.Equal("rgba(255, 165, 0, 1)", component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset[0].BorderColor);

            Assert.Equal("Normal difficulty", component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset[1].Label);
            Assert.Equal(new[] { 260, 270, 280, 290, 300, 310, 320 }, component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset[1].Data);
            Assert.Equal("rgba(75, 0, 130, 1)", component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset[1].BorderColor);

            Assert.Equal("Hard difficulty", component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset[2].Label);
            Assert.Equal(new[] { 310, 320, 330, 340, 350, 360, 370 }, component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset[2].Data);
            Assert.Equal("rgba(255, 20, 147, 1)", component.Instance.Sudoku_Average_Time_Last7Days_4x4_Dataset[2].BorderColor);

            Assert.NotNull(component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset);
            Assert.Equal(3, component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset.Length);

            Assert.Equal("Easy difficulty", component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset[0].Label);
            Assert.Equal(new[] { 230, 240, 250, 260, 270, 280, 290 }, component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset[0].Data);
            Assert.Equal("rgba(238, 130, 238, 1)", component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset[0].BorderColor);

            Assert.Equal("Normal difficulty", component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset[1].Label);
            Assert.Equal(new[] { 280, 290, 300, 310, 320, 330, 340 }, component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset[1].Data);
            Assert.Equal("rgba(60, 179, 113, 1)", component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset[1].BorderColor);

            Assert.Equal("Hard difficulty", component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset[2].Label);
            Assert.Equal(new[] { 330, 340, 350, 360, 370, 380, 390 }, component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset[2].Data);
            Assert.Equal("rgba(255, 165, 0, 1)", component.Instance.Sudoku_Average_Time_Last7Days_9x9_Dataset[2].BorderColor);

            Assert.NotNull(component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset);
            Assert.Equal(3, component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset.Length);

            Assert.Equal("Easy difficulty", component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset[0].Label);
            Assert.Equal(new[] { 250, 260, 270, 280, 290, 300, 310 }, component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset[0].Data);
            Assert.Equal("rgba(75, 192, 192, 1)", component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset[0].BorderColor);

            Assert.Equal("Normal difficulty", component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset[1].Label);
            Assert.Equal(new[] { 300, 310, 320, 330, 340, 350, 360 }, component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset[1].Data);
            Assert.Equal("rgba(54, 162, 235, 1)", component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset[1].BorderColor);

            Assert.Equal("Hard difficulty", component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset[2].Label);
            Assert.Equal(new[] { 350, 360, 370, 380, 390, 400, 410 }, component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset[2].Data);
            Assert.Equal("rgba(255, 99, 132, 1)", component.Instance.Sudoku_Average_Time_Last7Days_16x16_Dataset[2].BorderColor);
        }

        private void MathGameSetup()
        {
            MathGameScores = new List<UserScoreDto<MathGameData>> {
                new UserScoreDto<MathGameData>
                {
                    Username = username,
                    GameData = new MathGameData { Scores = 17 },
                    Timestamp = DateTime.Now
                }
            };

            MathGame_Played = 10;

            MathGame_Highscore = new GameScore { Scores = 20 };
            MathGame_AllTimeAverage = new GameScore { Scores = 14 };

            MathGame_Average_Last7Days_Easy = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { Scores = 10 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { Scores = 12 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { Scores = 13 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { Scores = 14 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { Scores = 15 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { Scores = 16 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { Scores = 17 }, Date = DateTime.Today }
            };

            MathGame_Average_Last7Days_Normal = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { Scores = 18 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { Scores = 17 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { Scores = 16 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { Scores = 15 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { Scores = 14 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { Scores = 13 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { Scores = 12 }, Date = DateTime.Today }
            };

            MathGame_Average_Last7Days_Hard = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { Scores = 22 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { Scores = 23 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { Scores = 24 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { Scores = 25 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { Scores = 26 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { Scores = 27 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { Scores = 28 }, Date = DateTime.Today }
            };

            // Mock service setup
            _mockAccountScoreService.Setup(s => s.GetMathGameScoresAsync(It.IsAny<string>()))
                .ReturnsAsync(MathGameScores);

            _mockAccountScoreService.Setup(s => s.GetMathGameMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Easy))
                .ReturnsAsync(MathGame_Played);
            _mockAccountScoreService.Setup(s => s.GetMathGameMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Normal))
                .ReturnsAsync(MathGame_Played);
            _mockAccountScoreService.Setup(s => s.GetMathGameMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Hard))
                .ReturnsAsync(MathGame_Played);

            _mockAccountScoreService.Setup(s => s.GetMathGameHighscoreAsync(It.IsAny<string>(), GameDifficulty.Easy))
                .ReturnsAsync(MathGame_Highscore);
            _mockAccountScoreService.Setup(s => s.GetMathGameHighscoreAsync(It.IsAny<string>(), GameDifficulty.Normal))
                .ReturnsAsync(MathGame_Highscore);
            _mockAccountScoreService.Setup(s => s.GetMathGameHighscoreAsync(It.IsAny<string>(), GameDifficulty.Hard))
                .ReturnsAsync(MathGame_Highscore);

            _mockAccountScoreService.Setup(s => s.GetMathGameAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Easy))
                .ReturnsAsync(MathGame_AllTimeAverage);
            _mockAccountScoreService.Setup(s => s.GetMathGameAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Normal))
                .ReturnsAsync(MathGame_AllTimeAverage);
            _mockAccountScoreService.Setup(s => s.GetMathGameAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Hard))
                .ReturnsAsync(MathGame_AllTimeAverage);

            _mockAccountScoreService.Setup(s => s.GetMathGameAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Easy))
                .ReturnsAsync(MathGame_Average_Last7Days_Easy);
            _mockAccountScoreService.Setup(s => s.GetMathGameAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Normal))
                .ReturnsAsync(MathGame_Average_Last7Days_Normal);
            _mockAccountScoreService.Setup(s => s.GetMathGameAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Hard))
                .ReturnsAsync(MathGame_Average_Last7Days_Hard);
        }


        private void AimTrainerSetup()
        {
            AimTrainerScores = new List<UserScoreDto<AimTrainerData>> {
                new UserScoreDto<AimTrainerData>
                {
                    Username = username,
                    GameData = new AimTrainerData
                    {
                        Scores = 25
                    },
                    Timestamp = DateTime.Now
                }
            };
            AimTrainer_Played_Normal = 15;
            AimTrainer_Played_Hard = 5;
            AimTrainer_Highscore_Normal = new GameScore { Scores = 30 };
            AimTrainer_Highscore_Hard = new GameScore { Scores = 35 };
            AimTrainer_AllTimeAverage_Normal = new GameScore { Scores = 22 };
            AimTrainer_AllTimeAverage_Hard = new GameScore { Scores = 28 };
            AimTrainer_Average_Last7Days_Normal = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { Scores = 24 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { Scores = 25 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { Scores = 26 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { Scores = 27 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { Scores = 28 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { Scores = 29 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { Scores = 30 }, Date = DateTime.Today }
            };

            AimTrainer_Average_Last7Days_Hard = new List<AverageScoreDto>
            
            {
                new AverageScoreDto { Score = new GameScore { Scores = 29 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { Scores = 30 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { Scores = 31 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { Scores = 32 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { Scores = 33 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { Scores = 34 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { Scores = 35 }, Date = DateTime.Today }
            };

            _mockAccountScoreService.Setup(s => s.GetAimTrainerScoresAsync(It.IsAny<string>())).ReturnsAsync(AimTrainerScores);
            _mockAccountScoreService.Setup(s => s.GetAimTrainerMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Normal)).ReturnsAsync(AimTrainer_Played_Normal);
            _mockAccountScoreService.Setup(s => s.GetAimTrainerMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Hard)).ReturnsAsync(AimTrainer_Played_Hard);
            _mockAccountScoreService.Setup(s => s.GetAimTrainerHighscoreAsync(It.IsAny<string>(), GameDifficulty.Normal)).ReturnsAsync(AimTrainer_Highscore_Normal);
            _mockAccountScoreService.Setup(s => s.GetAimTrainerHighscoreAsync(It.IsAny<string>(), GameDifficulty.Hard)).ReturnsAsync(AimTrainer_Highscore_Hard);
            _mockAccountScoreService.Setup(s => s.GetAimTrainerAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Normal)).ReturnsAsync(AimTrainer_AllTimeAverage_Normal);
            _mockAccountScoreService.Setup(s => s.GetAimTrainerAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Hard)).ReturnsAsync(AimTrainer_AllTimeAverage_Hard);
            _mockAccountScoreService.Setup(s => s.GetAimTrainerAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Normal)).ReturnsAsync(AimTrainer_Average_Last7Days_Normal);
            _mockAccountScoreService.Setup(s => s.GetAimTrainerAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Hard)).ReturnsAsync(AimTrainer_Average_Last7Days_Hard);
        }

        private void PairUpSetup()
        {
            PairUpScores = new List<UserScoreDto<PairUpData>> {
                new UserScoreDto<PairUpData>
                {
                    Username = username,
                    GameData = new PairUpData
                    {
                        TimeInSeconds = 20,
                        Difficulty = GameDifficulty.Easy,
                        Fails = 5
                    },
                    Timestamp = DateTime.Now
                }
            };
            PairUp_Played_Easy = 10;
            PairUp_Played_Normal = 12;
            PairUp_Played_Hard = 8;
            PairUp_Highscore_Easy = new GameScore { Scores = 3, TimeSpent = 25 };
            PairUp_Highscore_Normal = new GameScore { Scores = 8, TimeSpent = 45 };
            PairUp_Highscore_Hard = new GameScore { Scores = 12, TimeSpent = 67 };
            PairUp_AllTimeAverage_Easy = new GameScore { Scores = 4, TimeSpent = 30 };
            PairUp_AllTimeAverage_Normal = new GameScore { Scores = 10, TimeSpent = 55 };
            PairUp_AllTimeAverage_Hard = new GameScore { Scores = 16, TimeSpent = 79 };
            PairUp_Average_Last7Days_Easy = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { Scores = 4, TimeSpent = 28 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { Scores = 5, TimeSpent = 27 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { Scores = 6, TimeSpent = 26 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { Scores = 7, TimeSpent = 25 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { Scores = 8, TimeSpent = 24 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { Scores = 9, TimeSpent = 23 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { Scores = 10, TimeSpent = 22 }, Date = DateTime.Today }
            };

            PairUp_Average_Last7Days_Normal = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { Scores = 14, TimeSpent = 18 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { Scores = 15, TimeSpent = 17 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { Scores = 16, TimeSpent = 16 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { Scores = 17, TimeSpent = 15 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { Scores = 18, TimeSpent = 14 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { Scores = 19, TimeSpent = 13 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { Scores = 20, TimeSpent = 12 }, Date = DateTime.Today }
            };

            PairUp_Average_Last7Days_Hard = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { Scores = 24, TimeSpent = 8 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { Scores = 25, TimeSpent = 7 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { Scores = 26, TimeSpent = 6 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { Scores = 27, TimeSpent = 5 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { Scores = 28, TimeSpent = 4 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { Scores = 29, TimeSpent = 3 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { Scores = 30, TimeSpent = 2 }, Date = DateTime.Today }
            };

            _mockAccountScoreService.Setup(s => s.GetPairUpScoresAsync(It.IsAny<string>())).ReturnsAsync(PairUpScores);
            _mockAccountScoreService.Setup(s => s.GetPairUpMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Easy)).ReturnsAsync(PairUp_Played_Easy);
            _mockAccountScoreService.Setup(s => s.GetPairUpMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Normal)).ReturnsAsync(PairUp_Played_Normal);
            _mockAccountScoreService.Setup(s => s.GetPairUpMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Hard)).ReturnsAsync(PairUp_Played_Hard);
            _mockAccountScoreService.Setup(s => s.GetPairUpHighscoreAsync(It.IsAny<string>(), GameDifficulty.Easy)).ReturnsAsync(PairUp_Highscore_Easy);
            _mockAccountScoreService.Setup(s => s.GetPairUpHighscoreAsync(It.IsAny<string>(), GameDifficulty.Normal)).ReturnsAsync(PairUp_Highscore_Normal);
            _mockAccountScoreService.Setup(s => s.GetPairUpHighscoreAsync(It.IsAny<string>(), GameDifficulty.Hard)).ReturnsAsync(PairUp_Highscore_Hard);
            _mockAccountScoreService.Setup(s => s.GetPairUpAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Easy)).ReturnsAsync(PairUp_AllTimeAverage_Easy);
            _mockAccountScoreService.Setup(s => s.GetPairUpAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Normal)).ReturnsAsync(PairUp_AllTimeAverage_Normal);
            _mockAccountScoreService.Setup(s => s.GetPairUpAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Hard)).ReturnsAsync(PairUp_AllTimeAverage_Hard);
            _mockAccountScoreService.Setup(s => s.GetPairUpAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Easy)).ReturnsAsync(PairUp_Average_Last7Days_Easy);
            _mockAccountScoreService.Setup(s => s.GetPairUpAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Normal)).ReturnsAsync(PairUp_Average_Last7Days_Normal);
            _mockAccountScoreService.Setup(s => s.GetPairUpAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Hard)).ReturnsAsync(PairUp_Average_Last7Days_Hard);
        }

        private void SudokuSetup()
        {
            SudokuScores = new List<UserScoreDto<SudokuData>> {
                new UserScoreDto<SudokuData>
                {
                    Username = username,
                    GameData = new SudokuData
                    {
                        TimeInSeconds = 300,
                        Difficulty = GameDifficulty.Easy,
                        Mode = GameMode.NineByNine
                    },
                    Timestamp = DateTime.Now
                }
            };

            Sudoku_Played_Easy_4x4 = 10;
            Sudoku_Played_Normal_4x4 = 8;
            Sudoku_Played_Hard_4x4 = 5;
            Sudoku_Played_Easy_9x9 = 12;
            Sudoku_Played_Normal_9x9 = 9;
            Sudoku_Played_Hard_9x9 = 6;
            Sudoku_Played_Easy_16x16 = 7;
            Sudoku_Played_Normal_16x16 = 4;
            Sudoku_Played_Hard_16x16 = 3;

            Sudoku_Highscore_Easy_4x4 = new GameScore { TimeSpent = 200 };
            Sudoku_Highscore_Normal_4x4 = new GameScore { TimeSpent = 250 };
            Sudoku_Highscore_Hard_4x4 = new GameScore { TimeSpent = 300 };
            Sudoku_Highscore_Easy_9x9 = new GameScore { TimeSpent = 220 };
            Sudoku_Highscore_Normal_9x9 = new GameScore { TimeSpent = 270 };
            Sudoku_Highscore_Hard_9x9 = new GameScore { TimeSpent = 320 };
            Sudoku_Highscore_Easy_16x16 = new GameScore { TimeSpent = 240 };
            Sudoku_Highscore_Normal_16x16 = new GameScore { TimeSpent = 290 };
            Sudoku_Highscore_Hard_16x16 = new GameScore { TimeSpent = 340 };

            Sudoku_AllTimeAverage_Easy_4x4 = new GameScore { TimeSpent = 210 };
            Sudoku_AllTimeAverage_Normal_4x4 = new GameScore { TimeSpent = 260 };
            Sudoku_AllTimeAverage_Hard_4x4 = new GameScore { TimeSpent = 310 };
            Sudoku_AllTimeAverage_Easy_9x9 = new GameScore { TimeSpent = 230 };
            Sudoku_AllTimeAverage_Normal_9x9 = new GameScore { TimeSpent = 280 };
            Sudoku_AllTimeAverage_Hard_9x9 = new GameScore { TimeSpent = 330 };
            Sudoku_AllTimeAverage_Easy_16x16 = new GameScore { TimeSpent = 250 };
            Sudoku_AllTimeAverage_Normal_16x16 = new GameScore { TimeSpent = 300 };
            Sudoku_AllTimeAverage_Hard_16x16 = new GameScore { TimeSpent = 350 };

            Sudoku_Average_Last7Days_Easy_4x4 = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { TimeSpent = 210 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 220 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 230 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 240 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 250 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 260 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 270 }, Date = DateTime.Today }
            };

            Sudoku_Average_Last7Days_Normal_4x4 = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { TimeSpent = 260 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 270 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 280 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 290 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 300 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 310 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 320 }, Date = DateTime.Today }
            };

            Sudoku_Average_Last7Days_Hard_4x4 = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { TimeSpent = 310 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 320 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 330 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 340 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 350 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 360 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 370 }, Date = DateTime.Today }
            };

            Sudoku_Average_Last7Days_Easy_9x9 = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { TimeSpent = 230 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 240 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 250 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 260 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 270 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 280 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 290 }, Date = DateTime.Today }
            };

            Sudoku_Average_Last7Days_Normal_9x9 = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { TimeSpent = 280 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 290 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 300 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 310 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 320 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 330 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 340 }, Date = DateTime.Today }
            };

            Sudoku_Average_Last7Days_Hard_9x9 = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { TimeSpent = 330 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 340 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 350 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 360 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 370 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 380 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 390 }, Date = DateTime.Today }
            };

            Sudoku_Average_Last7Days_Easy_16x16 = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { TimeSpent = 250 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 260 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 270 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 280 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 290 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 300 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 310 }, Date = DateTime.Today }
            };

            Sudoku_Average_Last7Days_Normal_16x16 = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { TimeSpent = 300 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 310 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 320 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 330 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 340 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 350 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 360 }, Date = DateTime.Today }
            };

            Sudoku_Average_Last7Days_Hard_16x16 = new List<AverageScoreDto>
            {
                new AverageScoreDto { Score = new GameScore { TimeSpent = 350 }, Date = DateTime.Today.AddDays(-6) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 360 }, Date = DateTime.Today.AddDays(-5) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 370 }, Date = DateTime.Today.AddDays(-4) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 380 }, Date = DateTime.Today.AddDays(-3) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 390 }, Date = DateTime.Today.AddDays(-2) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 400 }, Date = DateTime.Today.AddDays(-1) },
                new AverageScoreDto { Score = new GameScore { TimeSpent = 410 }, Date = DateTime.Today }
            };

            _mockAccountScoreService.Setup(s => s.GetSudokuScoresAsync(It.IsAny<string>())).ReturnsAsync(SudokuScores);
            _mockAccountScoreService.Setup(s => s.GetSudokuMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.FourByFour)).ReturnsAsync(Sudoku_Played_Easy_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.FourByFour)).ReturnsAsync(Sudoku_Played_Normal_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.FourByFour)).ReturnsAsync(Sudoku_Played_Hard_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.NineByNine)).ReturnsAsync(Sudoku_Played_Easy_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.NineByNine)).ReturnsAsync(Sudoku_Played_Normal_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.NineByNine)).ReturnsAsync(Sudoku_Played_Hard_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_Played_Easy_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_Played_Normal_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuMatchesPlayedAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_Played_Hard_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuHighscoreAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.FourByFour)).ReturnsAsync(Sudoku_Highscore_Easy_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuHighscoreAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.FourByFour)).ReturnsAsync(Sudoku_Highscore_Normal_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuHighscoreAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.FourByFour)).ReturnsAsync(Sudoku_Highscore_Hard_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuHighscoreAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.NineByNine)).ReturnsAsync(Sudoku_Highscore_Easy_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuHighscoreAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.NineByNine)).ReturnsAsync(Sudoku_Highscore_Normal_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuHighscoreAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.NineByNine)).ReturnsAsync(Sudoku_Highscore_Hard_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuHighscoreAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_Highscore_Easy_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuHighscoreAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_Highscore_Normal_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuHighscoreAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_Highscore_Hard_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.FourByFour)).ReturnsAsync(Sudoku_AllTimeAverage_Easy_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.FourByFour)).ReturnsAsync(Sudoku_AllTimeAverage_Normal_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.FourByFour)).ReturnsAsync(Sudoku_AllTimeAverage_Hard_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.NineByNine)).ReturnsAsync(Sudoku_AllTimeAverage_Easy_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.NineByNine)).ReturnsAsync(Sudoku_AllTimeAverage_Normal_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.NineByNine)).ReturnsAsync(Sudoku_AllTimeAverage_Hard_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_AllTimeAverage_Easy_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_AllTimeAverage_Normal_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_AllTimeAverage_Hard_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.FourByFour)).ReturnsAsync(Sudoku_Average_Last7Days_Easy_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.FourByFour)).ReturnsAsync(Sudoku_Average_Last7Days_Normal_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.FourByFour)).ReturnsAsync(Sudoku_Average_Last7Days_Hard_4x4);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.NineByNine)).ReturnsAsync(Sudoku_Average_Last7Days_Easy_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.NineByNine)).ReturnsAsync(Sudoku_Average_Last7Days_Normal_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.NineByNine)).ReturnsAsync(Sudoku_Average_Last7Days_Hard_9x9);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Easy, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_Average_Last7Days_Easy_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Normal, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_Average_Last7Days_Normal_16x16);
            _mockAccountScoreService.Setup(s => s.GetSudokuAverageScoreLast7DaysAsync(It.IsAny<string>(), GameDifficulty.Hard, GameMode.SixteenBySixteen)).ReturnsAsync(Sudoku_Average_Last7Days_Hard_16x16);
        }
    }
}
