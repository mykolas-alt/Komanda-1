using Projektas.Server.Interfaces.MathGame;
using Projektas.Server.Services.MathGame;
using Projektas.Server.Enums;
using Moq;

namespace Projektas.Tests.Services.MathGameTests {
    public class MathGameServiceTests {
        private readonly Mock<IMathCalculationService> _mockMathCalculationService;
        private readonly Mock<IMathGenerationService> _mockMathGenerationService;
        private readonly MathGameService _mathGameService;

        public MathGameServiceTests() {
            _mockMathCalculationService = new Mock<IMathCalculationService>();
            _mockMathGenerationService = new Mock<IMathGenerationService>();
            _mathGameService = new MathGameService(_mockMathCalculationService.Object, _mockMathGenerationService.Object);
        }

        [Theory]
        [InlineData(10, 10, true)]
        [InlineData(15, 15, true)]
        [InlineData(9, 5, false)]
        [InlineData(17, 2, false)]
        public void CheckAnswer_ShouldReturnCorrectResult(int answer, int option, bool expectedResult) {
            _mathGameService.Answer = answer;
            bool result = _mathGameService.CheckAnswer(option);

            Assert.Equal(result, expectedResult);
        }

        [Theory]
        [InlineData(new int[] {2, 3}, new Operation[] {Operation.Addition}, "2 + 3")]
        [InlineData(new int[] {2, 3, 4}, new Operation[] {Operation.Addition, Operation.Subtraction}, "2 + 3 - 4")]
        [InlineData(new int[] {14, 2}, new Operation[] {Operation.Multiplication}, "14 * 2")]
        [InlineData(new int[] {27, 3, 3}, new Operation[] {Operation.Division, Operation.Subtraction}, "27 / 3 - 3")]
        public void GenerateQuestion_ShouldReturnValidQuestion(int[] numbers, Operation[] operations, string expectedResult) {
            _mockMathGenerationService.Setup(m => m.GenerateNumbers(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<int>(numbers));
            _mockMathGenerationService.Setup(m => m.GenerateOperations(It.IsAny<int>(), It.IsAny<int>())).Returns(new List<Operation>(operations));
            _mockMathCalculationService.Setup(m => m.CalculateAnswer(It.IsAny<List<int>>(), It.IsAny<List<Operation>>())).Returns(42); // arbitrary answer

            string question = _mathGameService.GenerateQuestion(score: 10); // score is arbitrary

            Assert.Equal(expectedResult, question);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(20)]
        public void GenerateOptions_ShouldReturnUniqueOptions(int answer) {
            _mathGameService.Answer = answer;
            
            List<int> result = _mathGameService.GenerateOptions();

            Assert.Equal(4, result.Count);
            Assert.Contains(answer, result);
            Assert.Equal(result.Count, new HashSet<int>(result).Count);
        }
    }
}
