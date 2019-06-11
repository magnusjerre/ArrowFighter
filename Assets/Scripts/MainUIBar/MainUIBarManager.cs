using Jerre.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.UI
{
    public class MainUIBarManager : MonoBehaviour, IAFEventListener
    {
        public RectTransform entireMenuBarUIArea;
        public UIElementContainer uiElementContainerPrefab;

        private SortedDictionary<int, UIElementContainer> playerToRect;

        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
            playerToRect = new SortedDictionary<int, UIElementContainer>();
        }

        public UIElementContainer GetRectTransformHolderForPlayerNumber(int playerNumber)
        {
            return playerToRect[playerNumber];
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            if (afEvent.type == AFEventType.PLAYERS_ALL_CREATED)
            {
                var payload = (PlayersAllCreatedPayload)afEvent.payload;
                AddUIForAllPlayers(payload.AllPlayers);
            }
            return false;
        }

        private void AddUIForAllPlayers(List<PlayerSettings> players)
        {
            var width = 1f / players.Count;
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var newContainer = Instantiate(uiElementContainerPrefab, entireMenuBarUIArea);
                newContainer.SetBackgroundColor(player.color);
                newContainer.SetSize(i * width, (i + 1) * width);
                playerToRect.Add(player.playerNumber, newContainer);
                AFEventManager.INSTANCE.PostEvent(AFEvents.PlayerMenuBarUICreated(player.playerNumber));
            }
        }
    }
}
