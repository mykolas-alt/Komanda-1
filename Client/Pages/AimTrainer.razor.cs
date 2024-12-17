namespace Projektas.Client.Pages {
    using Microsoft.AspNetCore.Components;
    using Projektas.Client.Interfaces;
    using Projektas.Client.Services;
    using Projektas.Shared.Enums;
    using Projektas.Shared.Models;
    using System;
    using System.Threading.Tasks;

    public partial class AimTrainer : IDisposable {
        [Inject]
        public Random _random {get; set;}

        [Inject]
        public IAimTrainerService AimTrainerService {get; set;}

        [Inject]
        public ITimerService TimerService {get; set;}

        [Inject]
        public IAccountAuthStateProvider AuthStateProvider {get; set;}
        
        public string gameScreen = "main";
        public GameDifficulty Difficulty {get; set;} = GameDifficulty.Normal;

        private System.Timers.Timer? moveDotTimer;
        public (int x,int y) TargetPosition {get; set;}
        public int moveCounter {get; private set;}
        public int moveDirection {get; set;} // 0 = left, 1 = right, 2 = up, 3 = down

        public int score {get; private set;}
        private UserScoreDto<AimTrainerData>? highscore {get; set;}
        public List<UserScoreDto<AimTrainerData>>? topScores {get; private set;}
        
        public string? username = null;

        public void ChangeScreen(string mode) {
            gameScreen = mode;
        }

        public void ChangeDifficulty(string mode) {
            switch(mode) {
                case "Normal":
                    Difficulty = GameDifficulty.Normal;
                    break;
                case "Hard":
                    Difficulty = GameDifficulty.Hard;
                    break;
            }
        }

        protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChangedAsync;

			await LoadUsernameAsync();
            if(username != null) {
                highscore = await AimTrainerService.GetUserHighscoreAsync(username);
            }
            topScores = await AimTrainerService.GetTopScoresAsync(topCount: 10);
		}

		private async Task LoadUsernameAsync() {
			username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
			StateHasChanged();
		}

		private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task) {
			await InvokeAsync(LoadUsernameAsync);
			StateHasChanged();
		}

        public void StartGame() {
            gameScreen = "started";
            ResetGame(1000, 400);
            TimerService.Start(30);
            TimerService.OnTick += TimerTick;

            if(Difficulty == GameDifficulty.Hard) {
                StartMovingDotTimer();
            }
            InvokeAsync(StateHasChanged);
        }

        private void StartMovingDotTimer() {
            moveDotTimer = new System.Timers.Timer(10);
            moveDotTimer.Elapsed += (sender, e) => {
                MoveTarget(1000, 400);
                InvokeAsync(StateHasChanged);
            };
            moveDotTimer.Start();
        }

        public void TimerTick() {
            if(TimerService.RemainingTime == 0) {
                EndGameAsync();
            } else {
                InvokeAsync(StateHasChanged);
            }
        }

        public void OnTargetClicked() {
            if(TimerService.RemainingTime > 0) {
                score++;
                SetRandomTargetPosition(1000, 400);
                InvokeAsync(StateHasChanged);
            }
        }

        private async Task EndGameAsync() {
            gameScreen = "ended";
            if(username != null) {
                await AimTrainerService.SaveScoreAsync(username, score, Difficulty);
                highscore = await AimTrainerService.GetUserHighscoreAsync(username);
            }
            topScores = await AimTrainerService.GetTopScoresAsync(topCount: 10);
            TimerService.OnTick -= TimerTick;
            moveDotTimer?.Stop();
            moveDotTimer?.Dispose();
            await InvokeAsync(StateHasChanged);
        }

        private void ResetGame(int boxWidth, int boxHeight) {
            score = 0;
            moveCounter = 0;
            SetRandomTargetPosition(boxWidth, boxHeight);
            moveDirection = _random.Next(4);
        }

        private void SetRandomTargetPosition(int boxWidth, int boxHeight) {
            int targetSizeOffset = Difficulty == GameDifficulty.Hard ? 34 : 54;
            int x = _random.Next(4, boxWidth - targetSizeOffset);
            int y = _random.Next(4, boxHeight - targetSizeOffset);
            TargetPosition = (x, y);
        }

        public void MoveTarget(int boxWidth, int boxHeight) {
            moveCounter++;

            if(moveCounter % 50 == 0) {
                moveDirection = _random.Next(4);
            }

            switch(moveDirection) {
                case 0: // left
                    TargetPosition = (TargetPosition.x - 1, TargetPosition.y);
                    if(TargetPosition.x < 4)
                        moveDirection = 1;
                    break;
                case 1: // right
                    TargetPosition = (TargetPosition.x + 1, TargetPosition.y);
                    if(TargetPosition.x > boxWidth - 34)
                        moveDirection = 0;
                    break;
                case 2: // up
                    TargetPosition = (TargetPosition.x ,TargetPosition.y - 1);
                    if(TargetPosition.y < 4)
                        moveDirection = 3;
                    break;
                case 3: // down
                    TargetPosition = (TargetPosition.x, TargetPosition.y + 1);
                    if(TargetPosition.y > boxHeight - 34)
                        moveDirection = 2;
                    break;
            }
        }

        private string GetTargetColor() {
            return Difficulty == GameDifficulty.Hard ? "black" : "blue";
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            AuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
            if(disposing) {
                TimerService.OnTick -= TimerTick;
                moveDotTimer?.Stop();
                moveDotTimer?.Dispose();
                moveDotTimer = null;
            }
        }
    }
}
