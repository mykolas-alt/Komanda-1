using Microsoft.AspNetCore.Components;
using Projektas.Client.Interfaces;
using Projektas.Shared.Models;

namespace Projektas.Client.Pages {
	public partial class MathGame {
        public string? question {get;private set;}=null;
        public bool isTimesUp {get;private set;}=false;
        public List<int>? options {get;private set;}
        public bool? isCorrect {get;private set;}=null;
		public List<UserScoreDto<MathGameData>>? topScores {get;private set;}
		public string? username=null;
        public int score {get;private set;}=0;
		private UserScoreDto<MathGameData>? highscore {get;set;}

        [Inject]
        public IMathGameService MathGameService {get;set;}

        [Inject]
        public ITimerService TimerService {get;set;}
        [Inject]
        public IAccountAuthStateProvider AuthStateProvider {get;set;}

		protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged+=OnAuthenticationStateChangedAsync;

			await LoadUsernameAsync();
		}

		private async Task LoadUsernameAsync() {
			username=await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
			StateHasChanged();
		}

		private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task) {
			await InvokeAsync(LoadUsernameAsync);
			StateHasChanged();
		}

        protected override async void OnInitialized() {
            TimerService.OnTick+=OnTimerTick;
        }


        public async Task StartGameAsync() {
            TimerService.Start(60);
            isTimesUp=false;
            score=0;
            await GenerateQuestionAsync();
        }

        public async Task GenerateQuestionAsync() {
            isCorrect=null;
            question=await MathGameService.GetQuestionAsync(score);
            options=await MathGameService.GetOptionsAsync();
        }

        public async Task CheckAnswer(int option) {
            if(question!=null) {
                isCorrect=await MathGameService.CheckAnswerAsync(option);
                if(isCorrect==false) {
                    if(TimerService.RemainingTime>5) {
                        TimerService.RemainingTime=TimerService.RemainingTime-5;
                    } else {
                        TimerService.RemainingTime=0;
                        OnTimerTick();
                        return;
                    }
                } else {
                    score++;
                }
                await GenerateQuestionAsync();
                await InvokeAsync(() => {
                    StateHasChanged();
                });
            }
        }

        public async void OnTimerTick() {
            await InvokeAsync(async () => {
                if(TimerService.RemainingTime==0) {
                    isTimesUp=true;
                    TimerService.Stop();

                    if(username!=null) {
                        await MathGameService.SaveScoreAsync(username,score);
                    }

                    if(username!=null) {
                        highscore=await MathGameService.GetUserHighscoreAsync(username);
                    }
                    
                    topScores=await MathGameService.GetTopScoresAsync(topCount:5);
                }
                StateHasChanged();
            });
        }

        public void Dispose() {
            AuthStateProvider.AuthenticationStateChanged-=OnAuthenticationStateChangedAsync;
            TimerService.OnTick-=OnTimerTick;
        }
    }
}