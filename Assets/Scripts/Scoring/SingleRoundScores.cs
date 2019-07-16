using System;
using System.Collections.Generic;

namespace Jerre
{
    public class SingleRoundScores<T> where T: IScore
    {
        private List<T> scores = new List<T>();
        public List<T> Scores
        {
            get
            {
                return scores;
            }
        }

        public List<T> SortedByDescendingScores()
        {
            var output = new List<T>(scores.Count);
            foreach (T score in scores)
            {
                output.Add(score);
            }
            output.Sort(new ScoreDescendingComparer<T>());
            return output;
        }

        public T GetScoreForPlayer(int playerNumber)
        {
            var playerScore = scores.Find(score => score.PlayerNumber() == playerNumber);
            if (playerScore != null)
            {
                return playerScore;
            }
            return default(T);
        }

        public void AddScoreForPlayer(T newScore)
        {
            scores.Add(newScore);
        }

        public bool DoTrueForAllCheck(Predicate<T> predicate)
        {
            return scores.TrueForAll(predicate);
        }
    }
}
