using Jerre.GameSettings;
using Jerre.MainMenu;
using System.Collections.Generic;
using UnityEngine;

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
                    instance.gameSettings = GameSettingsState.INSTANCE;
                }
                return instance;
            }
        }

        private List<PlayerMenuSettings> ReadyPlayers;
        public List<PlayerScore> Scores
        {
            get;
            private set;
        }

        public GameModes selectedGameMode;

        public void SetScores(List<PlayerScore> newScores)
        {
            newScores.Sort();
            Scores = new List<PlayerScore>();
            for (var i = 0; i < newScores.Count; i++)
            {
                var s = newScores[i];
                Scores.Add(new PlayerScore(s.PlayerNumber, s.PlayerColor, i + 1, s.Score));
            }
        }

        public GameSettingsState gameSettings;

        public float WaitTimeForPlayersToStart = 2f;
        public float WaitTimeToDisplayGameOver = 5f;

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
            Scores = null;
            selectedGameMode = GameModes.UNSELECTED;
        }

    }
}
