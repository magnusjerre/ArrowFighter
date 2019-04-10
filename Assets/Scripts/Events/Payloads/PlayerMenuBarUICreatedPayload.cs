using UnityEngine;

namespace Jerre.Events
{
    public struct PlayerMenuBarUICreatedPayload
    {
        public int PlayerNumber;

        public PlayerMenuBarUICreatedPayload(int playerNumber)
        {
            PlayerNumber = playerNumber;
        }
    }
}
