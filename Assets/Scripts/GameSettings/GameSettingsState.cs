using Jerre.Utils;

namespace Jerre.GameSettings
{
    public class GameSettingsState
    {
        public int baseSpeed = 150;
        public int baseHealth = 100;
        public int baseFireRate = 4;
        public int baseFireDamage = 10;

        public int pointsToWin = 5;

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