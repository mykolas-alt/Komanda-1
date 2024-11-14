namespace Projektas.Client.Pages
{
    using Microsoft.AspNetCore.Components;
    using Projektas.Client.Interfaces;
    using Projektas.Shared.Models;


    public partial class MathGame
    {
        public string? question { get; private set; } = null;
        public bool isTimesUp { get; private set; } = false;
        public List<int>? options { get; private set; }
        public GameState gameState { get; private set; } = new GameState();
        public bool? isCorrect { get; private set; } = null;
        public List<int>? topScores { get; private set; }

        [Inject]
        public IMathGameStateService MathGameStateService { get; set; }

        [Inject]
        public IMathGameService MathGameService { get; set; }

        [Inject]
        public ITimerService TimerService { get; set; }

        protected override async void OnInitialized()
        {
            TimerService.OnTick += OnTimerTick;
            gameState = await MathGameStateService.GetGameState();
        }


        public async Task StartGame()
        {
            TimerService.Start(60);
            isTimesUp = false;
            gameState.Score = 0;
            await GenerateQuestion();
            await MathGameStateService.UpdateGameState(gameState);
        }

        public async Task GenerateQuestion()
        {
            isCorrect = null;
            question = await MathGameService.GetQuestionAsync(gameState.Score);
            options = await MathGameService.GetOptionsAsync();
        }

        public async Task CheckAnswer(int option)
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
                else
                {
                    await MathGameStateService.IncrementScore(gameState);
                }
                await GenerateQuestion();
                await InvokeAsync(() =>
                {
                    StateHasChanged();
                });
            }
        }

        public async void OnTimerTick()
        {
            await InvokeAsync(async () =>
            {
                if (TimerService.RemainingTime == 0)
                {
                    isTimesUp = true;
                    TimerService.Stop();
                    await MathGameService.SaveDataAsync(gameState.Score);
                    topScores = await MathGameService.GetTopScoresAsync(topCount: 5);
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
