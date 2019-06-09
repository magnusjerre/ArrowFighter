using Jerre.GameSettings;

namespace Jerre.GameMode.FreeForAll
{
    public class FreeForAllGameSettings : GameModeSettingsBase
    {
        private const string MAX_SCORE = "MaxScore";
        private const string PLAY_TIME = "PlayTime";
        
        public FreeForAllGameSettings()
        {
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(MAX_SCORE, "Winning score", 5, 5, 50, 5));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(PLAY_TIME, "Max play time", 90, 30, 300, 30));
        }

        public int MaxScore
        {
            get
            {
                return IntSettingByName(MAX_SCORE);
            }
        }

        public int PlayTime
        {
            get
            {
                return IntSettingByName(PLAY_TIME);
            }
        }
    }
}
