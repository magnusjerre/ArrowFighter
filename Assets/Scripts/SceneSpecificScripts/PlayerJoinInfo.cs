using UnityEngine;

namespace Jerre
{
    public struct PlayerJoinInfo
    {
        public int Number;
        public Color Color;

        public PlayerJoinInfo(int number, Color color)
        {
            Number = number;
            Color = color;
        }
    }
}
