using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Jerre.Utils;

namespace Jerre.MainMenu.GameSettings
{
    public class ValueChanger : Selectable
    {
        public GameSettingsField settingsField;
        public int theValue;

        public int step = 1;
        public int maxValue = 10, minValue = 1;

        private bool IsSelected;
        public Text valueText;

        protected ValueChanger() : base()
        {

        }

        void Start()
        {
            valueText.text = theValue + "";
            PlayersState.INSTANCE.gameSettings.SetValue(settingsField, theValue);
        }

        void Update()
        {
            if (IsSelected)
            {
                if (Input.GetButtonDown(PlayerInputTags.DODGE_RIGHT + "1"))
                {
                    theValue = Mathf.Min(theValue + step, maxValue);
                    valueText.text = theValue + "";
                    PlayersState.INSTANCE.gameSettings.SetValue(settingsField, theValue);
                }
                else if (Input.GetButtonDown(PlayerInputTags.DODGE_LEFT + "1"))
                {
                    theValue = Mathf.Max(theValue - step, minValue);
                    valueText.text = theValue + "";
                    PlayersState.INSTANCE.gameSettings.SetValue(settingsField, theValue);
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
    }
}