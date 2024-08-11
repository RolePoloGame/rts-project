namespace RTS.Core
{
    public class TimeManager : Singleton<TimeManager>, ITickService
    {
        public event TickRateChangeEvent OnTickChanged;

        private ETickRate currentRate;
        public float TickRate => GetTickRate(currentRate);

        private float GetTickRate(ETickRate tickRate) => (float)tickRate;

        public void RegisterService() => ServiceManager.Instance.Register(this);
        public void RemoveService() => ServiceManager.Instance.Remove(this);

        public void SetTickRate(ETickRate newRate)
        {
            OnTickChanged?.Invoke(TickRate, GetTickRate(newRate));
            currentRate = newRate;
        }
    }
}