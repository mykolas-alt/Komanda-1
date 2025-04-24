namespace Projektas.Client.Pages {
    using Microsoft.AspNetCore.Components;
    using Projektas.Client.Interfaces;
    using Projektas.Client.Services;
    using Projektas.Shared.Enums;
    using Projektas.Shared.Models;
    using System;
    using System.Drawing;
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
        public GameDifficulty Difficulty {get; set;} = GameDifficulty.Easy;

        private System.Timers.Timer? moveDotTimer;
        public (int x,int y) TargetPosition {get; set;}
        public int moveCounter {get; private set;}
        public int moveDirection {get; set;}
        private int directionChangeThreshold = 50;
        private int bufferZone = 6;

        public int score {get; private set;}
        private UserScoreDto<AimTrainerData>? highscore {get; set;}
        private bool highscoreChecked = false;
        public List<UserScoreDto<AimTrainerData>>? topScores {get; private set;}
        
        public string? username = null;

        public void ChangeScreen(string mode) {
            gameScreen = mode;
        }

        public async void ChangeDifficulty(string mode) {
            switch(mode) {
                case "Easy":
                    Difficulty = GameDifficulty.Easy;
                    if(username != null) {
                        await FetchHighscoreAsync();
                    }
                    topScores = await AimTrainerService.GetTopScoresAsync(Difficulty, topCount: 10);
                    await InvokeAsync(StateHasChanged);
                    break;
                case "Normal":
                    Difficulty=GameDifficulty.Normal;
                    if(username != null) {
                        await FetchHighscoreAsync();
                    }
                    topScores = await AimTrainerService.GetTopScoresAsync(Difficulty, topCount: 10);
                    await InvokeAsync(StateHasChanged);
                    break;
                case "Hard":
                    Difficulty = GameDifficulty.Hard;
                    if(username != null) {
                        await FetchHighscoreAsync();
                    }
                    topScores = await AimTrainerService.GetTopScoresAsync(Difficulty, topCount: 10);
                    await InvokeAsync(StateHasChanged);
                    break;
            }
        }

        private async Task FetchHighscoreAsync() {
            try {
                highscore = await AimTrainerService.GetUserHighscoreAsync(username, Difficulty);
            } catch {
                highscore = null;
            } finally {
                highscoreChecked = true;
            }
        }

        protected override async Task OnInitializedAsync() {
			AuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChangedAsync;

			await LoadUsernameAsync();
            if(username != null) {
                await FetchHighscoreAsync();
            }
            topScores = await AimTrainerService.GetTopScoresAsync(Difficulty, topCount: 10);
		}

		private async Task LoadUsernameAsync() {
			username = await ((IAccountAuthStateProvider)AuthStateProvider).GetUsernameAsync();
            await InvokeAsync(StateHasChanged);
        }

		private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task) {
			await InvokeAsync(LoadUsernameAsync);
            await InvokeAsync(StateHasChanged);
        }

        public void StartGame() {
            gameScreen = "started";
            ResetGame(1000, 400);
            TimerService.Start(30);
            TimerService.OnTick += TimerTick;
            
            if (Difficulty == GameDifficulty.Normal) {
                StartMovingDotNormal();
            }
            if(Difficulty == GameDifficulty.Hard ) {
                StartMovingDotHard();
            }
            InvokeAsync(StateHasChanged);
        }

        private void StartMovingDotNormal() {
            moveDotTimer = new System.Timers.Timer(10);
            moveDotTimer.Elapsed += (sender, e) => {
                MoveTargetNormal(1000, 400);
                InvokeAsync(StateHasChanged);
            };
            moveDotTimer.Start();
        }
        private void StartMovingDotHard() {
            moveDotTimer = new System.Timers.Timer(10);
            moveDotTimer.Elapsed += (sender, e) => {
                MoveTargetHard(1000, 400);
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
                await FetchHighscoreAsync();
            }
            topScores = await AimTrainerService.GetTopScoresAsync(Difficulty, topCount: 10);
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

        public void MoveTargetNormal(int boxWidth, int boxHeight) {
            int dotSize = 50;
            moveCounter++;


            if(moveCounter % directionChangeThreshold == 0) {
                moveDirection = _random.Next(4);
            }

            switch(moveDirection) {
                case 0: // left
                    TargetPosition = (TargetPosition.x - 1, TargetPosition.y);
                    if(TargetPosition.x < bufferZone)
                        moveDirection = 1;
                    break;
                case 1: // right
                    TargetPosition = (TargetPosition.x + 1, TargetPosition.y);
                    if(TargetPosition.x > boxWidth - bufferZone - dotSize)
                        moveDirection = 0;
                    break;
                case 2: // up
                    TargetPosition = (TargetPosition.x ,TargetPosition.y - 1);
                    if(TargetPosition.y < bufferZone)
                        moveDirection = 3;
                    break;
                case 3: // down
                    TargetPosition = (TargetPosition.x, TargetPosition.y + 1);
                    if(TargetPosition.y > boxHeight - bufferZone - dotSize)
                        moveDirection = 2;
                    break;
            }
        }

        public void MoveTargetHard(int boxWidth, int boxHeight) {
            moveCounter++;
            int dotSize = 40;
            // max speed - fluctuates from 1 to 4. Every 3000 ms increses +1.
            int maxSpeed = 1 + moveCounter % 4 + moveCounter / 3000; // max end game speed: 1 + max(0,3) + 10 = 14
            // Min speed - fluctuates from 1 to 2. Every 400 ms increses +1. 
            int minSpeed = 1 + moveCounter % 2 + moveCounter / 3000; // max end game speed: 1 + max(0,1) + 10 = 12
            int travelSpeed = minSpeed;

            if (moveCounter % 10 == 0) {
                travelSpeed = _random.Next(minSpeed, maxSpeed);
            }

            if(moveCounter % directionChangeThreshold == 0) {

                moveDirection = _random.Next(8);
                directionChangeThreshold = _random.Next(20, 100);
            }

            switch(moveDirection) {
                case 0: // left
                    TargetPosition = (TargetPosition.x - travelSpeed, TargetPosition.y);
                    if(TargetPosition.x < bufferZone)
                        moveDirection = 1;
                    break;
                case 1: // right
                    TargetPosition = (TargetPosition.x + travelSpeed, TargetPosition.y);
                    if(TargetPosition.x > boxWidth - bufferZone - dotSize)
                        moveDirection = 0;
                    break;
                case 2: // up
                    TargetPosition = (TargetPosition.x ,TargetPosition.y - travelSpeed);
                    if(TargetPosition.y < bufferZone)
                        moveDirection = 3;
                    break;
                case 3: // down
                    TargetPosition = (TargetPosition.x, TargetPosition.y + travelSpeed);
                    if(TargetPosition.y > boxHeight - bufferZone - dotSize)
                        moveDirection = 2;
                    break;
                case 4: // diagonal up-right
                    TargetPosition = (TargetPosition.x + travelSpeed, TargetPosition.y + travelSpeed);
                    if(TargetPosition.x > boxWidth - bufferZone - dotSize || TargetPosition.y > boxHeight - bufferZone - dotSize)
                        moveDirection = 7;
                    break; 
                case 5: // diagnonal up-left
                    TargetPosition = (TargetPosition.x - travelSpeed, TargetPosition.y + travelSpeed);
                    if(TargetPosition.x < bufferZone || TargetPosition.y > boxHeight - bufferZone - dotSize)
                        moveDirection = 6;
                    break;
                case 6: // diagnonal down-right
                    TargetPosition = (TargetPosition.x + travelSpeed, TargetPosition.y - travelSpeed);
                    if(TargetPosition.x > boxWidth - bufferZone - dotSize || TargetPosition.y < bufferZone)
                        moveDirection = 5;
                    break;     
                case 7: // diagnonal down-left
                    TargetPosition = (TargetPosition.x - travelSpeed, TargetPosition.y - travelSpeed);
                    if(TargetPosition.x < bufferZone  || TargetPosition.y < bufferZone)
                        moveDirection = 4;
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
