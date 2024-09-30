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

        private int Answer;
        public int Score { get; set; } = 0;
        public int Lives { get; set; } = 3;
        private List<int> numbers = [];
        private List<Operation> operations = [];
        private int MaxNumber => 10 + (Score * 10); // increases the range of numbers as the score increases
        public string GenerateQuestion()
        {
            // increase difficulty based on the current score
            
            int maxOperands = (int)(3 + (Score * 0.2)); // increases the range of operands as the score inscreases
            int numberOfOperands = _random.Next(2, maxOperands);

            // generate numbers and operations
            numbers = GenerateNumbers(numberOfOperands);
            operations = GenerateOperations(numberOfOperands);

            // adjust numbers by operation (division or multiplication)
            AdjustNumbersForOperations();

            Answer = CalculateAnswer();

            return BuildQuestion();
        }

        // generates numbers and adds to the list
        private List<int> GenerateNumbers(int numberOfOperands)
        {
            List<int> numbers = [];
            for (int i = 0; i < numberOfOperands; i++)
            {
                numbers.Add(GenerateNumber());
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
        private void AdjustNumbersForOperations()
        {
            for (int i = 1; i < numbers.Count; i++)
            {
                if (operations[i - 1] == Operation.Division)
                {
                    AdjustForDivision(index:i);
                }
                else if (operations[i - 1] == Operation.Multiplication)
                {
                    numbers[i] = _random.Next(2, 10 + (Score / 2));
                }
            }
        }

        private void AdjustForDivision(int index)
        {
            // checks division by 0 and if number is not 1
            // then if 1st number is bigger than 2nd or if they are divisable
            while (numbers[index] == 0 || numbers[index] == 1 || numbers[index - 1] < numbers[index] || numbers[index - 1] % numbers[index] != 0)
            {
                numbers[index] = GenerateNumber();
            }
        }

        // generates the string of the quesrion
        private string BuildQuestion()
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
        private int CalculateAnswer()
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
        private static void HandleMultiplicationAndDivision(List<int> processedNumbers, List<Operation> processedOperations)
        {
            for (int i = 0; i < processedOperations.Count; i++)
            {
                if (processedOperations[i] == Operation.Multiplication || processedOperations[i] == Operation.Division)
                {
                    int left = processedNumbers[i];
                    int right = processedNumbers[i + 1];
                    int result = PerformOperation(left, right, processedOperations[i]);

                    // replaces the left number with the result and removes the right number and the operation
                    processedNumbers[i] = result;
                    processedNumbers.RemoveAt(i + 1);
                    processedOperations.RemoveAt(i);
                    i--; // adjust index
                }
            }
        }

        // handles addition and subtraction
        private static int HandleAdditionAndSubtraction(List<int> processedNumbers, List<Operation> processedOperations)
        {
            int result = processedNumbers[0];
            for (int i = 0; i < processedOperations.Count; i++)
            {
                int right = processedNumbers[i + 1];
                result = PerformOperation(left:result, right, processedOperations[i]);
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
        public bool CheckAnswer(int option)
        {
            bool isCorrect = Answer == option;

            // if correct, score is incremented
            if (isCorrect)
            {
                Score++;
            }
            else
            {
                Lives--;
            }

            return isCorrect;
        }

        // generates a random number
        private int GenerateNumber()
        {
            int number = _random.Next(1, MaxNumber);

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
        // generates 4 unique options, including the correct answer
        public List<int> GenerateOptions()
        {
            HashSet<int> options = new();
            options.Add(Answer);
            while (options.Count < 4)
            {
                int option = _random.Next(Answer - MaxNumber, Answer + MaxNumber);
                options.Add(option);
            }
            return options.OrderBy(x => Guid.NewGuid()).ToList(); // shuffles options
        }
    }
}