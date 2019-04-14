using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Jerre.MainMenu.GameMode
{
    public class TextHighlightButton : Button
    {
        public Text indicator;
        public Text buttonText;

        protected override void Start()
        {
            base.Start();
            var textChildren = GetComponentsInChildren<Text>();
            for (var i = 0; i < textChildren.Length; i++)
            {
                if (textChildren[i] == targetGraphic)
                {
                    buttonText = textChildren[i];
                } else
                {
                    indicator = textChildren[i];
                }
            }

            indicator.color = colors.highlightedColor;
            indicator.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            indicator.enabled = true;
            //buttonText.fontStyle = FontStyle.Bold;
            //buttonText.color = colors.highlightedColor;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            indicator.enabled = false;
            //buttonText.fontStyle = FontStyle.Normal;
            //buttonText.color = colors.normalColor;
        }
    }
}
