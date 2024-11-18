using Projektas.Server.Services.MathGame;
using Projektas.Server.Enums;

namespace Projektas.Tests.Services.MathGameTests
{
    public class MathCalculationServiceTests
    {
        [Theory]
        // Addition
        [InlineData(new int[] { 10, 25 }, new Operation[] { Operation.Addition }, 35)]
        [InlineData(new int[] { -20, 25, 14, -2 }, new Operation[] { Operation.Addition, Operation.Addition, Operation.Addition }, 17)]
        // Subtraction
        [InlineData(new int[] { 6, 5 }, new Operation[] { Operation.Subtraction }, 1)]
        [InlineData(new int[] { 50, 14, 2, 20 }, new Operation[] { Operation.Subtraction, Operation.Subtraction, Operation.Subtraction }, 14)]
        // Multiplication
        [InlineData(new int[] { 9, 4 }, new Operation[] { Operation.Multiplication }, 36)]
        [InlineData(new int[] { 11, 3, 2 }, new Operation[] { Operation.Multiplication, Operation.Multiplication }, 66)]
        // Division
        [InlineData(new int[] { 49, 7 }, new Operation[] { Operation.Division }, 7)]
        [InlineData(new int[] { 105, 5, 7 }, new Operation[] { Operation.Division, Operation.Division }, 3)]
        // Mixed
        [InlineData(new int[] { 49, 7, 5, 12 }, new Operation[] { Operation.Division, Operation.Multiplication, Operation.Subtraction }, 23)]
        [InlineData(new int[] { 78, 28, 4, 3, 89 }, new Operation[] { Operation.Addition, Operation.Division, Operation.Multiplication, Operation.Subtraction }, 10)]
        public void CalculateAnswer_AdditionSubtractionMultiplicationDivision_ReturnsCorrectResult(int[] numbers, Operation[] operations, int expectedResult)
        {
            var service = new MathCalculationService();
            List<int> numberList = new List<int>(numbers);
            List<Operation> operationList = new List<Operation>(operations);

            int result = service.CalculateAnswer(numberList, operationList);

            Assert.Equal(expectedResult, result);
        }
    }
}
