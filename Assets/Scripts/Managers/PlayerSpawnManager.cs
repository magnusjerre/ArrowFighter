using Jerre.Events;
using Jerre.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class PlayerSpawnManager : MonoBehaviour, IAFEventListener
    {
        public PlayerSettings playerPrefab;

        private Dictionary<int, PlayerSettings> playerNumberMap;
        private SpawnPointManager spawnPointManager;

        private Color[] playerColors;
        private int indexOfNextColor = 0;

        private bool CanJoinInGame = false;

        private void Awake()
        {
            playerNumberMap = new Dictionary<int, PlayerSettings>();
            playerColors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow };

            CanJoinInGame = PlayersState.INSTANCE.ReadyPlayersCount == 0;

            spawnPointManager = GameObject.FindObjectOfType<SpawnPointManager>();

            AFEventManager.INSTANCE.AddListener(this);
        }

        void Start()
        {
            if (!CanJoinInGame)
            {
                for (var i = 0; i < PlayersState.INSTANCE.ReadyPlayersCount; i++)
                {
                    var playerMenuSettings = PlayersState.INSTANCE.GetSettings(i);
                    AddPlayer(playerMenuSettings.Number, playerMenuSettings.Color);
                }
            }
        }

        void Update()
        {
            if (!CanJoinInGame) return;
            for (int i = 1; i <= 4; i++)
            {
                var joinLeaveKeyName = PlayerInputTags.JOIN_LEAVE + i;
                if (Input.GetButtonDown(joinLeaveKeyName)) {
                    if (!playerNumberMap.ContainsKey(i))
                    {
                        AddPlayer(i);
                    } else
                    {
                        RemovePlayer(i);
                    }
                }
            }
        }

        private void AddPlayer(int playerNumber)
        {
            AddPlayer(playerNumber, NextColor());
        }

        private void AddPlayer(int playerNumber, Color color)
        {
            var spawnPoint = spawnPointManager.GetNextSpawnPoint();
            var newPlayer = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            newPlayer.playerNumber = playerNumber;
            playerNumberMap.Add(playerNumber, newPlayer);
            newPlayer.color = color;
            var gameSettings = PlayersState.INSTANCE.gameSettings;
            newPlayer.MaxSpeed = gameSettings.baseSpeed;
            newPlayer.FireRate = gameSettings.baseFireRate;
            newPlayer.MaxHealth = gameSettings.baseHealth;
            AFEventManager.INSTANCE.PostEvent(AFEvents.PlayerJoin(playerNumber, color));
        }

        private void RemovePlayer(int playerNumber)
        {
            if (playerNumberMap.ContainsKey(playerNumber))
            {
                var playerToRemove = playerNumberMap[playerNumber];
                if (playerNumberMap.Remove(playerNumber))
                {
                    Destroy(playerToRemove.gameObject);
                }
            }
            AFEventManager.INSTANCE.PostEvent(AFEvents.PlayerLeave(playerNumber));
        }

        private Color NextColor()
        {
            var color = playerColors[indexOfNextColor];
            indexOfNextColor = (indexOfNextColor + 1) % playerColors.Length;
            return color;
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            if (afEvent.type == AFEventType.GAME_OVER)
            {
                for (var i = 1; i <= 4; i++)
                {
                    RemovePlayer(i);
                    indexOfNextColor = 0;
                    //scoreUIManager.ResetNumberInLine();
                }
            }
            return false;
        }
    }
}
