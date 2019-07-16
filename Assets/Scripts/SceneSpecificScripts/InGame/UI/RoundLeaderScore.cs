using Jerre.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    public class RoundLeaderScore : MonoBehaviour, IAFEventListener
    {
        public Text leaderScoreText;

        private int leaderScore;
        private int leaderPlayerNumber;
        private Color leaderColor;

        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
        }

        void Start()
        {
            leaderScoreText.text = "0";
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch (afEvent.type)
            {
                case AFEventType.SCORE:
                    {
                        var payload = (ScorePayload)afEvent.payload;
                        if (payload.playerScore > leaderScore)
                        {
                            leaderScore = payload.playerScore;
                            leaderPlayerNumber = payload.playerNumber;
                            leaderColor = PlayersState.INSTANCE.GetPlayerColor(leaderPlayerNumber);
                            leaderScoreText.color = leaderColor;
                            leaderScoreText.text = leaderScore + "";
                        }
                        break;
                    }
            }
            return false;
        }
    }
}
