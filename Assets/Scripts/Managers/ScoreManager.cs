using UnityEngine;
using Jerre.UI;
using System.Collections.Generic;
using Jerre.Events;

namespace Jerre
{
    public class ScoreManager : MonoBehaviour, IAFEventListener
    {
        public int maxScore = 10;

        private ScoreUIManager scoreUIManager;
        private Dictionary<int, int> playerScores;

        void Awake()
        {
            playerScores = new Dictionary<int, int>();
        }

        // Use this for initialization
        void Start()
        {
            scoreUIManager = GameObject.FindObjectOfType<ScoreUIManager>();
            AFEventManager.INSTANCE.AddListener(this);
        }

        public void AddPlayer(int playerNumber)
        {
            playerScores.Add(playerNumber, 0);
        }

        public void RemovePlayer(int playerNumber)
        {
            playerScores.Remove(playerNumber);
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            if (afEvent.type == AFEventType.KILLED)
            {
                var payload = (KilledEventPayload) afEvent.payload;
                if (playerScores.ContainsKey(payload.playerNumberOfKiller))
                {
                    var oldScore = playerScores[payload.playerNumberOfKiller];
                    var newScore = oldScore + 1;
                    playerScores[payload.playerNumberOfKiller] = newScore;
                    scoreUIManager.UpdateScoreForPlayer(newScore, maxScore, payload.playerNumberOfKiller);

                    if (newScore == maxScore)
                    {
                        Debug.Log("Game over");
                        AFEventManager.INSTANCE.PostEvent(AFEvents.GameOver(payload.playerNumberOfKiller));
                    }
                }
                else
                {
                    RemovePlayer(payload.playerNumberOfKiller);
                }

            }
            return false;
        }
    }
}
