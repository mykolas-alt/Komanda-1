namespace Projektas.Client.Pages {
    using Microsoft.AspNetCore.Components;
    using Projektas.Client.Interfaces;
    using System;
    using System.Threading.Tasks;

    public partial class AimTrainer : IDisposable {
        public bool isGameActive {get;private set;}=false;
        public bool isGameOver {get;private set;}=false;
        public bool isHardMode {get;private set;}=false;
        private System.Timers.Timer? moveDotTimer;

        public string? username=null;

        public (int x,int y) TargetPosition {get;set;}
        public int score {get;private set;}
        public int moveCounter {get;private set;}
        public int moveDirection {get;set;} // 0 = left, 1 = right, 2 = up, 3 = down

        [Inject]
        public Random _random {get;set;}

        [Inject]
        public ITimerService TimerService {get;set;}

        [Inject]
        public IAccountAuthStateProvider AuthStateProvider {get;set;}

		protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged+=OnAuthenticationStateChanged;

			await LoadUsernameAsync();
		}

		private async Task LoadUsernameAsync() {
			username=await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
			StateHasChanged();
		}

		private async void OnAuthenticationStateChanged(Task<AuthenticationState> task) {
			await InvokeAsync(LoadUsernameAsync);
			StateHasChanged();
		}

        public void OnDifficultyChanged(ChangeEventArgs e) {
            if(!isGameActive) {
                isHardMode=e.Value?.ToString()=="Hard";
            }
        }

        public void StartGame() {
            isGameActive=true;
            isGameOver=false;
            ResetGame(1000,400);
            TimerService.Start(30);
            TimerService.OnTick+=TimerTick;

            if(isHardMode) {
                StartMovingDotTimer();
            }
            InvokeAsync(StateHasChanged);
        }

        private void StartMovingDotTimer() {
            moveDotTimer=new System.Timers.Timer(10);
            moveDotTimer.Elapsed+=(sender, e) => {
                MoveTarget(1000,400);
                InvokeAsync(StateHasChanged);
            };
            moveDotTimer.Start();
        }

        public void TimerTick() {
            if(TimerService.RemainingTime==0) {
                EndGame();
            } else {
                InvokeAsync(StateHasChanged);
            }
        }

        public void OnTargetClicked() {
            if(TimerService.RemainingTime>0) {
                score++;
                SetRandomTargetPosition(1000,400);
                InvokeAsync(StateHasChanged);
            }
        }

        private async Task EndGame() {
            if(username!=null) {
                await AimTrainerService.SaveScoreAsync(username,score);
            }
            isGameActive=false;
            isGameOver=true;
            TimerService.OnTick-=TimerTick;
            moveDotTimer?.Stop();
            moveDotTimer?.Dispose();
            await InvokeAsync(StateHasChanged);
        }

        public void TryAgain() {
            isGameOver=false;
            StartGame();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            AuthStateProvider.AuthenticationStateChanged-=OnAuthenticationStateChanged;
            if(disposing) {
                TimerService.OnTick-=TimerTick;
                moveDotTimer?.Stop();
                moveDotTimer?.Dispose();
                moveDotTimer=null;
            }
        }

        private void ResetGame(int boxWidth,int boxHeight) {
            score=0;
            moveCounter=0;
            SetRandomTargetPosition(boxWidth,boxHeight);
            moveDirection=_random.Next(4);
        }

        private void SetRandomTargetPosition(int boxWidth,int boxHeight) {
            int targetSizeOffset=isHardMode?34 : 54;
            int x=_random.Next(4,boxWidth-targetSizeOffset);
            int y=_random.Next(4,boxHeight-targetSizeOffset);
            TargetPosition=(x,y);
        }

        public void MoveTarget(int boxWidth,int boxHeight) {
            moveCounter++;

            if(moveCounter%50==0) {
                moveDirection=_random.Next(4);
            }

            switch(moveDirection) {
                case 0: // left
                    TargetPosition=(TargetPosition.x-1,TargetPosition.y);
                    if(TargetPosition.x<4)
                        moveDirection=1;
                    break;
                case 1: // right
                    TargetPosition=(TargetPosition.x+1,TargetPosition.y);
                    if(TargetPosition.x>boxWidth-34)
                        moveDirection=0;
                    break;
                case 2: // up
                    TargetPosition=(TargetPosition.x,TargetPosition.y-1);
                    if(TargetPosition.y<4)
                        moveDirection=3;
                    break;
                case 3: // down
                    TargetPosition=(TargetPosition.x,TargetPosition.y+1);
                    if(TargetPosition.y>boxHeight-34)
                        moveDirection=2;
                    break;
            }
        }

        private string GetTargetColor() {
            return isHardMode?"black" : "blue";
        }
    }
}
