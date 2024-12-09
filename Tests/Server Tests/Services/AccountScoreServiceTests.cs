using Moq;
using Projektas.Server.Interfaces;
using Projektas.Server.Services;
using Projektas.Shared.Enums;
using Projektas.Shared.Models;


namespace Projektas.Tests
{
    public class AccountScoreServiceTests
    {
        private readonly Mock<IScoreRepository> _repositoryMock;
        private readonly AccountScoreService _service;

        public AccountScoreServiceTests()
        {
            _repositoryMock = new Mock<IScoreRepository>();
            _service = new AccountScoreService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetUserScores_ShouldReturnLast10Scores()
        {
            var user = new User { Username = "testuser" };
            var scores = new List<UserScoreDto<MathGameData>>
            {
                new UserScoreDto<MathGameData> { Username = "testuser", Timestamp = DateTime.Now, GameData = new MathGameData { Scores = 10 } },
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<MathGameData>()).ReturnsAsync(scores);

            var result = await _service.GetUserScores<MathGameData>(user);

            Assert.Single(result);
        }

        [Fact]
        public async Task GetHighscore_ShouldReturnCorrectHighscore()
        {
            var user = new User { Username = "testuser" };
            var scores = new List<UserScoreDto<MathGameData>>
            {
                new UserScoreDto<MathGameData> { Username = "testuser", GameData = new MathGameData { Scores = 100 } },
                new UserScoreDto<MathGameData> { Username = "testuser", GameData = new MathGameData { Scores = 200 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<MathGameData>()).ReturnsAsync(scores);

            var result = await _service.GetHighscore<MathGameData>(user);

            Assert.Equal(200, result.Scores);
        }

        [Fact]
        public async Task GetHighscore_ShouldReturnZeroWhenNoScores()
        {
            var user = new User { Username = "testuser" };
            var scores = new List<UserScoreDto<MathGameData>>();
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<MathGameData>()).ReturnsAsync(scores);

            var result = await _service.GetHighscore<MathGameData>(user);

            Assert.Equal(0, result.Scores);
        }

        [Fact]
        public async Task GetHighscore_WithDifficulty_ShouldReturnCorrectHighscore()
        {
            var user = new User { Username = "testuser" };
            var difficulty = GameDifficulty.Medium;
            var scores = new List<UserScoreDto<PairUpData>>
            {
                new UserScoreDto<PairUpData> { Username = "testuser", GameData = new PairUpData { Difficulty = difficulty, Fails = 2, TimeInSeconds = 30 } },
                new UserScoreDto<PairUpData> { Username = "testuser", GameData = new PairUpData { Difficulty = difficulty, Fails = 4, TimeInSeconds = 40 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<PairUpData>()).ReturnsAsync(scores);

            var result = await _service.GetHighscore<PairUpData>(user, difficulty);

            Assert.Equal(2, result.Scores);
            Assert.Equal(30, result.TimeSpent);
        }

        [Fact]
        public async Task GetHighscore_WithDifficultyAndMode_ShouldReturnCorrectHighscore()
        {
            var user = new User { Username = "testuser" };
            var difficulty = GameDifficulty.Hard;
            var mode = GameMode.FourByFour;
            var scores = new List<UserScoreDto<SudokuData>>
            {
                new UserScoreDto<SudokuData> { Username = "testuser", GameData = new SudokuData { Difficulty = difficulty, Mode = mode, TimeInSeconds = 300 } },
                new UserScoreDto<SudokuData> { Username = "testuser", GameData = new SudokuData { Difficulty = difficulty, Mode = mode, TimeInSeconds = 400 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<SudokuData>()).ReturnsAsync(scores);

            var result = await _service.GetHighscore<SudokuData>(user, difficulty, mode);

            Assert.Equal(300, result.TimeSpent);
        }

        [Fact]
        public async Task GetAllTimeAverageScore_ShouldReturnCorrectAverageScore()
        {
            var user = new User { Username = "testuser" };
            var scores = new List<UserScoreDto<MathGameData>>
            {
                new UserScoreDto<MathGameData> { Username = "testuser", GameData = new MathGameData { Scores = 100 } },
                new UserScoreDto<MathGameData> { Username = "testuser", GameData = new MathGameData { Scores = 200 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<MathGameData>()).ReturnsAsync(scores);

            var result = await _service.GetAllTimeAverageScore<MathGameData>(user);

            Assert.Equal(150, result.Scores);
        }

        [Fact]
        public async Task GetAllTimeAverageScore_WithDifficulty_ShouldReturnCorrectAverageScore()
        {
            var user = new User { Username = "testuser" };
            var difficulty = GameDifficulty.Medium;
            var scores = new List<UserScoreDto<PairUpData>>
            {
                new UserScoreDto<PairUpData> { Username = "testuser", GameData = new PairUpData { Difficulty = difficulty, Fails = 2, TimeInSeconds = 30 } },
                new UserScoreDto<PairUpData> { Username = "testuser", GameData = new PairUpData { Difficulty = difficulty, Fails = 4, TimeInSeconds = 40 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<PairUpData>()).ReturnsAsync(scores);

            var result = await _service.GetAllTimeAverageScore<PairUpData>(user, difficulty);

            Assert.Equal(3, result.Scores);
            Assert.Equal(35, result.TimeSpent);
        }

        [Fact]
        public async Task GetAllTimeAverageScore_WithDifficultyAndMode_ShouldReturnCorrectAverageScore()
        {
            var user = new User { Username = "testuser" };
            var difficulty = GameDifficulty.Hard;
            var mode = GameMode.FourByFour;
            var scores = new List<UserScoreDto<SudokuData>>
            {
                new UserScoreDto<SudokuData> { Username = "testuser", GameData = new SudokuData { Difficulty = difficulty, Mode = mode, TimeInSeconds = 300 } },
                new UserScoreDto<SudokuData> { Username = "testuser", GameData = new SudokuData { Difficulty = difficulty, Mode = mode, TimeInSeconds = 400 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<SudokuData>()).ReturnsAsync(scores);

            var result = await _service.GetAllTimeAverageScore<SudokuData>(user, difficulty, mode);

            Assert.Equal(350, result.TimeSpent);
        }

        [Fact]
        public async Task GetMatchesPlayed_ShouldReturnCorrectCount()
        {
            var user = new User { Username = "testuser" };
            var scores = new List<UserScoreDto<MathGameData>>
            {
                new UserScoreDto<MathGameData> { Username = "testuser" },
                new UserScoreDto<MathGameData> { Username = "testuser" }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<MathGameData>()).ReturnsAsync(scores);

            var result = await _service.GetMatchesPlayed<MathGameData>(user);

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetMatchesPlayed_WithDifficulty_ShouldReturnCorrectCount()
        {
            var user = new User { Username = "testuser" };
            var difficulty = GameDifficulty.Medium;
            var scores = new List<UserScoreDto<PairUpData>>
            {
                new UserScoreDto<PairUpData> { Username = "testuser", GameData = new PairUpData { Difficulty = difficulty } },
                new UserScoreDto<PairUpData> { Username = "testuser", GameData = new PairUpData { Difficulty = difficulty } },
                new UserScoreDto<PairUpData> { Username = "testuser", GameData = new PairUpData { Difficulty = GameDifficulty.Hard } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<PairUpData>()).ReturnsAsync(scores);

            var result = await _service.GetMatchesPlayed<PairUpData>(user, difficulty);

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetMatchesPlayed_WithDifficultyAndMode_ShouldReturnCorrectCount()
        {
            var user = new User { Username = "testuser" };
            var difficulty = GameDifficulty.Hard;
            var mode = GameMode.FourByFour;
            var scores = new List<UserScoreDto<SudokuData>>
            {
                new UserScoreDto<SudokuData> { Username = "testuser", GameData = new SudokuData { Difficulty = difficulty, Mode = mode } },
                new UserScoreDto<SudokuData> { Username = "testuser", GameData = new SudokuData { Difficulty = difficulty, Mode = mode } },
                new UserScoreDto<SudokuData> { Username = "testuser", GameData = new SudokuData { Difficulty = difficulty, Mode = GameMode.SixteenBySixteen } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<SudokuData>()).ReturnsAsync(scores);

            var result = await _service.GetMatchesPlayed<SudokuData>(user, difficulty, mode);

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetAllTimeAverageScore_ShouldReturnAverageScore()
        {
            var user = new User { Username = "testuser" };
            var scores = new List<UserScoreDto<MathGameData>>
            {
                new UserScoreDto<MathGameData> { Username = "testuser", GameData = new MathGameData { Scores = 10 } },
                new UserScoreDto<MathGameData> { Username = "testuser", GameData = new MathGameData { Scores = 20 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<MathGameData>()).ReturnsAsync(scores);

            var result = await _service.GetAllTimeAverageScore<MathGameData>(user);

            Assert.Equal(15, result.Scores);
        }

        [Fact]
        public async Task GetAverageScoreLast7Days_ShouldReturnCorrectAverageScores()
        {
            var user = new User { Username = "testuser" };
            var scores = new List<UserScoreDto<MathGameData>>
            {
                new UserScoreDto<MathGameData> { Username = "testuser", Timestamp = DateTime.Today, GameData = new MathGameData { Scores = 100 } },
                new UserScoreDto<MathGameData> { Username = "testuser", Timestamp = DateTime.Today.AddDays(-1), GameData = new MathGameData { Scores = 200 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<MathGameData>()).ReturnsAsync(scores);

            var result = await _service.GetAverageScoreLast7Days<MathGameData>(user);

            Assert.Equal(7, result.Count);
            Assert.Equal(100, result[6].Score.Scores);
            Assert.Equal(200, result[5].Score.Scores);
        }

        [Fact]
        public async Task GetAverageScoreLast7Days_WithDifficulty_ShouldReturnCorrectAverageScores()
        {
            var user = new User { Username = "testuser" };
            var difficulty = GameDifficulty.Medium;
            var scores = new List<UserScoreDto<PairUpData>>
            {
                new UserScoreDto<PairUpData> { Username = "testuser", Timestamp = DateTime.Today, GameData = new PairUpData { Difficulty = difficulty, Fails = 2, TimeInSeconds = 30 } },
                new UserScoreDto<PairUpData> { Username = "testuser", Timestamp = DateTime.Today.AddDays(-1), GameData = new PairUpData { Difficulty = difficulty, Fails = 4, TimeInSeconds = 40 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<PairUpData>()).ReturnsAsync(scores);

            var result = await _service.GetAverageScoreLast7Days<PairUpData>(user, difficulty);

            Assert.Equal(7, result.Count);
            Assert.Equal(2, result[6].Score.Scores);
            Assert.Equal(30, result[6].Score.TimeSpent);
            Assert.Equal(4, result[5].Score.Scores);
            Assert.Equal(40, result[5].Score.TimeSpent);
        }

        [Fact]
        public async Task GetAverageScoreLast7Days_WithDifficultyAndMode_ShouldReturnCorrectAverageScores()
        {
            var user = new User { Username = "testuser" };
            var difficulty = GameDifficulty.Hard;
            var mode = GameMode.NineByNine;
            var scores = new List<UserScoreDto<SudokuData>>
            {
                new UserScoreDto<SudokuData> { Username = "testuser", Timestamp = DateTime.Today, GameData = new SudokuData { Difficulty = difficulty, Mode = mode, TimeInSeconds = 300 } },
                new UserScoreDto<SudokuData> { Username = "testuser", Timestamp = DateTime.Today.AddDays(-1), GameData = new SudokuData { Difficulty = difficulty, Mode = mode, TimeInSeconds = 400 } }
            };
            _repositoryMock.Setup(repo => repo.GetAllScoresAsync<SudokuData>()).ReturnsAsync(scores);

            var result = await _service.GetAverageScoreLast7Days<SudokuData>(user, difficulty, mode);

            Assert.Equal(7, result.Count);
            Assert.Equal(300, result[6].Score.TimeSpent);
            Assert.Equal(400, result[5].Score.TimeSpent);
        }
    }
}