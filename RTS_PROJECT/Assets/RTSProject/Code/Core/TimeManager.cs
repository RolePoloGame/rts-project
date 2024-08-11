namespace RTS.Core
{
    public class TimeManager : Singleton<TimeManager>, ITickService
    {
        public event TickRateChangeEvent OnTickChanged;

        private ETickRate currentRate = ETickRate.Pause;
        public float TickRate => GetTickRate(currentRate);

        private float GetTickRate(ETickRate tickRate) => (float)tickRate;
        public override void Initialize()
        {
            base.Initialize();
            RegisterService();
        }
        private void OnDestroy()
        {
            RemoveService();
        }

        public void RegisterService() => ServiceManager.Register(this);
        public void RemoveService() => ServiceManager.Remove(this);

        public void SetTickRate(ETickRate newRate)
        {
            OnTickChanged?.Invoke(TickRate, GetTickRate(newRate));
            currentRate = newRate;
        }
    }
}