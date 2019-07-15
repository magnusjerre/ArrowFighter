using Jerre.GameMode;
using Jerre.GameMode.FreeForAll;
using Jerre.GameMode.Undead;
using Jerre.GameSettings;
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

        public List<PlayerJoinInfo> PlayerInfos;

        public List<PlayerScore> Scores
        {
            get;
            private set;
        }

        public GameModes selectedGameMode;
        public void SetSelectedGameMode(GameModes mode) {
            selectedGameMode = mode;
            switch(mode)
            {
                case GameModes.FREE_FOR_ALL:
                    {
                        gameSettings.GameModeSettings = new FreeForAllGameSettings();
                        break;

                    }
                case GameModes.UNDEAD:
                    {
                        gameSettings.GameModeSettings = new UndeadGameSettings();
                        break;
                    }
            }
        }

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
            PlayerInfos = new List<PlayerJoinInfo>();
        }

        public Color GetPlayerColorV2(int playerNumber)
        {
            foreach (var info in PlayerInfos)
            {
                if (info.Number == playerNumber)
                {
                    return info.Color;
                }
            }
            return Color.white;
        }

        public void Reset()
        {
            PlayerInfos.Clear();
            Scores = null;
            selectedGameMode = GameModes.UNSELECTED;
        }

    }
}
