using System;
using System.Collections.Generic;
using System.Linq;

namespace RTS.Core
{
    public class ServiceManager : Singleton<ServiceManager>
    {
        private List<IService> services = new();

        public T Get<T>() where T : IService => (T)services.FirstOrDefault(x => x is T);

        public override void Initialize()
        {
            base.Initialize();
            services = new();
        }

        public void Register(IService service)
        {
            if (services.Contains(service)) return;
            services.Add(service);
        }
        public void Remove(IService service)
        {

            if (!services.Contains(service)) return;
            services.Remove(service);
        }
    }
}
