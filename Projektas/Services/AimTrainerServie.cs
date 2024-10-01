namespace Projektas.Services
{
    public class AimTrainerService
    {
        private Random _random = new Random();
        public (int x, int y) TargetPosition { get; private set; }
        public int Score { get; private set; }

        public void ResetGame(int boxWidth, int boxHeight)
        {
            Score = 0;
            SetRandomTargetPosition(boxWidth, boxHeight); 
        }

        public void RegisterHit(int boxWidth, int boxHeight)
        {
            Score++;
            SetRandomTargetPosition(boxWidth, boxHeight); 
        }

        public void SetRandomTargetPosition(int boxWidth, int boxHeight)
        {
            int x = _random.Next(5, boxWidth - 55);
            int y = _random.Next(5, boxHeight - 55);
            TargetPosition = (x, y);
        }
    }
}
