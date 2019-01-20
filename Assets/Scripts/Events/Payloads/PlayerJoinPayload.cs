using UnityEngine;
namespace Jerre.Events
{
    public struct PlayerJoinPayload
    {
        public int playerNumber;
        public Color color;

        public PlayerJoinPayload(int playerNumber, Color color)
        {
            this.playerNumber = playerNumber;
            this.color = color;
        }
    }
}
