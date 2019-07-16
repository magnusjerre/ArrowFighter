using UnityEngine;

namespace Jerre.GameMode.Undead
{
    public class SingleRoundUndeadScore : IScore
    {
        private Color playerColor;
        private int playerNumber;
        private int score;
        public int Deaths;
        public bool Undead = false;
        public bool StartedAsUndead = false;

        public SingleRoundUndeadScore(Color playerColor, int playerNumber, int score, int deaths, bool undead, bool startedAsUndead)
        {
            this.playerColor = playerColor;
            this.playerNumber = playerNumber;
            this.score = score;
            Deaths = deaths;
            Undead = undead;
            StartedAsUndead = startedAsUndead;
        }

        public SingleRoundUndeadScore(int playerNumber, Color playerColor)
        {
            this.playerNumber = playerNumber;
            this.playerColor = playerColor;
        }

        public Color PlayerColor()
        {
            return playerColor;
        }

        public int PlayerNumber()
        {
            return playerNumber;
        }

        public int Score()
        {
            return score;
        }

        public void IncreaseScoreBy(int amount)
        {
            score += amount;
        }

        public static SingleRoundUndeadScore Accumulator(SingleRoundUndeadScore acc, SingleRoundUndeadScore current)
        {
            return new SingleRoundUndeadScore(acc.playerColor, acc.playerNumber, acc.score + current.score, acc.Deaths + current.Deaths, false, acc.StartedAsUndead || current.StartedAsUndead);
        }
    }
}
