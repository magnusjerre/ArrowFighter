using UnityEngine;

namespace Jerre.Utils
{
    public class PlayerFetcher
    {
        public static PlayerSettings FindPlayerByPlayerNumber(int playerNumber)
        {
            var allPlayers = GameObject.FindObjectsOfType<PlayerSettings>();
            for (var i = 0; i < allPlayers.Length; i++)
            {
                if (allPlayers[i].playerNumber == playerNumber) {
                    return allPlayers[i];
                }
            }
            return null;
        }
    }
}
