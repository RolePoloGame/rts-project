using System.Collections.Generic;

namespace RTS.Core
{
    public class ServiceManager : Singleton<ServiceManager>
    {
        private List<IAgentService> agentServices = new();

        public override void Initialize()
        {
            base.Initialize();
            agentServices = new();
        }

        public void Register(IAgentService service)
        {
            if (agentServices.Contains(service)) return;
            agentServices.Add(service);
        }
        public void Remove(IAgentService service)
        {

            if (!agentServices.Contains(service)) return;
            agentServices.Remove(service);
        }
    }
}
