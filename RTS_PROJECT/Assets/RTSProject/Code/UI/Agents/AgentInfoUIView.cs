using RTS.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace RTS.UI
{
    public class AgentInfoUIView : ServiceUIView<IAgentService>
    {
        [SerializeField] private TextMeshProUGUI amountTMP;
        [SerializeField] private Dictionary<UniqueID, AgentUIElement> infoList = new();
        [SerializeField] private AgentUIElement elementPrefab;
        [SerializeField] private Transform root;
        private ObjectPool<AgentUIElement> pool;

        private void OnEnable()
        {
            pool = new ObjectPool<AgentUIElement>(CreateElement);
            if (service == null)
            {
                Debug.LogError($"There is no service of type {nameof(IAgentService)}");
                enabled = false;
            }
            service.OnAgentSpawned += OnAgentSpawned;
            service.OnAgentRemoved += OnAgentRemoved;
            service.OnAgentArrived += OnAgentArrived;
        }

        private void OnDisable()
        {
            if (service == null) return;
            service.OnAgentSpawned -= OnAgentSpawned;
            service.OnAgentRemoved -= OnAgentRemoved;
            service.OnAgentArrived -= OnAgentArrived;
        }

        private void OnAgentSpawned(UniqueID id)
        {
            SpawnNotice($"[{id}] {("was added").Color(Color.green)}");
            UpdateLabel();
        }

        private void UpdateLabel() => amountTMP.SetText(service.AgentCount.ToString());

        private void OnAgentArrived(UniqueID id) => SpawnNotice($"[{id}] {("arrived").Color(Color.cyan)}");
        private void OnAgentRemoved(UniqueID id)
        {
            SpawnNotice($"[{id}] {("was removed").Color(Color.red)}");
            UpdateLabel();
        }

        private void SpawnNotice(string message)
        {
            var item = pool.Get();
            item.gameObject.SetActive(true);
            item.Show(message);
            item.OnAgentFadeComplete += Item_OnAgentFadeComplete;
        }

        private void Item_OnAgentFadeComplete(AgentUIElement obj)
        {
            obj.OnAgentFadeComplete -= Item_OnAgentFadeComplete;
            pool.Release(obj);
            obj.gameObject.SetActive(false);
        }
        private AgentUIElement CreateElement()
        {
            return Instantiate(elementPrefab, root);
        }
    }
}
