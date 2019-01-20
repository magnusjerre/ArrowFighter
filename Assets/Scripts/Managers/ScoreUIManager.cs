using UnityEngine;

namespace Jerre.UI
{
    public class ScoreUIManager : MonoBehaviour
    {
        public RectTransform ScoreArea;
        public ScoreUIElement scoreUIPrefab;
        public int scorePadding = 10;
        private int nextNumberInLine = 1;

        void Start()
        {

        }

        public void AddScoreForPlayer(int playerScore, int maxScore, int playerNumber, Color color)
        {
            var scoreUI = Instantiate(scoreUIPrefab, ScoreArea);
            scoreUI.PlayerNumber = playerNumber;
            scoreUI.PlayerColor = color;
            scoreUI.NumberInLine = nextNumberInLine++;
            scoreUI.Padding = scorePadding;
            scoreUI.InitialScore = playerScore;
            scoreUI.MaxScore = maxScore;
        }

        public void UpdateScoreForPlayer(int playerScore, int maxScore, int playerNumber)
        {
            for (var i = 0; i < ScoreArea.childCount; i++)
            {
                var scoreUI = ScoreArea.GetChild(i).GetComponent<ScoreUIElement>();
                if (scoreUI != null && scoreUI.PlayerNumber == playerNumber)
                {
                    scoreUI.UpdateScore(playerScore, maxScore);
                    break;
                }
            }
        }

        public void RemoveScoreForPlayer(int playerNumber)
        {
            ScoreUIElement childToRemove = null;
            for (var i = 0; i < ScoreArea.childCount; i++)
            {
                var scoreUI = ScoreArea.GetChild(i).GetComponent<ScoreUIElement>();
                if (scoreUI != null && scoreUI.PlayerNumber == playerNumber)
                {
                    childToRemove = scoreUI;
                    break;
                }
            }

            if (childToRemove == null)
            {
                return;
            }

            Destroy(childToRemove.gameObject);

            nextNumberInLine = 1;
            for (var i = 0; i < ScoreArea.childCount; i++)
            {
                var scoreUI = ScoreArea.GetChild(i).GetComponent<ScoreUIElement>();
                if (scoreUI != null)
                {
                    scoreUI.NumberInLine = nextNumberInLine++;
                }
            }
        }
    }
}
