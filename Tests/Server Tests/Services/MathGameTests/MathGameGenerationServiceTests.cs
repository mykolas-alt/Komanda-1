using Projektas.Server.Services.MathGame;
using Projektas.Server.Enums;

namespace Projektas.Tests.Services.MathGameTests {
    public class MathGameGenerationServiceTests {
        private readonly MathGenerationService _mathGenerationService;

        public MathGameGenerationServiceTests() {
            _mathGenerationService=new MathGenerationService();
        }

        [Theory]
        [InlineData(2,2)]
        [InlineData(5,5)]
        [InlineData(20,20)]
        public void GenerateNumbers_ShouldReturnCorrectNumberOfOperands(int numberOfOperands,int expectedResult) {
            const int arbitraryScore=1; // score doesn't matter in this case

            List<int> result=_mathGenerationService.GenerateNumbers(numberOfOperands,arbitraryScore);

            Assert.Equal(expectedResult,result.Count);
        }

        [Theory]
        [InlineData(3,0,1,10)]
        [InlineData(5,2,3,14)]
        [InlineData(10,15,16,40)]
        public void GenerateNumbers_ShouldReturnNumbersWithinRange(int numberOfOperands,int score,int min,int max) {
            List<int> result=_mathGenerationService.GenerateNumbers(numberOfOperands,score);

            foreach(int number in result) {
                Assert.InRange(number,min,max);
            }
        }

        [Theory]
        [InlineData(2,1)]
        [InlineData(5,4)]
        [InlineData(20,19)]
        public void GenerateOperations_ShouldReturnCorrectNumberOfOperations(int numberOfOperands,int expectedResult) {
            const int arbitraryScore=1; // score doesn't matter in this case

            List<Operation> result=_mathGenerationService.GenerateOperations(numberOfOperands,arbitraryScore);

            // generates numberOfOperands - 1 operations
            Assert.Equal(expectedResult,result.Count);
        }

        [Theory]
        [InlineData(3,0)]
        [InlineData(5,4)]
        [InlineData(20,5)]
        public void GenerateOperations_ReturnsAdditionOrSubtraction_WhenScoreIsFiveOrBelow(int numberOfOperands,int score) {
            List<Operation> result=_mathGenerationService.GenerateOperations(numberOfOperands,score);

            foreach(Operation operation in result) {
                Assert.True(operation==Operation.Addition || operation==Operation.Subtraction);
            }
        }
        [Theory]
        [InlineData(3,6)]
        [InlineData(5,10)]
        [InlineData(20,12)]
        public void GenerateOperations_ReturnsAdditionOrSubtraction_WhenScoreIsAboveFive(int numberOfOperands,int score) {
            List<Operation> result=_mathGenerationService.GenerateOperations(numberOfOperands,score);

            foreach(Operation operation in result) {
                Assert.True(operation==Operation.Addition || operation==Operation.Subtraction || operation==Operation.Multiplication || operation==Operation.Division);
            }
        }

        [Theory]
        // Division
        [InlineData(1,new int[] {15,4},new Operation[] {Operation.Division},new int[] {15,5,3})]
        [InlineData(1,new int[] {17,24,7},new Operation[] {Operation.Addition,Operation.Division},new int[] {24,12,8,6,4,3,2})]

        // Multiplication
        [InlineData(10,new int[] {20,6},new Operation[] {Operation.Multiplication},new int[] {2,3,4,5})]
        [InlineData(20,new int[] {100,12,18},new Operation[] {Operation.Subtraction,Operation.Multiplication},new int[] {2,3,4,5,6,7,8,9,10})]
        public void AdjustNumbersForOperations_ShouldAdjustNumbersCorrectly (int score,int[] initialNumbers,Operation[] operations,int[] adjustedNumberVariants) {
            List<int> initialNumbersList=new List<int>(initialNumbers);
            List<Operation> operationsList=new List<Operation>(operations);

            _mathGenerationService.AdjustNumbersForOperations(score,initialNumbersList,operationsList);

            for(int i=1;i<initialNumbersList.Count;i++) {
                if(operationsList[i-1]==Operation.Division || operationsList[i-1]==Operation.Multiplication) {
                    Assert.Contains(initialNumbersList[i],adjustedNumberVariants);
                }
            }
        }
    }
}
