using RTS.Core;
using UnityEngine;

namespace RTS.Agents
{
    public class WorldEntity : MonoBehaviour
    {
        public EntityData Data => entityData;
        protected EntityData entityData;
    }
}
