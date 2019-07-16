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

        public R GetScoreForPlayer<R>(int playerNumber) where R : T
        {
            var playerScore = scores.Find(score => score.PlayerNumber() == playerNumber);
            if (playerScore != null)
            {
                return (R)playerScore;
            }
            return default(R);
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
