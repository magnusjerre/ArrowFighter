using UnityEngine;

namespace Jerre.Events
{
    public struct PauseMenuEnablePayload
    {
        public int PlayerNumber;
        public Color PlayerColor;

        public PauseMenuEnablePayload(int playerNumber, Color playerColor)
        {
            PlayerNumber = playerNumber;
            PlayerColor = playerColor;
        }
    }
}
