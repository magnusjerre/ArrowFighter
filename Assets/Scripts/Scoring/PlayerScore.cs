using System;
using UnityEngine;

namespace Jerre
{
    public struct PlayerScore : IComparable
    {
        public int PlayerNumber;
        public Color32 PlayerColor;
        public int Position;
        public int Score;

        public PlayerScore(int playerNumber, Color32 playerColor, int position, int score)
        {
            PlayerNumber = playerNumber;
            PlayerColor = playerColor;
            Position = position;
            Score = score;
        }

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is PlayerScore)) return 0;

            return ((PlayerScore)obj).Score - Score;
        }
    }
}
