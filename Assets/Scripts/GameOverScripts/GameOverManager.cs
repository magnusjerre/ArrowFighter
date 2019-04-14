using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jerre.GameOver
{
    public class GameOverManager : MonoBehaviour
    {
        [SerializeField]
        private ScoreEntry ScoreEntryPrefab;
        [SerializeField]
        private RectTransform ScoreContainer;

        public int PlayerCount;
        public bool Debug = false;

        void Start()
        {
            var offsetCounter = 0;
            foreach (var score in PlayersState.INSTANCE.Scores)
            {
                var scoreEntry = Instantiate(ScoreEntryPrefab, ScoreContainer);
                var verticalPosition = new Vector2(1f - (offsetCounter + 1) * scoreEntry.GetHeight(), 1f - offsetCounter * scoreEntry.GetHeight());
                scoreEntry.SetHeightAnchoredPosition(verticalPosition.x, verticalPosition.y);
                scoreEntry.PlayerColor = score.PlayerColor;
                scoreEntry.Position = score.Position;
                scoreEntry.Score = score.Score;
                scoreEntry.Debug = Debug;
                offsetCounter++;
            }
            Invoke("LoadFirstMenuScene", 3f);
        }

        void LoadFirstMenuScene()
        {
            SceneManager.LoadScene(SceneNames.GAME_MODE_SELECTION, LoadSceneMode.Single);
        }
    }
}
