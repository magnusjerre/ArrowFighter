namespace Jerre.GameSettings
{
    public class GameSettingsState
    {
        private static GameSettingsState instance;
        public static GameSettingsState INSTANCE
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameSettingsState();
                }
                return instance;
            }
        }

        private GameSettingsState()
        {
            BasicGameSettings = new BasicGameSettings();
            BasicWeaponsSettings = new BasicWeaponsSettings();
        }

        public BasicGameSettings BasicGameSettings;
        public BasicWeaponsSettings BasicWeaponsSettings;
        public GameModeSettingsBase GameModeSettings;
    }
}