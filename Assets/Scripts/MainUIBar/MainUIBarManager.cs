using UnityEngine;
using Jerre.Events;
using System.Collections.Generic;
using Jerre.Utils;

namespace Jerre.UI
{
    public class MainUIBarManager : MonoBehaviour, IAFEventListener
    {
        private PlayerJoinLeaveHandlerHelper joinLeaveHandler;
        public RectTransform entireMenuBarUIArea;
        public UIElementContainer uiElementContainerPrefab;

        private SortedDictionary<int, UIElementContainer> playerToRect;

        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
            playerToRect = new SortedDictionary<int, UIElementContainer>();
            joinLeaveHandler = new PlayerJoinLeaveHandlerHelper(
                join => HandleJoin(join),
                leave => HandleLeave(leave)
            );
        }
        
        void Start()
        {
            
        }

        private void HandleJoin(PlayerJoinPayload joinPayload)
        {
            var newContainer = Instantiate(uiElementContainerPrefab, entireMenuBarUIArea);
            newContainer.SetBackgroundColor(PlayerFetcher.FindPlayerByPlayerNumber(joinPayload.playerNumber).color);
            playerToRect.Add(joinPayload.playerNumber, newContainer);
            UpdateSizes();
            joinLeaveHandler.PostEvent(AFEvents.PlayerMenuBarUICreated(joinPayload.playerNumber));
        }

        private void HandleLeave(PlayerLeavePayload leavePayload)
        {
            var containerToRemove = playerToRect[leavePayload.playerNumber];
            playerToRect.Remove(leavePayload.playerNumber);
            Destroy(containerToRemove.gameObject);
            UpdateSizes();
        }

        private void UpdateSizes()
        {
            var counter = 0;
            var width = 1f / playerToRect.Count;
            foreach (var entry in playerToRect)
            {
                entry.Value.SetSize(counter * width, ++counter * width);
            }
        }

        public UIElementContainer GetRectTransformHolderForPlayerNumber(int playerNumber)
        {
            return playerToRect[playerNumber];
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            return joinLeaveHandler.HandleEvent(afEvent);
        }
    }
}
