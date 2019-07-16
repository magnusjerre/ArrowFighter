using UnityEngine;

namespace Jerre
{
    public class SimpleScore : IScore
    {
        private Color playerColor;
        private int playerNumber;
        private int score;

        public SimpleScore(Color playerColor, int playerNumber, int score)
        {
            this.playerColor = playerColor;
            this.playerNumber = playerNumber;
            this.score = score;
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

        public static SimpleScore Accumulate(SimpleScore acc, SimpleScore current)
        {
            return new SimpleScore(acc.playerColor, acc.playerNumber, acc.score + current.score);
        }
    }
}
