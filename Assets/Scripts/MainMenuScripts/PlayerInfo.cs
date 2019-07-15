using UnityEngine;

namespace Jerre
{
    public struct PlayerInfo
    {
        public int Number;
        public Color Color;

        public PlayerInfo(int number, Color color)
        {
            Number = number;
            Color = color;
        }
    }
}
