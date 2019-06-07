using System.Collections.Generic;
using UnityEngine;

namespace Jerre.GameMode.Undead
{
    public class WholeGameUndeadScore
    {
        public List<SingleRoundUndeadScore> roundScores;


        public WholeGameUndeadScore()
        {
            roundScores = new List<SingleRoundUndeadScore>();
        }

        public SingleRoundUndeadScore GetCurrentRoundScoreForPlayer(int playerNumber)
        {
            var currentScore = roundScores.Find(score => score.PlayerNumber == playerNumber);
            if (currentScore != null) return currentScore;
            Debug.Log("Didn't find score for player " + playerNumber + ", making a new one");
            currentScore = new SingleRoundUndeadScore(playerNumber, roundScores.Count);
            roundScores.Add(currentScore);
            return currentScore;
        }

        public bool AllPlayersDead()
        {
            return roundScores.TrueForAll(score => score.Undead);
        }

        public Dictionary<int, int> CalculatePlayerScoresWithoutPositionOrColor(List<List<SingleRoundUndeadScore>> allRounds)
        {
            var scoreDict = new Dictionary<int, int>();

            foreach (var round in allRounds)
            {
                foreach (var score in round)
                {
                    if (!scoreDict.ContainsKey(score.PlayerNumber))
                    {
                        scoreDict.Add(score.PlayerNumber, 0);
                    }

                    scoreDict[score.PlayerNumber] = scoreDict[score.PlayerNumber] + score.Score;
                }
            }

            return scoreDict;
        }

        public Dictionary<int, int> CalcualtePlayerScoresWithoutPositionOrColorForRound(List<SingleRoundUndeadScore> round)
        {
            var scoreDict = new Dictionary<int, int>();

            foreach (var score in round)
            {
                if (!scoreDict.ContainsKey(score.PlayerNumber))
                {
                    scoreDict.Add(score.PlayerNumber, 0);
                }

                scoreDict[score.PlayerNumber] = scoreDict[score.PlayerNumber] + score.Score;
            }

            return scoreDict;
        }
    }
}