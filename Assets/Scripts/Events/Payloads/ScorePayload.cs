namespace Jerre.Events
{
    public struct ScorePayload
    {
        public int playerNumber;
        public int playerScore;
        public int maxScore;

        public ScorePayload(int playerNumber, int playerScore, int maxScore)
        {
            this.playerNumber = playerNumber;
            this.playerScore = playerScore;
            this.maxScore = maxScore;
        }
    }
}
