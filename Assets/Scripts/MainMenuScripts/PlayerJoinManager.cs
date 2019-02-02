using System.Collections.Generic;
using UnityEngine;

namespace Jerre.MainMenu
{
    public class PlayerJoinManager : MonoBehaviour
    {
        public RectTransform WaitingForPlayerJoin;
        public RectTransform JoinedPlayers;

        private ColorManager colorManager;

        private Dictionary<int, PlayerMenuSettings> playerNumberMap;    //int is the controller number, doesn't have to match the player number

        private void Awake()
        {
            playerNumberMap = new Dictionary<int, PlayerMenuSettings>();
        }

        void Start()
        {
            DisableAllChildren(JoinedPlayers);
            colorManager = GetComponent<ColorManager>();
        }

        void Update()
        {
            for (int i = 1; i <= 4; i++)
            {
                var joinLeaveKeyName = PlayerInputTags.JOIN_LEAVE + i;
                var leaveKeyName = PlayerInputTags.FIRE2 + i;
                if (Input.GetButtonDown(joinLeaveKeyName))
                {
                    if (!playerNumberMap.ContainsKey(i))
                    {
                        AddPlayer(i);
                    }
                }
                else if (Input.GetButtonDown(leaveKeyName)) {
                    RemovePlayer(i);
                }
            }
        }

        void DisableAllChildren(RectTransform rectTransform)
        {
            for (var i = 0; i < rectTransform.childCount; i++) {
                rectTransform.GetChild(i).gameObject.SetActive(false);
            }
        }

        void AddPlayer(int controllerIndex)
        {
            if (playerNumberMap.ContainsKey(controllerIndex))
            {
                return;
            }

            for (var i = 0; i < JoinedPlayers.childCount; i++)
            {
                var child = JoinedPlayers.GetChild(i).gameObject;
                if (!child.activeInHierarchy)
                {
                    WaitingForPlayerJoin.GetChild(i).gameObject.SetActive(false);
                    child.SetActive(true);
                    var playerSettings = child.GetComponentInChildren<PlayerMenuSettings>();
                    playerSettings.Color = colorManager.ExtractNextColor();
                    playerSettings.Number = controllerIndex;
                    var colorScript = child.GetComponentInChildren<PlayerColorScript>();
                    colorScript.ColorManager = colorManager;
                    colorScript.UpdateColor();
                    playerNumberMap.Add(controllerIndex, playerSettings);
                    break;
                }
            }

        }

        void RemovePlayer(int playerNumber)
        {
            if (!playerNumberMap.ContainsKey(playerNumber))
            {
                return;
            }

            var index = FindIndexForPlayerNumber(playerNumber, JoinedPlayers);
            var playerChild = JoinedPlayers.GetChild(index);
            var playerSettings = playerChild.GetComponentInChildren<PlayerMenuSettings>();
            colorManager.ReturnColor(playerSettings.Color);
            playerChild.gameObject.SetActive(false);
            WaitingForPlayerJoin.GetChild(index).gameObject.SetActive(true);
            playerNumberMap.Remove(playerNumber);
        }

        private int FindIndexForPlayerNumber(int playerNumber, RectTransform playerContainer)
        {
            for (var i = 0; i < playerContainer.childCount; i++)
            {
                var settings = playerContainer.GetChild(i).GetComponentInChildren<PlayerMenuSettings>();
                if (settings.Number == playerNumber) return i;
            }
            return -1;
        }
    }
}
