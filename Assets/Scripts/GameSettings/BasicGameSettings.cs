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
        private static string FIRE_RATE = "FireRate";
        private static string FIRE_DAMAGE = "FireDamage";
        private static string BOMB_DAMAGE = "BombDamage";
        private static string BOMB_MAX_LIFETIME = "BomMaxLifetime";
        private static string BOMB_PAUSE_TIME = "BombPauseTime";
        private static string BOMB_EXPLOSION_RADIUS = "BombExplosionRadius";
        private static string BOMB_EXPLOSION_ACCELERATION = "BombExplosionAcceleration";
        
        public BasicGameSettings()
        {
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(SPEED, "Speed", 200, 50, 400, 25));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(MAX_ACCELERATION, "Max acceleration", 500, 100, 1500, 100));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOOST_SPEED, "Boost speed", 500, 250, 1000, 50));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOOST_DURATION, "Boost duration in seconds", 2, 1, 5, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOOST_PAUSE, "Time between boosts", 2, 0, 10, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(HEALTH, "Health", 100, 10, 300, 10));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(FIRE_RATE, "Main weapon fire rate per second", 4, 1, 10, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(FIRE_DAMAGE, "Main weapon damage", 10, 5, 30, 5));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_DAMAGE, "Bomb damage", 50, 10, 100, 10));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_MAX_LIFETIME, "Bomb max lifetime", 10, 0, 30, 5));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_PAUSE_TIME, "Time between bomb drops", 2, 0, 10, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_EXPLOSION_RADIUS, "Bomb explosion radius", 70, 40, 100, 15));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_EXPLOSION_ACCELERATION, "Bomb explosion acceleration", 10000, 5000, 30000, 5000));
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

        public int FireRate
        {
            get
            {
                return IntSettingByName(FIRE_RATE);
            }
        }

        public int FireDamage
        {
            get
            {
                return IntSettingByName(FIRE_DAMAGE);
            }
        }

        public int BombDamage
        {
            get
            {
                return IntSettingByName(BOMB_DAMAGE);
            }
        }

        public int BombMaxLifetime
        {
            get
            {
                return IntSettingByName(BOMB_MAX_LIFETIME);
            }
        }

        public int BombPauseTime
        {
            get
            {
                return IntSettingByName(BOMB_PAUSE_TIME);
            }
        }

        public int BombExplosionRadius
        {
            get
            {
                return IntSettingByName(BOMB_EXPLOSION_RADIUS);
            }
        }

        public int BombExplosionAcceleration
        {
            get
            {
                return IntSettingByName(BOMB_EXPLOSION_ACCELERATION);
            }
        }
    }
}
