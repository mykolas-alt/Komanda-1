namespace Projektas.Server.Services.MathGame
{
    public class MathGameService
    {
        private readonly MathQuestion Question;

        public int Score { get; set; } = 0;
        public int Highscore { get; set; }

        public MathGameService()
        {
            MathCalculations mathCalculations = new MathCalculations();
            Question = new MathQuestion(this, mathCalculations);
        }

        public string GenerateQuestion()
        {
            return Question.GenerateQuestion();
        }

        // checks answer and returns true or false
        public bool CheckAnswer(int option)
        {
            bool isCorrect = Question.Answer == option;
            // if correct, score is incremented
            if (isCorrect)
            {
                Score++;
                if (Score > Highscore)
                {
                    Highscore = Score;
                }
            }
            return isCorrect;
        }
        public List<int> GenerateOptions()
        {
            return Question.GenerateOptions();
        }
    }
}