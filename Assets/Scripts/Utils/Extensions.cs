using UnityEngine;
using System.Collections;

namespace Jerre
{
    public static class Extensions
    {
        public static Color GetColorForPlayerNumber(this PlayerSettings[] playerSettings, int playerNumber)
        {
            for (var i = 0; i < playerSettings.Length; i++)
            {
                if (playerSettings[i].playerNumber == playerNumber)
                {
                    return playerSettings[i].color;
                }
            }
            return Color.black;
        }
    }
}
