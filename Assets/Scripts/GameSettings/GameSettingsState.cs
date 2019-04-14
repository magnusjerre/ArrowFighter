using Jerre.Utils;

namespace Jerre.GameSettings
{
    public class GameSettingsState
    {
        public int baseSpeed;
        public int baseHealth;
        public int baseFireRate;
        public int baseFireDamage;

        public int pointsToWin;

        public void SetValue(GameSettingsField field, int value)
        {
            switch(field)
            {
                case GameSettingsField.BASE_SPEED:
                    {
                        baseSpeed = value;
                        break;
                    }
                case GameSettingsField.BASE_HEALTH:
                    {
                        baseHealth = value;
                        break;
                    }
                case GameSettingsField.BASE_FIRE_RATE:
                    {
                        baseFireRate = value;
                        break;
                    }
                case GameSettingsField.BASE_FIRE_STRENGTH:
                    {
                        baseFireDamage = value;
                        break;
                    }
                case GameSettingsField.POINTS_TO_WIN:
                    {
                        pointsToWin = value;
                        break;
                    }
            }
        }
    }
}