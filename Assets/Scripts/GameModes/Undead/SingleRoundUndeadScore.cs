namespace Jerre.GameMode.Undead
{
    public class SingleRoundUndeadScore
    {
        public int PlayerNumber;
        public int RoundNumber;
        public int Score;
        public int Deaths;
        public bool Undead = false;
        public bool StartedAsUndead = false;

        public SingleRoundUndeadScore(int PlayerNumber, int RoundNumber)
        {
            this.PlayerNumber = PlayerNumber;
        }

    }
}
