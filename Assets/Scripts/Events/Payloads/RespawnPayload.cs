namespace Jerre.Events
{
    public struct RespawnPayload
    {
        public int PlayerNumber;
        public int Health;

        public RespawnPayload(int playerNumber, int health)
        {
            PlayerNumber = playerNumber;
            Health = health;
        }
    }
}
