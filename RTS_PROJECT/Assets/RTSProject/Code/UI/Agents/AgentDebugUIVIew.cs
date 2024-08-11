using RTS.Core;
using UnityEngine;

namespace RTS.UI
{
    public class AgentDebugUIView : ServiceUIView<IAgentService>
    {
        public void OnRequestSpawn()
        {
            if (service == null) return;
            service.RequestSpawnAgent();
        }
        public void OnRequestRemove()
        {
            if (service == null) return;
            service.RequestRemoveAgent(null);
        }
        public void OnRequestRemoveAll()
        {
            if (service == null) return;
            service.RequestRemoveAllAgents();
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
}
