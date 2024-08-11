using System;

namespace RTS.Core
{
    public interface ITickService : IService
    {
        public event TickRateChangeEvent OnTickChanged;
        public void SetTickRate(ETickRate newRate);
        public float TickRate { get; }
    }
}
