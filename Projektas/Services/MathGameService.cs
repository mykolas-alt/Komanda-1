using System.Formats.Asn1;
using System.Text;

namespace Projektas.Services
{
    public enum Operation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public class MathGameService
    {
        private readonly Random _random = new();

        public int Score { get; set; } = 0;

        public (string question, List<int> numbers, List<Operation> operations) GenerateQuestion()
        {
            // increase difficulty based on the current score
            int maxNumber = 10 + (Score * 10); // increases the range of numbers as the score increases
            int maxOperands = (int)(3 + (Score * 0.1)); // increases the range of operands as the score inscreases
            int numberOfOperands = _random.Next(2, maxOperands);

            // generate numbers and operations
            List<int> numbers = GenerateNumbers(numberOfOperands, maxNumber);
            List<Operation> operations = GenerateOperations(numberOfOperands);

            // adjust numbers by operation (division or multiplication)
            AdjustNumbersForOperations(numbers, operations, maxNumber);

            return (BuildQuestion(numbers,operations), numbers, operations);
        }

        // generates numbers and adds to the list
        private List<int> GenerateNumbers(int numberOfOperands, int maxNumber)
        {
            List<int> numbers = [];
            for (int i = 0; i < numberOfOperands; i++)
            {
                numbers.Add(GenerateNumber(maxNumber));
            }
            return numbers;
        }

        // generates operations and adds to the list
        private List<Operation> GenerateOperations(int numberOfOperands)
        {
            List<Operation> operations = [];
            Operation[] possibleOperations = { Operation.Addition, Operation.Subtraction, Operation.Multiplication, Operation.Division };

            for (int i = 0; i < numberOfOperands - 1; i++)
            {
                operations.Add(possibleOperations[_random.Next(possibleOperations.Length)]);
            }

            return operations;
        }

        // adjusts numbers by operations
        private void AdjustNumbersForOperations(List<int> numbers, List<Operation> operations, int maxNumber)
        {
            for (int i = 1; i < numbers.Count; i++)
            {
                if (operations[i - 1] == Operation.Division)
                {
                    AdjustForDivision(numbers, i, maxNumber);
                }
                else if (operations[i - 1] == Operation.Multiplication)
                {
                    numbers[i] = _random.Next(2, 10 + (Score / 2));
                }
            }
        }

        private void AdjustForDivision(List<int> numbers, int index, int maxNumber)
        {
            // checks division by 0 and if number is not 1
            // then if 1st number is bigger than 2nd or if they are divisable
            while (numbers[index] == 0 || numbers[index] == 1 || numbers[index - 1] < numbers[index] || numbers[index - 1] % numbers[index] != 0)
            {
                numbers[index] = GenerateNumber(maxNumber);
            }
        }

        // generates the string of the quesrion
        private static string BuildQuestion(List<int> numbers, List<Operation> operations)
        {
            StringBuilder questionBuilder = new();
            questionBuilder.Append(numbers[0]);
            for (int i = 0; i < operations.Count; i++)
            {
                questionBuilder.Append($" {GetOperationSymbol(operations[i])} {numbers[i + 1] }");
            }
            return questionBuilder.ToString();
        }

        // calculates the result of the generated question
        private static int CalculateAnswer(List<int> numbers, List<Operation> operations)
        {
            // handles multiplication and division
            List<int> processedNumbers = new(numbers);
            List<Operation> processedOperations = new(operations);

            HandleMultiplicationAndDivision(processedNumbers, processedOperations);

            // handles addition and subtraction
            int finalResult = HandleAdditionAndSubtraction(processedNumbers, processedOperations);

            return finalResult;
        }

        // handles multiplication and division
        private static void HandleMultiplicationAndDivision(List<int> numbers, List<Operation> operations)
        {
            for (int i = 0; i < operations.Count; i++)
            {
                if (operations[i] == Operation.Multiplication || operations[i] == Operation.Division)
                {
                    int left = numbers[i];
                    int right = numbers[i + 1];
                    int result = PerformOperation(left, right, operations[i]);

                    // replaces the left number with the result and removes the right number and the operation
                    numbers[i] = result;
                    numbers.RemoveAt(i + 1);
                    operations.RemoveAt(i);
                    i--; // adjust index
                }
            }
        }

        // handles addition and subtraction
        private static int HandleAdditionAndSubtraction(List<int> numbers, List<Operation> operations)
        {
            int result = numbers[0];
            for (int i = 0; i < operations.Count; i++)
            {
                int right = numbers[i + 1];
                result = PerformOperation(result, right, operations[i]);
            }
            return result;
        }

        // performs calculations by operation
        private static int PerformOperation(int left, int right, Operation operation)
        {
            switch (operation)
            {
                case Operation.Multiplication:
                    return left * right;
                case Operation.Division:
                    return left / right;
                case Operation.Addition:
                    return left + right;
                case Operation.Subtraction:
                    return left - right;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }

        // checks answer and returns true or false
        public bool CheckAnswer(int option, List<int> numbers, List<Operation> operations)
        {
            int finalResult = CalculateAnswer(numbers, operations);
            bool isCorrect = finalResult == option;

            // if correct, score is incremented
            if (isCorrect)
            {
                Score++;
            }

            return isCorrect;
        }

        // generates a random number
        private int GenerateNumber(int maxNumber)
        {
            int number = _random.Next(1, maxNumber);

            return number;
        }

        // returns a symbol of an operation
        private static string GetOperationSymbol(Operation operation)
        {
            switch (operation)
            {
                case Operation.Addition:
                    return "+";
                case Operation.Subtraction:
                    return "-";
                case Operation.Multiplication:
                    return "*";
                case Operation.Division:
                    return "/";
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), operation, null);
            }
        }
        // generates four options, including the correct answer
        public List<int> GenerateOptions(List<int> numbers, List<Operation> operations)
        {
            int answer = CalculateAnswer(numbers, operations);
            int option1 = _random.Next(answer - 100, answer + 100);
            int option2 = _random.Next(answer - 100, answer + 100);
            int option3 = _random.Next(answer - 100, answer + 100);
            var options = new List<int> { option1, option2, option3, answer };
            return options.OrderBy(x => Guid.NewGuid()).ToList(); // shuffles options
        }
    }
}