using RTS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace RTS.Agents
{
    public class AgentManager : Singleton<AgentManager>, IAgentService
    {
        public event Action<UniqueID> OnAgentSpawned;
        public event Action<UniqueID> OnAgentRemoved;
        public event Action<UniqueID> OnAgentSelectedNewTarget;
        public event Action<UniqueID> OnAgentArrived;

        [SerializeField] private Dictionary<UniqueID, Agent> spawnedAgents = new();
        [SerializeField] private GameObject prefab;
        public override void Initialize()
        {
            base.Initialize();
            RegisterService();
        }
        private void OnDestroy()
        {
            RemoveService();
        }

        protected virtual void HandleAgentRemove(UniqueID entityID)
        {
            if (!spawnedAgents.ContainsKey(entityID)) return;
            spawnedAgents.Remove(entityID);
        }

        protected virtual Agent HandleAgentSpawn()
        {
            var agent = SpawnNewAgent();
            if (agent == null) return null;
            spawnedAgents.Add(agent.Data.ID, agent);
            return agent;
        }

        private Agent SpawnNewAgent()
        {
            var instance = Instantiate(prefab, null);
            return instance.GetComponent<Agent>();
        }


        #region IAgentService
        public void RegisterService() => ServiceManager.Instance.Register(this);
        public void RemoveService() => ServiceManager.Instance.Remove(this);

        public void RequestRemoveAgent(UniqueID entityID)
        {
            HandleAgentRemove(entityID);
            OnAgentRemoved?.Invoke(entityID);
        }

        public void SetSpeed(float speed)
        {
            foreach (var agent in spawnedAgents.Values)
            {
                agent.ChangeSpeed(speed);
            }
        }


        public void RequestSpawnAgent()
        {
            var agent = HandleAgentSpawn();
            if (agent == null) return;
            OnAgentSpawned?.Invoke(agent.Data.ID);
        }
        #endregion
    }
}
