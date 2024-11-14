using Projektas.Client.Interfaces;

namespace Projektas.Client.Services
{
    public class TimerService : ITimerService
    {
        private Timer? _timer;
        public int RemainingTime { get; set; }
        public event Action? OnTick;

        // a timer is created for a selected amount of seconds
        public void Start(int seconds)
        {
            RemainingTime = seconds;
            _timer?.Dispose(); // disposes any existing timer instance
            _timer = new Timer(OnTimerElapsed, null, 0, 1000);
        }

        // disposes any existing timer instances and sets it to null
        public void Stop()
        {
            _timer?.Dispose();
            _timer = null;
            RemainingTime = 0;
        }

        // method is called once a second has passed
        // a _seconds field is decremented if there is still time left
        // else, the time is stopped
        private void OnTimerElapsed(object? state)
        {
            if (RemainingTime > 0)
            {
                RemainingTime--;
                OnTick?.Invoke(); // notifies subscribers that a second has passed
            }
            else
            {
                Stop();
            }
        }
    }
}