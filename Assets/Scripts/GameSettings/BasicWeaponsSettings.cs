namespace Jerre.GameSettings
{
    public class BasicWeaponsSettings : GameModeSettingsBase
    {
        private static string FIRE_RATE = "FireRate";
        private static string FIRE_DAMAGE = "FireDamage";
        private static string BOMB_DAMAGE = "BombDamage";
        private static string BOMB_MAX_LIFETIME = "BomMaxLifetime";
        private static string BOMB_PAUSE_TIME = "BombPauseTime";
        private static string BOMB_EXPLOSION_RADIUS = "BombExplosionRadius";
        private static string BOMB_EXPLOSION_ACCELERATION = "BombExplosionAcceleration";

        public BasicWeaponsSettings()
        {
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(FIRE_RATE, "Main weapon fire rate per second", 4, 1, 10, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(FIRE_DAMAGE, "Main weapon damage", 10, 5, 30, 5));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_DAMAGE, "Bomb damage", 50, 10, 100, 10));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_MAX_LIFETIME, "Bomb max lifetime", 10, 0, 30, 5));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_PAUSE_TIME, "Time between bomb drops", 2, 0, 10, 1));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_EXPLOSION_RADIUS, "Bomb explosion radius", 70, 40, 100, 15));
            settings.Add(KeyValueWithOptionsSetting.NumberedValue(BOMB_EXPLOSION_ACCELERATION, "Bomb explosion acceleration", 10000, 5000, 30000, 5000));
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

        public override string GetSettingsName()
        {
            return "BASIC WEAPONS SETTINGS";
        }
    }
}
