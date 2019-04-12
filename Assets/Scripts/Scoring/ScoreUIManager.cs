using UnityEngine;
using Jerre.UI;
using Jerre.Events;
using Jerre.Utils;

namespace Jerre
{
    public class ScoreUIManager : MonoBehaviour, IAFEventListener
    {
        private ScoreManager scoreManager;

        public ScoreUIElement scoreUIElementPrefab;

        public MainUIBarManager uiBarManager;

        private void Awake()
        {
            scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        }

        void Start()
        {
            AFEventManager.INSTANCE.AddListener(this);
        }

        void HandleJoin(PlayerMenuBarUICreatedPayload joinPayload)
        {
            var settings = PlayerFetcher.FindPlayerByPlayerNumber(joinPayload.PlayerNumber);
            if (settings == null)
            {
                throw new System.Exception("Couldn't find player with playerNumber " + joinPayload.PlayerNumber);
            }

            var parent = uiBarManager.GetRectTransformHolderForPlayerNumber(joinPayload.PlayerNumber);
            var scoreIndicator = Instantiate(scoreUIElementPrefab, parent.Right);
            scoreIndicator.PlayerNumber = settings.playerNumber;
            scoreIndicator.PlayerColor = settings.color;
            scoreIndicator.MaxScore = scoreManager.maxScore;
            scoreIndicator.InitialScore = scoreManager.GetPlayerScore(settings.playerNumber);
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type)
            {
                case AFEventType.PLAYER_MENU_BAR_UI_CREATED:
                    {
                        HandleJoin((PlayerMenuBarUICreatedPayload)afEvent.payload);           
                        break;
                    }
            }
            return false;
        }
    }
}
