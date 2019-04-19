using UnityEngine;
using System.Collections.Generic;
using Jerre.Events;
using UnityEngine.SceneManagement;
using Jerre.GameSettings;

namespace Jerre
{
    public class ScoreManager : MonoBehaviour, IAFEventListener
    {
        public int maxScore = 10;

        private Dictionary<int, int> playerScores;

        void Awake()
        {
            //Debug.Log("pointsToWin:" + PlayersState.INSTANCE.gameSettings.pointsToWin);
            playerScores = new Dictionary<int, int>();
            AFEventManager.INSTANCE.AddListener(this);
            //maxScore = PlayersState.INSTANCE.gameSettings.pointsToWin;
            Debug.Log("pointsToWin:" + GameSettingsState.INSTANCE.pointsToWin);
            maxScore = GameSettingsState.INSTANCE.pointsToWin;
        }

        // Use this for initialization
        void Start()
        {
            Debug.Log("pointsToWin:" + GameSettingsState.INSTANCE.pointsToWin);
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch (afEvent.type)
            {
                case AFEventType.KILLED:
                    {
                        return HandleKilledEvent((KilledEventPayload)afEvent.payload);
                    }
                case AFEventType.PLAYER_JOIN:
                    {
                        return HandlePlayerJoinEvent((PlayerJoinPayload)afEvent.payload);
                    }
                case AFEventType.PLAYER_LEAVE:
                    {
                        return HandlePlayerLeaveEvent((PlayerLeavePayload)afEvent.payload);
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        private bool HandleKilledEvent(KilledEventPayload payload)
        {
            if (playerScores.ContainsKey(payload.playerNumberOfKiller))
            {
                var oldScore = playerScores[payload.playerNumberOfKiller];
                var newScore = oldScore + 1;
                playerScores[payload.playerNumberOfKiller] = newScore;
                AFEventManager.INSTANCE.PostEvent(AFEvents.Score(payload.playerNumberOfKiller, newScore, maxScore));

                if (newScore == maxScore)
                {
                    Debug.Log("Game over");
                    

                    var players = GameObject.FindObjectsOfType<PlayerSettings>();
                    var scores = new List<PlayerScore>();                    
                    for (var i = 0; i < players.Length; i++)
                    {
                        var p = players[i];
                        scores.Add(new PlayerScore(p.playerNumber, p.color, 0, playerScores[p.playerNumber]));
                    }
                    PlayersState.INSTANCE.SetScores(scores);
                    AFEventManager.INSTANCE.PostEvent(AFEvents.GameOver(payload.playerNumberOfKiller));
                }
            }
            else
            {
                playerScores.Remove(payload.playerNumberOfKilledPlayer);
            }

            return false;
        }

        private bool HandlePlayerJoinEvent(PlayerJoinPayload payload)
        {
            playerScores.Add(payload.playerNumber, 0);
            return false;
        }

        private bool HandlePlayerLeaveEvent(PlayerLeavePayload payload)
        {
            playerScores.Remove(payload.playerNumber);
            return false;
        }

        public Dictionary<int, int> GetPlayerScores()
        {
            return playerScores;
        }

        public int GetPlayerScore(int playerNumber)
        {
            return playerScores[playerNumber];
        }
    }
}
