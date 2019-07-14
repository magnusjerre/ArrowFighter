using Jerre.Events;
using UnityEngine;

namespace Jerre.UI.InGame
{
    public class ScoreUIManager : MonoBehaviour, IAFEventListener
    {
        private FreeForAllGameModeManager scoreManager;
        private MainUIBarManager uiBarManager;

        private void Awake()
        {
            scoreManager = GameObject.FindObjectOfType<FreeForAllGameModeManager>();
            uiBarManager = GameObject.FindObjectOfType<MainUIBarManager>();
            AFEventManager.INSTANCE.AddListener(this);
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch (afEvent.type)
            {
                case AFEventType.SCORE:
                    {
                        var payload = (ScorePayload)afEvent.payload;
                        var uiElement = uiBarManager.GetUiBarElemntForPlayerNumber(payload.playerNumber);
                        uiElement.SetScoreText("" + payload.playerScore);
                        break;
                    }
            }
            return false;
        }
    }
}
