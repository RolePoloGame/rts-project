using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Core
{
    public class ServiceManager : Singleton<ServiceManager>
    {
        private List<IService> agentServices = new();

        public T Get<T>() where T : IService => (T)agentServices.FirstOrDefault(x => x is T);

        public override void Initialize()
        {
            base.Initialize();
            agentServices = new();
        }

        public void Register(IService service)
        {
            if (agentServices.Contains(service)) return;
            agentServices.Add(service);
        }
        public void Remove(IService service)
        {

            if (!agentServices.Contains(service)) return;
            agentServices.Remove(service);
        }
    }
}
