namespace Projektas.Client.Services
{
    public class TimerService
    {
        private Timer? _timer;
        private int _seconds;
        public event Action? OnTick;

        // a timer is created for a selected amount of seconds
        public void Start(int seconds)
        {
            _seconds = seconds;
            _timer?.Dispose(); // disposes any existing timer instance
            _timer = new Timer(OnTimerElapsed, null, 0, 1000);
        }

        // disposes any existing timer instances and sets it to null
        public void Stop()
        {
            _timer?.Dispose();
            _timer = null;
            _seconds = 0;
        }

        // method is called once a second has passed
        // a _seconds field is decremented if there is still time left
        // else, the time is stopped
        private void OnTimerElapsed(object? state)
        {
            if (_seconds > 0)
            {
                _seconds--;
                OnTick?.Invoke(); // notifies subscribers that a second has passed
            }
            else
            {
                Stop();
            }
        }

        // returns remaining time in seconds
        public int GetRemainingTime()
        {
            return _seconds;
        }
    }
}