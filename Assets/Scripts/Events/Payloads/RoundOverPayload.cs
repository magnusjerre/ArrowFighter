using UnityEngine;

namespace Jerre.Events
{
    public struct RoundOverPayload
    {
        public int roundWinnerPlayerNumber;
        public int roundScore;
        public Color playerColor;

        public RoundOverPayload(int roundWinnerPlayerNumber, int roundScore, Color playerColor)
        {
            this.roundWinnerPlayerNumber = roundWinnerPlayerNumber;
            this.roundScore = roundScore;
            this.playerColor = playerColor;
        }
    }
}
