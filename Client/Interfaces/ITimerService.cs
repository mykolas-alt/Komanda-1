namespace Projektas.Client.Interfaces {
    public interface ITimerService {
        public event Action OnTick;
        public void Start(int duration);
        public void Stop();
        public int RemainingTime {get;set;}
    }
}
