using Jerre.MainMenu;
using System.Collections.Generic;

namespace Jerre
{
    public class PlayersState
    {
        private static PlayersState instance;
        public static PlayersState INSTANCE
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayersState();
                }
                return instance;
            }
        }

        private List<PlayerMenuSettings> ReadyPlayers;

        private PlayersState()
        {
            ReadyPlayers = new List<PlayerMenuSettings>();
        }

        public bool AddPlayer(PlayerMenuSettings player)
        {
            if (!ReadyPlayers.Contains(player))
            {
                ReadyPlayers.Add(player);
                return true;
            }
            return false;
        }

        public bool RemovePlayer(PlayerMenuSettings player)
        {
            return ReadyPlayers.Remove(player);
        }

        public int ReadyPlayersCount => ReadyPlayers.Count;
        public PlayerMenuSettings GetSettings(int index) => ReadyPlayers[index];

        public void Reset()
        {
            ReadyPlayers.Clear();
        }
    }
}
