using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jerre.MainMenu.GameSettings
{
    public class GameSettingsManager : MonoBehaviour
    {
        public ValueChangerV2 KeyValueSettingsPrefab;
        public RectTransform GameModeSpecificUIRectangle;

        private void Start()
        {
            var gameModeSpecificSettings = PlayersState.INSTANCE.gameSettings.GameModeSettings;
            var offsetCounter = 0;
            for (var i = 0; i < gameModeSpecificSettings.settings.Count; i++)
            {
                var settingsInput = Instantiate(KeyValueSettingsPrefab, GameModeSpecificUIRectangle);
                settingsInput.backingSetting = gameModeSpecificSettings.settings[i];
                var topPosition = -offsetCounter * settingsInput.GetHeight();
                settingsInput.SetHeightPosition(0, topPosition);
                offsetCounter++;
            }
        }

        void Update()
        {
            if (Input.GetButtonDown(PlayerInputTags.ACCEPT + "1"))
            {
                SceneManager.LoadScene(SceneNames.START_MENU);
            } else if (Input.GetButtonDown(PlayerInputTags.FIRE2 + "1"))
            {
                SceneManager.LoadScene(SceneNames.GAME_MODE_SELECTION);
            }
        }
    }
}
