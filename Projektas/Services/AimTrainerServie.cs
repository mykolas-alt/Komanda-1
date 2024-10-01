using System;

namespace Projektas.Services
{
    public class AimTrainerService
    {
        private Random _random = new Random();
        public (int x, int y) TargetPosition { get; private set; }
        public int Score { get; private set; }
        private int moveCounter = 0; 
        private int moveDirection; // 0 = left 1 = right 2 = up 3 = down
        public bool IsHardMode { get; set; }

        public void ResetGame(int boxWidth, int boxHeight)
        {
            Score = 0;
            moveCounter = 0;
            SetRandomTargetPosition(boxWidth, boxHeight);
            moveDirection = _random.Next(4);
        }

        public void RegisterHit(int boxWidth, int boxHeight)
        {
            Score++;
            SetRandomTargetPosition(boxWidth, boxHeight);
        }

        public void SetRandomTargetPosition(int boxWidth, int boxHeight)
        {
            int targetSizeOffset = IsHardMode ? 34 : 54;
            int x = _random.Next(4, boxWidth - targetSizeOffset);
            int y = _random.Next(4, boxHeight - targetSizeOffset);
            TargetPosition = (x, y);
        }

        public void MoveTarget(int boxWidth, int boxHeight)
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

        public string GetTargetColor(bool isHardMode)
        {
            return isHardMode ? "black" : "blue"; 
        }
    }
}