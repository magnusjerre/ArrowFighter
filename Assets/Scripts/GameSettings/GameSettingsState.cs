
using Jerre.Utils;
using UnityEngine;

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

        //public int speed = 150;
        //public int maxAcceleration = 250;
        //public int boostSpeed = 300;
        //public int boostDuration = 2;
        //public int boostPause = 2;
        //public int health = 100;
        //public int fireRate = 4;
        //public int fireDamage = 10;
        //public int bombDamage = 50;
        //public int bombMaxLifeTime = 7;
        //public int bombPauseTime = 2;
        //public int bombExplosionRadius = 75;
        //public int pointsToWin = 5;
        //public int bombExplosionAcceleration = 200_000;

        public void SetValue(GameSettingsField field, int value)
        {
            //switch(field)
            //{
            //    case GameSettingsField.SPEED:
            //        {
            //            speed = value;
            //            break;
            //        }
            //    case GameSettingsField.MAX_ACCELERATION:
            //        {
            //            maxAcceleration = value;
            //            break;
            //        }
            //    case GameSettingsField.BOOST_SPEED: {
            //        boostSpeed = value;
            //        break;
            //    }
            //    case GameSettingsField.BOOST_DURATION: {
            //        boostDuration = value;
            //        break;
            //    }
            //    case GameSettingsField.BOOST_PAUSE: {
            //        boostPause = value;
            //        break;
            //    }
            //    case GameSettingsField.HEALTH:
            //        {
            //            health = value;
            //            break;
            //        }
            //    case GameSettingsField.FIRE_RATE:
            //        {
            //            fireRate = value;
            //            break;
            //        }
            //    case GameSettingsField.FIRE_DAMAGE:
            //        {
            //            fireDamage = value;
            //            break;
            //        }
            //    case GameSettingsField.BOMB_DAMAGE: {
            //        bombDamage = value;
            //        break;
            //    }
            //    case GameSettingsField.BOMB_MAX_LIFETIME: {
            //        bombMaxLifeTime = value;
            //        break;
            //    }
            //    case GameSettingsField.BOMB_PAUSE_TIME: {
            //        bombPauseTime = value;
            //        break;
            //    }
            //    case GameSettingsField.BOMB_EXPLOSION_RADIUS: {
            //        bombExplosionRadius = value;
            //        break;
            //    }
            //    case GameSettingsField.BOMB_EXPLOSION_ACCELERATION:
            //        {
            //            bombExplosionAcceleration = value;
            //            break;
            //        }
            //    case GameSettingsField.POINTS_TO_WIN:
            //        {
            //            pointsToWin = value;
            //            break;
            //        }
            //}
        }

        public void SetGameModeSpecificValue(string name, string value)
        {
            GameModeSettings.SetValue(name, value);
        }
    }
}