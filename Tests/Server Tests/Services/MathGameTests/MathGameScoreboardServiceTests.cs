using Moq;
using Projektas.Server.Interfaces.MathGame;
using Projektas.Server.Services.MathGame;

namespace Projektas.Tests.Services.MathGameTests
{
    public class MathGameScoreboardServiceTests
    {
        private readonly Mock<IMathGameDataService> _mockMathDataService;
        private readonly MathGameScoreboardService _mathGameScoreBoardService;

        public MathGameScoreboardServiceTests()
        {
            _mockMathDataService = new Mock<IMathGameDataService>();
            _mathGameScoreBoardService = new MathGameScoreboardService(_mockMathDataService.Object);
        }

        [Theory]
        [InlineData(4, new int[] { }, new int[] { })]
        [InlineData(3, new int[] { 20, 7, 12, 18, 3}, new int[] {20, 18, 12})]
        [InlineData(5, new int[] { 2, 7, 4, 18, 3, 1, 6, 7 }, new int[] { 18, 7, 7, 6, 4 })]
        [InlineData(5, new int[] { 9, 9, 9, 9, 9, 9, 9, 9 }, new int[] { 9, 9, 9, 9, 9 })]
        [InlineData(10, new int[] { 16, 21, 4, 6, 9 }, new int[] { 21, 16, 9, 6, 4 })]
        public void GetTopScores_Returns_TopCountDescendingOrderedlistOfScores(int topCount, int[] numbers, int[] expectedResults)
        {
            List<int> expectedResultList = new List<int>(expectedResults);
            _mockMathDataService.Setup(m => m.LoadData()).Returns(new List<int>(numbers));

            List<int> result = _mathGameScoreBoardService.GetTopScores(topCount);

            Assert.Equal(expectedResultList, result);
        }
    }
}
