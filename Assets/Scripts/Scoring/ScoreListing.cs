using UnityEngine;

namespace Jerre
{
    public class ScoreListing : MonoBehaviour
    {
        [SerializeField]
        private ScoreEntry ScoreEntryPrefab;
        [SerializeField]
        private RectTransform ScoreContainer;
        public bool ShowCompleteGameScores;
        //public bool Debug = false;

        void Start()
        {
            Debug.Log("ScoreListing.Start");
            var offsetCounter = 0;
            var scores = ShowCompleteGameScores ?
                PlayersState.INSTANCE.gameScores.CalculateTotalScoresByDescendingScore(PlayersState.INSTANCE.gameScoresAccumulator) :
                PlayersState.INSTANCE.gameScores.GetCurrentRoundScores().SortedByDescendingScores();

            Debug.Log("PlayersState.currentRoundScores.Count: " + PlayersState.INSTANCE.gameScores.GetCurrentRoundScores().Scores.Count);
            Debug.Log("ScoreListing.Start::scores: " + scores.Count);

            var pos = 1;
            foreach (var score in scores)
            {
                Debug.Log("ScoreListing.Start.forEach, playerNumber: " + score.PlayerNumber());
                var scoreEntry = Instantiate(ScoreEntryPrefab, ScoreContainer);
                var verticalPosition = new Vector2(1f - (offsetCounter + 1) * scoreEntry.GetHeight(), 1f - offsetCounter * scoreEntry.GetHeight());
                scoreEntry.SetHeightAnchoredPosition(verticalPosition.x, verticalPosition.y);
                scoreEntry.PlayerColor = score.PlayerColor();
                scoreEntry.Position = pos++;
                scoreEntry.Score = score.Score();
                //scoreEntry.Debug = Debug;
                offsetCounter++;
            }
        }
    }
}
