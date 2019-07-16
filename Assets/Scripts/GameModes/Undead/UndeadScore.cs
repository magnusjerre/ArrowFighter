using UnityEngine;

namespace Jerre.GameMode.Undead
{
    public class UndeadScore : IScore
    {
        private Color playerColor;
        private int playerNumber;
        private int score;
        public int Deaths;
        public bool Undead = false;
        public bool StartedAsUndead = false;

        public UndeadScore(Color playerColor, int playerNumber, int score, int deaths, bool undead, bool startedAsUndead)
        {
            this.playerColor = playerColor;
            this.playerNumber = playerNumber;
            this.score = score;
            Deaths = deaths;
            Undead = undead;
            StartedAsUndead = startedAsUndead;
        }

        public UndeadScore(int playerNumber, Color playerColor)
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

        public static UndeadScore Accumulator(UndeadScore acc, UndeadScore current)
        {
            return new UndeadScore(acc.playerColor, acc.playerNumber, acc.score + current.score, acc.Deaths + current.Deaths, false, acc.StartedAsUndead || current.StartedAsUndead);
        }
    }
}
