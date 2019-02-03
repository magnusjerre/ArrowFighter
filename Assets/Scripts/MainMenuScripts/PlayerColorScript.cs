using UnityEngine;
using UnityEngine.UI;

namespace Jerre.MainMenu
{
    [RequireComponent(typeof(PlayerMenuSettings))]
    public class PlayerColorScript : MonoBehaviour
    {
        public ColorManager ColorManager;

        private PlayerMenuSettings settings;

        
        void Start()
        {
            settings = GetComponent<PlayerMenuSettings>();
        }

        public void Update()
        {
            if (!settings.CanListenForInput) return;
            var changeColor = Input.GetButtonDown(PlayerInputTags.DODGE_RIGHT + settings.Number);
            if (settings.Ready && changeColor)
            {
                Debug.Log("Player is ready, can't change color");
                return;
            } else if (changeColor)
            {
                settings.Color = ColorManager.SwapForNextColor(settings.Color);
                UpdateColor();
            }
        }

        public void UpdateColor()
        {
            var imageComponents = transform.parent.GetComponentsInChildren<Image>();
            for (var i = 0; i < imageComponents.Length; i++)
            {
                imageComponents[i].color = settings.Color;
            }
        }
    }
}
