using UnityEngine;
using Jerre.Events;
using Jerre.UI;

namespace Jerre.Health
{
    public class HealthUIManager : MonoBehaviour, IAFEventListener
    {
        public MainUIBarManager mainUIBarCanvas;
        public HealthUIElement healthUIElementPrefab;

        void Start()
        {
            AFEventManager.INSTANCE.AddListener(this);
        }

        private void HandlePlayerJoined(PlayerMenuBarUICreatedPayload payload)
        {
            var container = mainUIBarCanvas.GetRectTransformHolderForPlayerNumber(payload.PlayerNumber);
            var instance = Instantiate(healthUIElementPrefab, container.Left.transform);
            instance.PlayerNumber = payload.PlayerNumber;


            var allPlayerSettings = GameObject.FindObjectsOfType<PlayerSettings>();
            for (var i = 0; i < allPlayerSettings.Length; i++)
            {
                if (allPlayerSettings[i].playerNumber == payload.PlayerNumber)
                {
                    var settings = allPlayerSettings[i];
                    instance.PlayerColor = settings.color;
                    instance.InitialHealth = settings.MaxHealth;
                    break;
                }
            }
            
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type)
            {
                case AFEventType.PLAYER_MENU_BAR_UI_CREATED:
                    {
                        HandlePlayerJoined((PlayerMenuBarUICreatedPayload)afEvent.payload);
                        break;
                    }
            }
            return false;
        }
    }
}
