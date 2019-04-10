using UnityEngine;
using Jerre.Events;
using UnityEngine.UI;

namespace Jerre.Health
{
    [RequireComponent(typeof (Text))]
    public class HealthUIElement : MonoBehaviour, IAFEventListener
    {
        public int PlayerNumber;
        public Color PlayerColor;
        public int InitialHealth;

        private Text textElement;

        void Start()
        {
            textElement = GetComponent<Text>();
            textElement.color = PlayerColor;
            textElement.text = InitialHealth + " %";
            AFEventManager.INSTANCE.AddListener(this);
        }
        
        public bool HandleEvent(AFEvent afEvent)
        {
            switch (afEvent.type)
            {
                case AFEventType.HEALTH_DAMAGE:
                    {
                        var payload = (HealthDamagePayload)afEvent.payload;
                        if (payload.DamagedPlayerNumber == PlayerNumber)
                        {
                            textElement.text = payload.HealthLeft + " %";
                        }
                        
                        break;
                    }
                case AFEventType.RESPAWN:
                    {
                        var payload = (RespawnPayload)afEvent.payload;
                        if (payload.PlayerNumber == PlayerNumber)
                        {
                            textElement.text = payload.Health + " %";
                        }
                        break;
                    }
            }
            return false;
        }
    }
}
