namespace Projektas.Client.Pages
{
    using Projektas.Client.Services;

    public partial class MathGame
    {
        private string? question = null;
        private bool isTimesUp = false;
        private List<int>? options;
        private int score;
        private int highscore;
        private bool? isCorrect = null;

        protected override async Task OnInitializedAsync()
        {
            TimerService.OnTick += OnTimerTick;
            score = await MathGameService.GetScoreAsync();
        }


        private async Task StartGame()
        {
            await MathGameService.ResetScoreAsync();
            TimerService.Start(60);
            await GenerateQuestion();
            highscore = await MathGameService.GetHighscoreAsync();
            score = await MathGameService.GetScoreAsync();

        }

        private async Task GenerateQuestion()
        {
            isCorrect = null;
            question = await MathGameService.GetQuestionAsync();
            options = await MathGameService.GetOptionsAsync();
            isTimesUp = false;
        }

        private async Task CheckAnswer(int option)
        {
            if (question != null)
            {
                isCorrect = await MathGameService.CheckAnswerAsync(option);
                if (isCorrect == false)
                {
                    if (TimerService.RemainingTime > 5)
                    {
                        TimerService.RemainingTime = TimerService.RemainingTime - 5;
                    }
                    else
                    {
                        TimerService.RemainingTime = 0;
                        OnTimerTick();
                        return;
                    }
                }
                await GenerateQuestion();
                score = await MathGameService.GetScoreAsync();
                highscore = await MathGameService.GetHighscoreAsync();
                StateHasChanged();
            }
        }

        private async void OnTimerTick()
        {
            await InvokeAsync(() =>
            {
                if (TimerService.RemainingTime == 0)
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
