using Projektas.Client.Services;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages {
    public partial class MathGame {
        private string? question=null;
        public string? username=null;
        private bool isTimesUp=false;
        private List<int>? options;
        private int score=0;
        private int highscore=0;
        private bool? isCorrect=null;
        private List<UserScoreDto>? topScores;

		protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged+=OnAuthenticationStateChanged;

			await LoadUsernameAsync();
		}

		private async Task LoadUsernameAsync() {
			username=await ((AccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
			StateHasChanged();
		}

		private async void OnAuthenticationStateChanged(Task<AuthenticationState> task) {
			await InvokeAsync(LoadUsernameAsync);
			StateHasChanged();
		}

        protected override async void OnInitialized() {
            TimerService.OnTick+=OnTimerTick;
        }


        private async Task StartGame() {
            TimerService.Start(60);
            isTimesUp=false;
            score=0;
            await GenerateQuestion();
        }

        private async Task GenerateQuestion() {
            isCorrect=null;
            question=await MathGameService.GetQuestionAsync(score);
            options=await MathGameService.GetOptionsAsync();
        }

        private async Task CheckAnswer(int option) {
            if (question!=null) {
                isCorrect=await MathGameService.CheckAnswerAsync(option);
                if (isCorrect==false) {
                    if (TimerService.RemainingTime>5) {
                        TimerService.RemainingTime=TimerService.RemainingTime-5;
                    } else {
                        TimerService.RemainingTime=0;
                        OnTimerTick();
                        return;
                    }
                } else {
                    score++;
                }
                await GenerateQuestion();
                StateHasChanged();
            }
        }

        private async void OnTimerTick() {
            await InvokeAsync(async () => {
                if (TimerService.RemainingTime==0) {
                    isTimesUp=true;
                    TimerService.Stop();

                    if(username!=null) {
                        await MathGameService.SaveScoreAsync(username,score);
                    }

                    if(username!=null) {
                        highscore=await MathGameService.GetUserHighscore(username);
                    }
                    
                    topScores=await MathGameService.GetTopScoresAsync(topCount:5);
                }
                StateHasChanged();
            });
        }

        public void Dispose() {
            AuthStateProvider.AuthenticationStateChanged-=OnAuthenticationStateChanged;
            TimerService.OnTick-=OnTimerTick;
        }
    }
}
