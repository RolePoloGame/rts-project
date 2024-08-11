using RTS.Core;
using System;
using UnityEngine;

namespace RTS.UI
{
    public class UIView : MonoBehaviour
    {
        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            
        }
    }

    public class ServiceUIView<T> : UIView where T : IService
    {
        protected T service;
        protected override void OnAwake()
        {
            base.OnAwake();
            service = ServiceManager.Get<T>();
        }
    }

}
