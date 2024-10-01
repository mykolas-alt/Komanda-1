namespace Projektas.Components.Pages
{
    using Projektas.Services;

    public partial class MathGame
    {
        private string? question = null;
        private string? result;
        private bool isTimesUp = false;
        private List<int>? options;

        protected override void OnInitialized()
        {
            // subscribes the OnTimerTick method to the TimerService's onTick event
            // so it gets called every time the timer ticks
            TimerService.OnTick += OnTimerTick;
            MathGameService.Score = 0;
            MathGameService.Lives = 3;
        }

        // starts a 60 second timer, resets score and generates the 1st question
        private void StartGame()
        {
            MathGameService.Score = 0;
            MathGameService.Lives = 3;
            TimerService.Start(60);
            GenerateQuestion();
        }

        // generates a question and answer options
        private void GenerateQuestion()
        {
            question = MathGameService.GenerateQuestion();
            options = MathGameService.GenerateOptions();
            result = null;
            isTimesUp = false;
        }

        private async void GenerateQuestionWithDelay()
        {
            await Task.Delay(500);
            GenerateQuestion();
        }

        private async void ResetResultWithDelay()
        {
            await Task.Delay(500);
            result = null;
        }

        // checks the answer if it's correct
        private void CheckAnswer(int option)
        {
            if (question != null)
            {
                bool isCorrect = MathGameService.CheckAnswer(option);
                result = isCorrect ? "Correct!" : "Try again!";
                StateHasChanged();
            }
        }

        private void StopGame()
        {
            isTimesUp = true;
            TimerService.Stop();
        }

        private void OnTimerTick()
        {
            InvokeAsync(() => // makes sure this code runs on the right thread so the UI updates correctly
            {
                // checks if the timer has run out of time,
                // then mark the game as submitted
                // then stop the timer
                if (TimerService.GetRemainingTime() == 0 || MathGameService.Lives == 0)
                {
                    StopGame();
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