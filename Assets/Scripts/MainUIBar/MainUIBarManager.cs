using Jerre.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.UI
{
    public class MainUIBarManager : MonoBehaviour, IAFEventListener
    {
        public RectTransform entireMenuBarUIArea;
        public PlayerUIBarElement uiPlayerElementPrefab;

        private SortedDictionary<int, PlayerUIBarElement> playerToUiElement;

        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
            playerToUiElement = new SortedDictionary<int, PlayerUIBarElement>();
        }

        public PlayerUIBarElement GetUiBarElemntForPlayerNumber(int number)
        {
            return playerToUiElement[number];
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
            var width = entireMenuBarUIArea.rect.width / players.Count;
            for (var i = 0; i < players.Count; i++)
            {
                var player = players[i];
                var uiPlayerElementInstance = Instantiate(uiPlayerElementPrefab, entireMenuBarUIArea);
                uiPlayerElementPrefab.BackgroundColor = new Color(player.color.r, player.color.g, player.color.b, uiPlayerElementInstance.BackgroundColor.a);
                uiPlayerElementInstance.PlayerNumber = player.playerNumber;
                uiPlayerElementInstance.Position = new Vector3(i * width + 0.5f * width, 0f, 0f);
                uiPlayerElementInstance.health = player.MaxHealth;
                playerToUiElement.Add(player.playerNumber, uiPlayerElementInstance);

                AFEventManager.INSTANCE.PostEvent(AFEvents.PlayerMenuBarUICreated(player.playerNumber));
            }
        }
    }
}
