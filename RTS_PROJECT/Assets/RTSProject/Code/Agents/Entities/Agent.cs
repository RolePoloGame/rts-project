using RTS.Core;
using System;
using UnityEngine;

namespace RTS.Agents
{
    public class Agent : MovingEntity
    {
        [SerializeField] private Pathfinder pathfinder;

        public Action<UniqueID> OnArrived { get; internal set; }

        public void ChangeSpeed(float speedFactor) => pathfinder.SetSpeedFactor(speedFactor);

        public void OnEnable()
        {
            pathfinder.OnArrvied += HandleArrived;
        }
        private void OnDisable()
        {
            pathfinder.OnArrvied -= HandleArrived;
        }

        private void HandleArrived() => OnArrived?.Invoke(Data.ID);
    }
}
