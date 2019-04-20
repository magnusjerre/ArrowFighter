using System.Collections;
using System.Collections.Generic;
using Jerre.Events;
using Jerre.UIStuff;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.Scoring
{
    public class ScoreSummary : MonoBehaviour, IAFEventListener
    {
        public Text leaderScoreText, totalScoreText;

        private int leaderScore;
        private int leaderPlayerNumber;
        private Dictionary<int, Color> playerColorMap;

        void Awake() {
            AFEventManager.INSTANCE.AddListener(this);
        }
        
        void Start()
        {
            var scoreManager = GameObject.FindObjectOfType<ScoreManager>();
            Debug.Log("Max score");
            totalScoreText.text = scoreManager.maxScore + "";
            leaderScoreText.text = "0";

            InitializeColorMap();
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type) {
                case AFEventType.SCORE: {
                    var payload = (ScorePayload)afEvent.payload;
                    if (payload.playerScore > leaderScore) {
                        leaderScore = payload.playerScore;
                        leaderPlayerNumber = payload.playerNumber;
                        if (!playerColorMap.ContainsKey(leaderPlayerNumber)) {
                            InitializeColorMap();
                        }
                        leaderScoreText.color = playerColorMap[leaderPlayerNumber];
                        leaderScoreText.text = leaderScore + "";
                    }
                    break;
                }
            }
            return false;
        }
        void InitializeColorMap() {
            playerColorMap = new Dictionary<int, Color>();
            var playerSettings = GameObject.FindObjectsOfType<PlayerSettings>();
            for (var i = 0; i < playerSettings.Length; i++) {
                playerColorMap.Add(playerSettings[i].playerNumber, playerSettings[i].color);
            }
        }
    }
}
