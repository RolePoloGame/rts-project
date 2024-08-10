using System;

namespace RTS.Core
{
    public interface ITickService
    {
        public event TickRateChangeEvent OnTickChanged;
        public void SetTickRate(ETickRate newRate);
    }
}
