using System.Collections.Generic;
using UnityEngine;

namespace Jerre.GameMode.Undead
{
    public class WholeGameUndeadScore
    {
        private Dictionary<int, List<SingleRoundUndeadScore>> rounds;


        public WholeGameUndeadScore()
        {
            rounds = new Dictionary<int, List<SingleRoundUndeadScore>>();
        }

        public void NewRound()
        {
            rounds.Add(rounds.Count + 1, new List<SingleRoundUndeadScore>());
        }

        public SingleRoundUndeadScore GetCurrentRoundScoreForPlayer(int playerNumber)
        {
            if (rounds.Count == 0)
            {
                throw new System.Exception("No rounds started");
            }

            var currentRoundScores = rounds[rounds.Count];
            var currentScore = currentRoundScores.Find(score => score.PlayerNumber == playerNumber);
            if (currentScore != null) return currentScore;
            Debug.Log("Didn't find score for player " + playerNumber + ", making a new one");
            currentScore = new SingleRoundUndeadScore(playerNumber, rounds.Count);
            currentRoundScores.Add(currentScore);
            return currentScore;
        }

        public bool AllPlayersDead()
        {
            return rounds[rounds.Count].TrueForAll(score => score.Undead);
        }

        public int RoundCount()
        {
            return rounds.Count;
        }

        public Dictionary<int, int> CalculatePlayerScoresWithoutPositionOrColor()
        {
            var scoreDict = new Dictionary<int, int>();

            foreach (var keyValue in rounds)
            {
                foreach (var roundScore in keyValue.Value)
                {
                    if (!scoreDict.ContainsKey(roundScore.PlayerNumber))
                    {
                        scoreDict.Add(roundScore.PlayerNumber, 0);
                    }

                    scoreDict[roundScore.PlayerNumber] = scoreDict[roundScore.PlayerNumber] + roundScore.Score;
                }
            }

            return scoreDict;
        }
    }
}