namespace Projektas.Components.Pages
{
    using Projektas.Services;
    using System.Numerics;

    public partial class MathGame
    {
        private string? question;
        private List<int>? numbers;
        private List<Operation>? operations;
        private string userAnswer = string.Empty;
        private string? result;
        private bool isSubmitted = false;

        protected override void OnInitialized()
        {
            // subscribes the OnTimerTick method to the TimerService's onTick event
            // so it gets called every time the timer ticks
            TimerService.OnTick += OnTimerTick;
        }

        // starts a 60 second timer, resets score and generates the 1st question
        private void StartGame()
        {
            MathGameService.Score = 0;
            TimerService.Start(60);
            GenerateQuestion();
        }

        // generates a question and resets userAnswer, result and isSubmitted fields
        private void GenerateQuestion()
        {
            var questionData = MathGameService.GenerateQuestion();
            question = questionData.question;
            numbers = questionData.numbers;
            operations = questionData.operations;
            userAnswer = string.Empty;
            result = null;
            isSubmitted = false;
        }

        private void CheckAnswer()
        {
            // parses the answer from the user input to an integer number
            // then checks the answer and returns true or false
            // according to the true/false value it sets the result value accordingly (correct or try again)
            // if user input was not and integer number, then result is set to "try again"
            if (question != null && numbers != null && operations != null)
            {
                if (int.TryParse(userAnswer, out int parsedAnswer))
                {
                    bool isCorrect = MathGameService.CheckAnswer(numbers, operations, parsedAnswer);
                    result = isCorrect ? "Correct!" : "Try again!";
                    isSubmitted = true;
                }
                else
                {
                    result = "Try again!";
                }
            }
        }

        private void OnTimerTick()
        {
            InvokeAsync(() => // makes sure this code runs on the right thread so the UI updates correctly
            {
                // checks if the timer has run out of time,
                // then mark the game as submitted
                // then stop the timer
                if (TimerService.GetRemainingTime() == 0)
                {
                    result = "Time's up!";
                    isSubmitted = true;
                    TimerService.Stop();
                }
                // updates the UI to reflect the changes
                StateHasChanged();
            });
        }

        public void Dispose()
        {
            // unsubscribes from the OnTick event to prevent memory leaks
            TimerService.OnTick -= OnTimerTick;
        }
    }
}