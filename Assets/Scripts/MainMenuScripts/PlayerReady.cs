using UnityEngine;

namespace Jerre.MainMenu
{
    [RequireComponent(typeof (PlayerMenuSettings))]
    public class PlayerReady : MonoBehaviour
    {
        public PlayerJoinManager playerJoinManager;
        public RectTransform PlayerReadyTransform;
        private PlayerMenuSettings settings;

        void Start()
        {
            settings = GetComponent<PlayerMenuSettings>();
            Reset();
        }

        void Update()
        {
            if (!settings.mm_CanListenForInput) return; 
            if (Input.GetButtonDown(PlayerInputTags.ACCEPT + settings.Number))
            {
                settings.mm_Ready = true;
                PlayerReadyTransform.gameObject.SetActive(settings.mm_Ready);
                playerJoinManager.NotifyPlayerReady(settings.Number);
            }
            else if (Input.GetButton(PlayerInputTags.FIRE2 + settings.Number))
            {
                settings.mm_Ready = false;
                PlayerReadyTransform.gameObject.SetActive(settings.mm_Ready);
                playerJoinManager.NotifyPlayerNotReady(settings.Number);
            }
        }

        public void Reset()
        {
            settings.mm_Ready = false;
            PlayerReadyTransform.gameObject.SetActive(settings.mm_Ready);
        }
    }
}
