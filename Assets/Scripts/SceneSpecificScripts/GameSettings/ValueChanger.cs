using Jerre.GameSettings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jerre
{
    public class ValueChanger : Selectable
    {
        public KeyValueWithOptionsSetting backingSetting;

        private bool IsSelected;
        public Text ValueText;
        public Text LabelText;

        protected ValueChanger() : base()
        {

        }

        void Start()
        {
            LabelText.text = backingSetting.DisplayName.ToUpper();
            ValueText.text = backingSetting.Value;
        }

        void Update()
        {
            if (IsSelected)
            {
                if (Input.GetButtonDown(PlayerInputTags.DODGE_RIGHT + "1"))
                {
                    backingSetting.SetNextValue();
                    ValueText.text = backingSetting.Value;
                }
                else if (Input.GetButtonDown(PlayerInputTags.DODGE_LEFT + "1"))
                {
                    backingSetting.SetPreviousValue();
                    ValueText.text = backingSetting.Value;
                }
            }
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            IsSelected = true;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            IsSelected = false;
        }

        public float GetHeight()
        {
            var rectTransf = GetComponent<RectTransform>();
            return rectTransf.rect.height;
        }

        public void SetHeightPosition(float posX, float posY)
        {
            var rectTransf = GetComponent<RectTransform>();
            rectTransf.anchoredPosition = new Vector2(posX, posY);
        }
    }
}