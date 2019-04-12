using UnityEngine;
using Jerre.UI;
using System.Collections.Generic;
using Jerre.Events;
using UnityEngine.SceneManagement;

namespace Jerre
{
    public class ScoreManager : MonoBehaviour, IAFEventListener
    {
        public int maxScore = 10;

        private ScoreUICanvas scoreUIManager;
        private AFEventManager eventManager;
        private Dictionary<int, int> playerScores;

        void Awake()
        {
            playerScores = new Dictionary<int, int>();
            scoreUIManager = GameObject.FindObjectOfType<ScoreUICanvas>();
            eventManager = GetComponent<AFEventManager>();
            eventManager.AddListener(this);
        }

        // Use this for initialization
        void Start()
        {
            
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
                eventManager.PostEvent(AFEvents.Score(payload.playerNumberOfKiller, newScore, maxScore));

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
                    eventManager.PostEvent(AFEvents.GameOver(payload.playerNumberOfKiller));
                    SceneManager.LoadScene(SceneNames.GAME_OVER_SCENE, LoadSceneMode.Single);
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
