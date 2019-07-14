using UnityEngine;
using Jerre.Events;
using Jerre.UI;

namespace Jerre.Health
{
    public class HealthUIManager : MonoBehaviour, IAFEventListener
    {
        public MainUIBarManager mainUIBarCanvas;
        //public HealthUIElement healthUIElementPrefab;

        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
        }

        void Start()
        {
            
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type)
            {
                case AFEventType.HEALTH_DAMAGE:
                    {
                        var payload = (HealthDamagePayload)afEvent.payload;
                        var uiElement = mainUIBarCanvas.GetUiBarElemntForPlayerNumber(payload.DamagedPlayerNumber);
                        uiElement.SetHealth(payload.HealthLeft);
                        break;
                    }
                case AFEventType.RESPAWN:
                    {
                        var payload = (RespawnPayload)afEvent.payload;
                        var uiElement = mainUIBarCanvas.GetUiBarElemntForPlayerNumber(payload.PlayerNumber);
                        uiElement.SetHealth(payload.Health);
                        break;
                    }
            }

            return false;
        }
    }
}
