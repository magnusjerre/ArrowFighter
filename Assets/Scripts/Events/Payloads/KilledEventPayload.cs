namespace Jerre.Events
{
    public struct KilledEventPayload
    {
        public int playerNumberOfKiller;
        public int playerNumberOfKilledPlayer;

        public KilledEventPayload(int killerNumber, int playerNumber)
        {
            playerNumberOfKiller = killerNumber;
            playerNumberOfKilledPlayer = playerNumber;
        }
    }
}
