using Jerre.GameSettings;

namespace Jerre.GameMode.Undead
{
    public class UndeadGameSettings: GameModeSettingsBase
    {
        private const string GAME_PLAYER_TIME_IN_SECONDS = "GamePlayerTimeInSeconds";
        private const string NUMBER_OF_GAME_ROUNDS = "GameRounds";
        private const string STARTING_UNDEAD = "StartingUndead";
        private const string UNDEAD_RESPAWN_TIME_IN_SECONDS = "UndeadRespawnTimeInSeconds";
        private const string ALIVE_KILL_POINTS = "AliveKillPoints";
        private const string UNDEAD_KILL_POINTS = "UndeadKillPoints";

        public UndeadGameSettings()
        {
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(GAME_PLAYER_TIME_IN_SECONDS, "Round time (seconds)", 30, 10, 60, 10));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(NUMBER_OF_GAME_ROUNDS, "Number of rounds", 2, 1, 10, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(STARTING_UNDEAD, "Number of players starting as undead",  1, 1, 7, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(UNDEAD_RESPAWN_TIME_IN_SECONDS, "Respawn time", 1, 0, 5, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(ALIVE_KILL_POINTS, "Points for killing when alive", 1, 1, 5, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(UNDEAD_KILL_POINTS, "Points for killing when undead", 2, 1, 5, 1));
        }

        public int GamePlayerTimeInSeconds
        {
            get
            {
                return IntSettingByName(GAME_PLAYER_TIME_IN_SECONDS);
            }
        }

        public int NumberOfGameRounds
        {
            get
            {
                return IntSettingByName(NUMBER_OF_GAME_ROUNDS);
            }
        }

        public int StartingUndead
        {
            get
            {
                return IntSettingByName(STARTING_UNDEAD);
            }
        }

        public int UndeadRespawnTimeInSeconds
        {
            get
            {
                return IntSettingByName(UNDEAD_RESPAWN_TIME_IN_SECONDS);
            }
        }

        public int AliveKillPoints
        {
            get
            {
                return IntSettingByName(ALIVE_KILL_POINTS);
            }
        }

        public int UndeadKillPoints
        {
            get
            {
                return IntSettingByName(UNDEAD_KILL_POINTS);
            }
        }
    }
}
