using Jerre.Events;
using UnityEngine;

namespace Jerre.Managers
{
    public class GameStartDelayManager : MonoBehaviour, IAFEventListener
    {
        PlayerComponentsManager playerCompManager;
        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
            playerCompManager = GameObject.FindObjectOfType<PlayerComponentsManager>();
        }

        void Start()
        {
            
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type) {
                case AFEventType.PLAYER_MENU_BAR_UI_CREATED: {
                    playerCompManager.EnableOrDisableAllPlayersInputResponses(false);
                    Invoke("ReEnableAllPlayersInputResponses", PlayersState.INSTANCE.WaitTimeForPlayersToStart);
                    break;
                }
            }
            return false;
        }

        void ReEnableAllPlayersInputResponses()
        {
            playerCompManager.EnableOrDisableAllPlayersInputResponses(true);
        }
    }
}