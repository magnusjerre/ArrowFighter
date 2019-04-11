using UnityEngine;
using UnityEngine.UI;

namespace Jerre.UI
{
    public class ScoreUIElement : MonoBehaviour
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

            var newXPos = (NumberInLine - 1) * (rectTransform.rect.width + Padding);
            rectTransform.anchoredPosition = new Vector2(newXPos, 0);

            UpdateScore(InitialScore, MaxScore);
        }

        public void UpdateScore(int currentScore, int maxScore)
        {
            scoreText.text = currentScore + " / " + maxScore;
        }
    }
}
