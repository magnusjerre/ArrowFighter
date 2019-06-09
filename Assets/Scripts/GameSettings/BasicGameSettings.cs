namespace Jerre.GameSettings
{
    public class BasicGameSettings : GameModeSettingsBase
    {
        private static string SPEED = "Speed";
        private static string MAX_ACCELERATION = "MaxAcceleration";
        private static string BOOST_SPEED = "BoostSpeed";
        private static string BOOST_DURATION = "BoostDuration";
        private static string BOOST_PAUSE = "BoosPause";
        private static string HEALTH = "Health";
        private static string PLAY_TIME = "PlayTime";
        
        public BasicGameSettings()
        {
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(PLAY_TIME, "Max play time", 90, 30, 300, 30));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(HEALTH, "Health", 100, 10, 300, 10));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(SPEED, "Speed", 200, 50, 400, 25));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(MAX_ACCELERATION, "Max acceleration", 500, 100, 1500, 100));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOOST_SPEED, "Boost speed", 500, 250, 1000, 50));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOOST_DURATION, "Boost duration in seconds", 2, 1, 5, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOOST_PAUSE, "Time between boosts", 2, 0, 10, 1));
        }

        public int Speed
        {
            get
            {
                return IntSettingByName(SPEED);
            }
        }

        public int MaxAcceleration
        {
            get
            {
                return IntSettingByName(MAX_ACCELERATION);
            }
        }

        public int BoostSpeed
        {
            get
            {
                return IntSettingByName(BOOST_SPEED);
            }
        }

        public int BoostDuration
        {
            get
            {
                return IntSettingByName(BOOST_DURATION);

            }
        }

        public int BoostPause
        {
            get
            {
                return IntSettingByName(BOOST_PAUSE);
            }
        }

        public int Health
        {
            get
            {
                return IntSettingByName(HEALTH);
            }
        }

        public int PlayTime
        {
            get
            {
                return IntSettingByName(PLAY_TIME);
            }
        }

        public override string GetSettingsName()
        {
            return "BASIC GAME SETTINGS";
        }
    }
}
