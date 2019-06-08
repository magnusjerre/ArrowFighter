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
            RoundState = new GameRoundState();
            BasicGameSettings = new BasicGameSettings();
        }

        public GameRoundState RoundState;
        public BasicGameSettings BasicGameSettings;
        public GameModeSettingsBase GameModeSettings;

    }
}