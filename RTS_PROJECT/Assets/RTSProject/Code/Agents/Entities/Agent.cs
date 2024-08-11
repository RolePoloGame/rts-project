using System;
using UnityEngine;

namespace RTS.Agents
{
    public class Agent : MovingEntity
    {
        [SerializeField] private Pathfinder pathfinder;
        public void ChangeSpeed(float speedFactor) => pathfinder.SetSpeedFactor(speedFactor);

        public void OnEnable()
        {

        }
    }
}
