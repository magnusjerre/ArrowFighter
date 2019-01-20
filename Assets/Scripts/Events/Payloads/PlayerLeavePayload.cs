namespace Jerre.Events
{
    public struct PlayerLeavePayload
    {
        public int playerNumber;

        public PlayerLeavePayload(int playerNumber)
        {
            this.playerNumber = playerNumber;
        }
    }
}
