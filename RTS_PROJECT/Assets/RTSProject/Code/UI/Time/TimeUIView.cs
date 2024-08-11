using RTS.Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RTS.UI
{
    public class TimeUIView : ServiceUIView<ITickService>
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private Transform buttonRoot;
        [SerializeField] private List<TimeButtonConfig> buttonConfigs = new();
        [System.Serializable]
        private class TimeButtonConfig
        {
            [field: SerializeField] public ETickRate Rate { get; set; }
            [field: SerializeField] public Sprite Sprite { get; set; }
        }

        [SerializeField] private TextMeshProUGUI speedTMP;
        protected override void OnAwake()
        {
            base.OnAwake();
            CreateButtons();
            service.OnTickChanged += UpdateLabel;
            UpdateLabel(0.0f, service.TickRate);
        }

        private void CreateButtons()
        {
            foreach (var config in buttonConfigs)
            {
                var instance = Instantiate(buttonPrefab, buttonRoot);
                var button = instance.GetComponent<TimeUIElement>();
                button.Set(config.Sprite, () => SetSpeed(config.Rate));
            }
        }

        public void SetSpeed(ETickRate rate) => service.SetTickRate(rate);


        private void UpdateLabel(float oldRate, float newRate)
        {
            speedTMP.SetText(newRate.ToString());
        }
    }
}
