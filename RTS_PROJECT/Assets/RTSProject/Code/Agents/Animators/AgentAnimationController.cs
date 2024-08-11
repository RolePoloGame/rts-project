using UnityEditor.Animations;
using UnityEngine;

namespace RTS.Agents
{
    [RequireComponent(typeof(Agent))]
    public class AgentAnimationController : MonoBehaviour
    {
        [SerializeField] private Agent agent;
        [SerializeField] private Animator animatorController;
        private void LateUpdate()
        {
            animatorController.SetFloat("Blend", agent.Pathfinder.Velocity / agent.Pathfinder.MaxVelocity);
        }
    }
}
