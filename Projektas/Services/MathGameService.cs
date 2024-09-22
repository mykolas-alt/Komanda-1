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

        public (int, int, Operation) GenerateQuestion()
        {
            // increase difficulty based on the current score
            int maxNumber = 100 + (Score * 10); // increases the range of numbers as the score increases
            int number1 = _random.Next(1, maxNumber);
            int number2 = _random.Next(1, maxNumber);
            Operation[] operations = { Operation.Addition, Operation.Subtraction, Operation.Multiplication, Operation.Division };
            Operation operation = operations[_random.Next(operations.Length)]; // chooses a random operation

            // checks if there is no division by 0, number1 is divisible by number2, number1 is not prime and number1 >= number2
            // if true, then new numbers are generated to meet the criteria
            if (operation == Operation.Division)
            {
                while (number2 == 0 || number2 == 1 || number1 % number2 != 0 || number1 < number2 || IsPrime(number1))
                {
                    number1 = _random.Next(1, maxNumber);
                    number2 = _random.Next(1, maxNumber);
                }
            }
            else if (operation == Operation.Multiplication)
            {
                number2 = _random.Next(2, 10 + (Score / 2)); // increases the range of the second number for multiplication
            }

            return (number1, number2, operation);
        }

        // checks answer and returns true or false
        public bool CheckAnswer(int number1, int number2, Operation operation, int answer)
        {
            bool isCorrect = false;
            switch (operation)
            {
                case Operation.Addition:
                    isCorrect = number1 + number2 == answer;
                    break;
                case Operation.Subtraction:
                    isCorrect = number1 - number2 == answer;
                    break;
                case Operation.Multiplication:
                    isCorrect = number1 * number2 == answer;
                    break;
                case Operation.Division:
                    isCorrect = number1 / number2 == answer;
                    break;
            }

            // if correct, score is incremented
            if (isCorrect)
            {
                Score++;
            }
            return isCorrect;
        }

        // checks if the number is prime and returns true or false
        private static bool IsPrime(int number)
        {
            if (number <= 1)
                return false;
            if (number == 2)
                return true;
            if (number % 2 == 0)
                return false;
            for (int i = 3; i <= Math.Sqrt(number); i += 2)
            {
                if (number % i == 0)
                    return false;
            }
            return true;
        }
    }
}