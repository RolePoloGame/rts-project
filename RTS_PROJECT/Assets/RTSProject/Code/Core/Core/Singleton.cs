using UnityEngine;

namespace RTS.Core
{
    public abstract class Singleton : MonoBehaviour
    {
        public bool IsInitialized => m_Init;
        protected bool m_Init = false;
        public virtual void Initialize() => Debug.Log($"{name} is initialized.");
    }
    public class Singleton<T> : Singleton where T : Component
    {
        public static T Instance { get; protected set; }

        private void Awake() => InstantiateSingleton();

        private void InstantiateSingleton()
        {
            if (Instance == this) return;

            if (Instance != null)
            {
                Debug.Log($"Instance of {name} already exists! Deleting duplicate...");
                Destroy(this);
                return;
            }
            Instance = this as T;
            Initialize();
            m_Init = true;
        }
    }
}