using System;

namespace RTS.Core
{
    public interface IAgentService : IService
    {
        public event Action<UniqueID> OnAgentSpawned;
        public event Action<UniqueID> OnAgentRemoved;
        public event Action<UniqueID> OnAgentSelectedNewTarget;
        public event Action<UniqueID> OnAgentArrived;
        public void RequestSpawnAgent();
        public void RequestRemoveAgent(UniqueID entityID);
    }
}
