using RTS.Core;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RTS.UI
{
    public class AgentUIElement : MonoBehaviour
    {
        public event Action<AgentUIElement> OnAgentFadeComplete;
        [SerializeField] private TextMeshProUGUI textTMP;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeTime = .5f;
        [SerializeField] private float displayTime = 2.5f;
        [SerializeField] private EFadeType fadeType = EFadeType.Sigmoid;
        private enum EFadeType
        {
            Sigmoid,
            SineEaseInOut,
            EaseInOutBack,

        }
        public void Show(string value)
        {
            textTMP.SetText(value);
            StartCoroutine(FadeRoutine());
        }

        private IEnumerator FadeRoutine()
        {
            canvasGroup.alpha = 0.0f;
            yield return null;
            float time = 0.0f;
            while (time < fadeTime)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Evaluate(time / fadeTime);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(displayTime);
            time = 0.0f;
            while (time < fadeTime)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = 1.0f - Evaluate(time / fadeTime);
                yield return null;
            }
            OnAgentFadeComplete?.Invoke(this);
        }

        private float Evaluate(float value) => fadeType switch
        {
            EFadeType.SineEaseInOut => MathExtensions.SineEaseInOut(Mathf.Clamp01(value)),
            EFadeType.EaseInOutBack => MathExtensions.EaseInOutBack(Mathf.Clamp01(value)),
            _ => MathExtensions.Sigmoid(Mathf.Clamp01(value)),
        };
    }
}
