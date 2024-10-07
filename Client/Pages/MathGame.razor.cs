namespace Projektas.Client.Pages
{
    using Projektas.Client.Services;

    public partial class MathGame
    {
        private string? question = null;
        private bool isTimesUp = false;
        private List<int>? options;
        private int remainingTime;
        private int score;
        private int lives;
        private int highscore;

        protected override async Task OnInitializedAsync()
        {
            TimerService.OnTick += OnTimerTick;
            score = await MathGameService.GetScoreAsync();
            lives = await MathGameService.GetLivesAsync();
        }


        private async Task StartGame()
        {
            await MathGameService.ResetScoreAsync();
            await MathGameService.ResetLivesAsync();
            lives = await MathGameService.GetLivesAsync();
            TimerService.Start(60);
            await GenerateQuestion();
            highscore = await MathGameService.GetHighscoreAsync();
            score = await MathGameService.GetScoreAsync();

        }

        private async Task GenerateQuestion()
        {
            question = await MathGameService.GetQuestionAsync();
            options = await MathGameService.GetOptionsAsync();
            isTimesUp = false;
        }

        private async Task CheckAnswer(int option)
        {
            if (question != null)
            {
                await MathGameService.CheckAnswerAsync(option);
                await GenerateQuestion();
                score = await MathGameService.GetScoreAsync();
                lives = await MathGameService.GetLivesAsync();
                highscore = await MathGameService.GetHighscoreAsync();
                StateHasChanged();
            }
        }

        private async void OnTimerTick()
        {
            await InvokeAsync(() =>
            {
                remainingTime = TimerService.GetRemainingTime();
                if (remainingTime == 0 || lives == 0)
                {
                    isTimesUp = true;
                    TimerService.Stop();
                }
                StateHasChanged();
            });
        }

        public void Dispose()
        {
            TimerService.OnTick -= OnTimerTick;
        }
    }
}
