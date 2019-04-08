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
            scoreText = GetComponentInChildren<Text>();
            scoreButton = GetComponent<Button>();
            rectTransform = GetComponent<RectTransform>();

            scoreButton.interactable = false;
            var colors = scoreButton.colors;
            colors.disabledColor = new Color(PlayerColor.r, PlayerColor.g, PlayerColor.b, colors.disabledColor.a);
            scoreButton.colors = colors;

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
