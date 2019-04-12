using Jerre.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.UI
{
    public class ScoreUIElement : MonoBehaviour, IAFEventListener
    {
        private Text scoreText;
        private Button scoreButton;
        private RectTransform rectTransform;

        public int PlayerNumber;
        public Color PlayerColor;
        public int NumberInLine;
        public int Padding;
        public int MaxScore;
        public int InitialScore;

        void Start()
        {
            scoreText = GetComponent<Text>();
            scoreText.color = PlayerColor;
            rectTransform = GetComponent<RectTransform>();

            UpdateScore(InitialScore, MaxScore);
            AFEventManager.INSTANCE.AddListener(this);
        }

        private void UpdateScore(int currentScore, int maxScore)
        {
            scoreText.text = currentScore + " / " + maxScore;
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type)
            {
                case AFEventType.SCORE:
                    {
                        var payload = (ScorePayload)afEvent.payload;
                        if (payload.playerNumber == PlayerNumber)
                        {
                            UpdateScore(payload.playerScore, payload.maxScore);
                        }
                        break;
                    }
            }
            return false;
        }
    }
}
