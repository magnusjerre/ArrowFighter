using UnityEngine;
using UnityEngine.UI;

namespace Jerre.MainMenu
{
    [RequireComponent(typeof(PlayerMenuSettings))]
    public class PlayerColorScript : MonoBehaviour
    {
        public ColorManager ColorManager;
        public Image image;

        private PlayerMenuSettings settings;

        
        void Start()
        {
            settings = GetComponent<PlayerMenuSettings>();
            UpdateColor();
        }

        public void Update()
        {
            if (!settings.mm_CanListenForInput) return;
            var changeColor = Input.GetButtonDown(PlayerInputTags.DODGE_RIGHT + settings.Number);
            if (settings.mm_Ready && changeColor)
            {
                Debug.Log("Player is ready, can't change color");
                return;
            } else if (changeColor)
            {
                Debug.Log("Player " + settings.Number + " is changing color!");
                settings.Color = ColorManager.SwapForNextColor(settings.Color);
                UpdateColor();
            }
        }

        public void UpdateColor()
        {
            image.color = settings.Color;
        }
    }
}
