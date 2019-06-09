using Jerre.GameMode.FreeForAll;
using Jerre.GameSettings;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.Scoring
{
    public class TotalScoreInitializer : MonoBehaviour
    {
        public Text Score;
        public Text Title;

        void Awake()
        {
            Score.text = null;
        }
        
        void Start()
        {
            var selectedGameMode = PlayersState.INSTANCE.selectedGameMode;
            if (selectedGameMode == GameModes.FREE_FOR_ALL)
            {
                var gameSettings = (FreeForAllGameSettings)GameSettingsState.INSTANCE.GameModeSettings;
                Title.text = "SCORE TO WIN";
                Score.text = gameSettings.MaxScore.ToString();
            }
            else if (selectedGameMode == GameModes.UNDEAD)
            {
                Title.text = "SURVIVE THE ROUND";
                Score.text = "DON'T DIE AND BECOME UNDEAD";
            }
        }
    }
}
