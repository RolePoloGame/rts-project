using RTS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RTS.Agents
{
    public class AgentManager : Singleton<AgentManager>, IAgentService, ITickReceiver
    {
        public event Action<UniqueID> OnAgentSpawned;
        public event Action<UniqueID> OnAgentRemoved;
        public event Action<UniqueID> OnAgentSelectedNewTarget;
        public event Action<UniqueID> OnAgentArrived;

        [SerializeField] private Dictionary<UniqueID, Agent> spawnedAgents = new();
        [SerializeField] private GameObject prefab;
        public int AgentCount => spawnedAgents.Count;

        public override void Initialize()
        {
            base.Initialize();
            RegisterService(); 

            ITickService tickService = ServiceManager.Get<ITickService>();
            if (tickService != null)
                tickService.OnTickChanged += SetTickSpeed;
        }
        private void OnDestroy()
        {
            RemoveService();
            ITickService tickService = ServiceManager.Get<ITickService>();
            if (tickService != null)
                tickService.OnTickChanged -= SetTickSpeed;
        }

        protected virtual void HandleAgentRemove(UniqueID entityID)
        {
            if (!spawnedAgents.ContainsKey(entityID)) return;
            var agent = spawnedAgents[entityID];
            spawnedAgents.Remove(entityID);
            DestroyImmediate(agent.gameObject);
            OnAgentRemoved?.Invoke(entityID);
            //ToDo: Pooling
        }

        protected virtual Agent HandleAgentSpawn()
        {
            var agent = SpawnNewAgent();
            if (agent == null) return null;
            agent.OnArrived += OnAgentArrived;
            agent.ChangeSpeed(ServiceManager.Get<ITickService>().TickRate);
            spawnedAgents.Add(agent.Data.ID, agent);
            return agent;
        }

        private Agent SpawnNewAgent()
        {
            var instance = Instantiate(prefab, null);
            return instance.GetComponent<Agent>();
        }

        #region ITickReceiver
        public void SetTickSpeed(float oldSpeed, float newSpeed)
        {
            foreach (var agent in spawnedAgents.Values)
            {
                agent.ChangeSpeed(newSpeed);
            }
        }
        #endregion

        #region IAgentService
        public void RegisterService() => ServiceManager.Register(this);
        public void RemoveService() => ServiceManager.Remove(this);

        public void RequestRemoveAgent(UniqueID entityID)
        {
            if (entityID == null)
            {
                if (spawnedAgents.Count == 0) return;
                int index = 0;
                if (spawnedAgents.Count > 1)
                    index = UnityEngine.Random.Range(0, spawnedAgents.Count - 1);
                entityID = spawnedAgents.Keys.ToList()[index];
            }
            HandleAgentRemove(entityID);
        }
        public void RequestSpawnAgent()
        {
            var agent = HandleAgentSpawn();
            if (agent == null) return;
            OnAgentSpawned?.Invoke(agent.Data.ID);
        }
        public void RequestRemoveAllAgents()
        {
            var agents = spawnedAgents.Keys.ToList();
            var agentsCount = agents.Count;
            for (var i = 0; i < agentsCount; i++)
                RequestRemoveAgent(agents[i]);
        }
        #endregion
    }
}
