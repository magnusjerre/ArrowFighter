using UnityEngine;

namespace Jerre.Events
{
    public struct GameOverPayload
    {
        public int winnerPlayerNumber;
        public int score;
        public Color playerColor;
        
        public GameOverPayload(int winnerPlayerNumber, int score, Color playerColor)
        {
            this.winnerPlayerNumber = winnerPlayerNumber;
            this.score = score;
            this.playerColor = playerColor;
        }
    }
}
