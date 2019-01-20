namespace Jerre.Events
{
    public struct GameOverPayload
    {
        public int winnerPlayerNumber;

        public GameOverPayload(int winnerPlayerNumber)
        {
            this.winnerPlayerNumber = winnerPlayerNumber;
        }
    }
}
