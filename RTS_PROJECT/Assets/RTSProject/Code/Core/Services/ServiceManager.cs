using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTS.Core
{
    public static class ServiceManager
    {
        private static readonly List<IService> services = new();

        public static T Get<T>() where T : IService
        {
            T service = (T)services.FirstOrDefault(x => x is T);
            if (service == null)
            {
                Debug.LogError($"Cannot find {nameof(IService)} of type {typeof(T)}. Did you forget to register?");
            }
            return service;
        }

        public static void Register(IService service)
        {
            if (services.Contains(service)) return;
            services.Add(service);
        }
        public static void Remove(IService service)
        {

            if (!services.Contains(service)) return;
            services.Remove(service);
        }
    }
}
