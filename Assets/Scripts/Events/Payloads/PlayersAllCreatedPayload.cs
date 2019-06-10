using System.Collections.Generic;

namespace Jerre.Events
{
    public struct PlayersAllCreatedPayload
    {
        public List<PlayerSettings> AllPlayers;

        public PlayersAllCreatedPayload(List<PlayerSettings> allPlayers)
        {
            AllPlayers = allPlayers;
        }
    }
}
