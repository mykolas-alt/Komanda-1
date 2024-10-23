namespace Projektas.Client.Pages
{
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Threading.Tasks;

    public partial class AimTrainer : IDisposable
    {
        private bool isGameActive = false;
        private bool isGameOver = false;
        private bool isHardMode = false;
        private System.Timers.Timer? moveDotTimer;
        private readonly Random _random = new Random();

        public (int x, int y) TargetPosition { get; private set; }
        public int Score { get; private set; }
        private int moveCounter;
        private int moveDirection; // 0 = left, 1 = right, 2 = up, 3 = down

        private void OnDifficultyChanged(ChangeEventArgs e)
        {
            if (!isGameActive)
            {
                isHardMode = e.Value?.ToString() == "Hard";
            }
        }

        private void StartGame()
        {
            isGameActive = true;
            isGameOver = false;
            ResetGame(1000, 400);
            TimerService.Start(30);
            TimerService.OnTick += TimerTick;

            if (isHardMode)
            {
                StartMovingDotTimer();
            }

            StateHasChanged();
        }

        private void StartMovingDotTimer()
        {
            moveDotTimer = new System.Timers.Timer(10);
            moveDotTimer.Elapsed += (sender, e) =>
            {
                MoveTarget(1000, 400);
                InvokeAsync(StateHasChanged);
            };
            moveDotTimer.Start();
        }

        private void TimerTick()
        {
            if (TimerService.RemainingTime == 0)
            {
                EndGame();
            }
            else
            {
                InvokeAsync(StateHasChanged);
            }
        }

        private void OnTargetClicked()
        {
            if (TimerService.RemainingTime > 0)
            {
                Score++;
                SetRandomTargetPosition(1000, 400);
                StateHasChanged();
            }
        }

        private async Task EndGame()
        {
            isGameActive = false;
            isGameOver = true;
            TimerService.OnTick -= TimerTick;
            moveDotTimer?.Stop();
            moveDotTimer?.Dispose();
            await InvokeAsync(StateHasChanged);
        }

        private void TryAgain()
        {
            isGameOver = false;
            StartGame();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TimerService.OnTick -= TimerTick;
                moveDotTimer?.Stop();
                moveDotTimer?.Dispose();
                moveDotTimer = null;
            }
        }

        private void ResetGame(int boxWidth, int boxHeight)
        {
            Score = 0;
            moveCounter = 0;
            SetRandomTargetPosition(boxWidth, boxHeight);
            moveDirection = _random.Next(4);
        }

        private void SetRandomTargetPosition(int boxWidth, int boxHeight)
        {
            int targetSizeOffset = isHardMode ? 34 : 54;
            int x = _random.Next(4, boxWidth - targetSizeOffset);
            int y = _random.Next(4, boxHeight - targetSizeOffset);
            TargetPosition = (x, y);
        }

        private void MoveTarget(int boxWidth, int boxHeight)
        {
            moveCounter++;

            if (moveCounter % 50 == 0)
            {
                moveDirection = _random.Next(4);
            }

            switch (moveDirection)
            {
                case 0: // left
                    TargetPosition = (TargetPosition.x - 1, TargetPosition.y);
                    if (TargetPosition.x < 4)
                        moveDirection = 1;
                    break;
                case 1: // right
                    TargetPosition = (TargetPosition.x + 1, TargetPosition.y);
                    if (TargetPosition.x > boxWidth - 34)
                        moveDirection = 0;
                    break;
                case 2: // up
                    TargetPosition = (TargetPosition.x, TargetPosition.y - 1);
                    if (TargetPosition.y < 4)
                        moveDirection = 3;
                    break;
                case 3: // down
                    TargetPosition = (TargetPosition.x, TargetPosition.y + 1);
                    if (TargetPosition.y > boxHeight - 34)
                        moveDirection = 2;
                    break;
            }
        }

        private string GetTargetColor()
        {
            return isHardMode ? "black" : "blue";
        }
    }
}
