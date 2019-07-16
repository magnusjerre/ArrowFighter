using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class CompleteGameScores<T> where T : IScore
    {
        public List<SingleRoundScores<T>> roundScores = new List<SingleRoundScores<T>>();

        public SingleRoundScores<T> StartNewRound()
        {
            var singleRoundScores = new SingleRoundScores<T>();
            roundScores.Add(singleRoundScores);
            return singleRoundScores;
        }

        public int CurrentRoundNumber
        {
            get
            {
                return roundScores.Count;
            }
        }

        public SingleRoundScores<T> GetCurrentRoundScores()
        {
            return roundScores[roundScores.Count - 1];
        }

        public List<T> CalculateTotalScoresByDescendingScore(Func<T, T, T> scoreAccumulator)
        {
            Dictionary<int, T> sumScores = new Dictionary<int, T>();
            foreach (SingleRoundScores<T> singleRoundScores in roundScores)
            {
                foreach (T score in singleRoundScores.Scores)
                {
                    if (sumScores.ContainsKey(score.PlayerNumber()))
                    {
                        var accumulatedPlayerscore = sumScores[score.PlayerNumber()];
                        sumScores[score.PlayerNumber()] = scoreAccumulator(accumulatedPlayerscore, score);
                    } else
                    {
                        sumScores.Add(score.PlayerNumber(), score);
                    }
                }
            }
            List<T> output = new List<T>();
            foreach (T score in sumScores.Values)
            {
                output.Add(score);
            }
            output.Sort(new ScoreDescendingComparer<T>());
            Debug.Log("CompleteGameScores.CalculateTotalScoresByDescending.count: " + output.Count);
            return output;
        }

    }
}
