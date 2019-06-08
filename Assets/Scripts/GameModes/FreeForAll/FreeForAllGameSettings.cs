using Jerre.GameSettings;

namespace Jerre.GameMode.FreeForAll
{
    public class FreeForAllGameSettings : GameModeSettingsBase
    {
        private const string MAX_SCORE = "MaxScore";
        
        public FreeForAllGameSettings()
        {
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(MAX_SCORE, "Winning score", 5, 5, 50, 5));
        }

        public int MaxScore
        {
            get
            {
                return IntSettingByName(MAX_SCORE);
            }
        }
    }
}
