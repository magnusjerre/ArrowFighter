using System;
using System.Collections.Generic;

namespace Jerre.GameSettings
{
    public abstract class GameModeSettingsBase
    {
        public List<KeyValueWithOptionsSetting> settings = new List<KeyValueWithOptionsSetting>();

        protected int IntSettingByName(string name)
        {
            return Int32.Parse(settings.Find(kv => name.Equals(kv.Name)).Value);
        }

        public void SetValue(string name, string value)
        {
            var setting = settings.Find(kv => name.Equals(kv.Name));
            setting.Value = value;
        }

        public abstract string GetSettingsName();

    }
}
