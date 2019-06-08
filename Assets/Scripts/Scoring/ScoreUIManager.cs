using Jerre.UI;
using Jerre.Utils;
using UnityEngine;

namespace Jerre
{
    public class ScoreUIManager : MonoBehaviour
    {
        private ScoreManager scoreManager;

        public ScoreUIElement scoreUIElementPrefab;

        public MainUIBarManager uiBarManager;

        private void Awake()
        {
            scoreManager = GameObject.FindObjectOfType<ScoreManager>();
            uiBarManager = GameObject.FindObjectOfType<MainUIBarManager>();
        }

        void Start()
        {
            for (var i = 0; i < PlayersState.INSTANCE.ReadyPlayersCount; i++)
            {
                var playerMenuSettings = PlayersState.INSTANCE.GetSettings(i);
                HandleJoin(playerMenuSettings.Number);
            }
        }

        void HandleJoin(int playerNumber)
        {
            var settings = PlayerFetcher.FindPlayerByPlayerNumber(playerNumber);
            if (settings == null)
            {
                throw new System.Exception("Couldn't find player with playerNumber " + playerNumber);
            }

            var parent = uiBarManager.GetRectTransformHolderForPlayerNumber(playerNumber);
            var scoreIndicator = Instantiate(scoreUIElementPrefab, parent.Right);
            scoreIndicator.PlayerNumber = settings.playerNumber;
            scoreIndicator.PlayerColor = settings.color;
            scoreIndicator.MaxScore = scoreManager != null ? scoreManager.maxScore : -1;
            scoreIndicator.InitialScore = scoreManager != null ? scoreManager.GetPlayerScore(settings.playerNumber) : -1;
        }
    }
}
