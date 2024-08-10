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

        public override void Initialize()
        {
            base.Initialize();
            RegisterAgentService();
        }

        private void OnDestroy()
        {
            RemoveAgentService();
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
            //TODO: Instantiate Agent
            return null;
        }


        #region IAgentService
        public void RegisterAgentService() => ServiceManager.Instance.Register(this);
        public void RemoveAgentService() => ServiceManager.Instance.Remove(this);

        public void RequestRemoveAgent(UniqueID entityID)
        {
            HandleAgentRemove(entityID);
            OnAgentRemoved?.Invoke(entityID);
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
