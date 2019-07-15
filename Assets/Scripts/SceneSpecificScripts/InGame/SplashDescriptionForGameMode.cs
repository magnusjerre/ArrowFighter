using Jerre.GameMode;
using Jerre.GameMode.FreeForAll;
using Jerre.GameSettings;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.Scoring
{
    public class SplashDescriptionForGameMode : MonoBehaviour
    {
        public Text Description;
        public Text Title;

        void Awake()
        {
            Description.text = null;
        }
        
        void Start()
        {
            var selectedGameMode = PlayersState.INSTANCE.selectedGameMode;
            if (selectedGameMode == GameModes.FREE_FOR_ALL)
            {
                var gameSettings = (FreeForAllGameSettings)GameSettingsState.INSTANCE.GameModeSettings;
                Title.text = "SCORE TO WIN";
                Description.text = gameSettings.MaxScore.ToString();
            }
            else if (selectedGameMode == GameModes.UNDEAD)
            {
                Title.text = "SURVIVE THE ROUND";
                Description.text = "DON'T DIE AND BECOME UNDEAD";
            }
        }
    }
}
