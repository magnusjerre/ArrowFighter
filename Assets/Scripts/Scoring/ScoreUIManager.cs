using Jerre.Events;
using Jerre.UI;
using System.Collections.Generic;
using UnityEngine;

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
            uiBarManager = GameObject.FindObjectOfType<MainUIBarManager>();
            AFEventManager.INSTANCE.AddListener(this);
        }

        private void AddScoreIndicatorsForPlayers(List<PlayerSettings> players)
        {
            for (var i = 0; i < players.Count; i++)
            {
                AddScoreIndicatorForPlayer(players[i]);
            }
        }

        private void AddScoreIndicatorForPlayer(PlayerSettings player)
        {
            var parent = uiBarManager.GetRectTransformHolderForPlayerNumber(player.playerNumber);
            var scoreIndicator = Instantiate(scoreUIElementPrefab, parent.Right);
            scoreIndicator.PlayerNumber = player.playerNumber;
            scoreIndicator.PlayerColor = player.color;
            scoreIndicator.MaxScore = scoreManager != null ? scoreManager.maxScore : -1;
            scoreIndicator.InitialScore = scoreManager != null ? scoreManager.GetPlayerScore(player.playerNumber) : -1;
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            if (afEvent.type == AFEventType.PLAYERS_ALL_CREATED)
            {
                var payload = (PlayersAllCreatedPayload)afEvent.payload;
                AddScoreIndicatorsForPlayers(payload.AllPlayers);
            }
            return false;
        }
    }
}
