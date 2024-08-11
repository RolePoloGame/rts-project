using RTS.Core;
using UnityEngine;

namespace RTS.UI
{
    public class UIView : MonoBehaviour
    {

    }

    public class ServiceUIView<T> : UIView where T : IService
    {
        protected T service;
        protected virtual void Awake()
        {
            service = ServiceManager.Instance.Get<T>();
        }
    }

}
