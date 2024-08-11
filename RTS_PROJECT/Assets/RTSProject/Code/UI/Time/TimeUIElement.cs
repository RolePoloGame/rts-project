using System;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI
{
    public class TimeUIElement : UIElement
    {
        [SerializeField] private Button button;
        [SerializeField] private Image icon;
        private event Action onClick;
        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }
        public void Set(Sprite sprite, Action onClick)
        {
            this.onClick = onClick;
            icon.sprite = sprite;
        }

        public void OnClick() => onClick?.Invoke();
    }
}
