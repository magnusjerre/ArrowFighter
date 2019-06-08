using Jerre.GameSettings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Jerre.MainMenu.GameSettings
{
    public class GameSettingsManager : MonoBehaviour
    {
        public ValueChanger KeyValueSettingsPrefab;
        public RectTransform GameModeSpecificUIRectangle;
        public bool HandleBasicGameSettings;
        public EventSystem EventSystemForSelectingFirstFocusedElement;

        private void Start()
        {
            var gameModeSpecificSettings = HandleBasicGameSettings ? GameSettingsState.INSTANCE.BasicGameSettings : GameSettingsState.INSTANCE.GameModeSettings;
            var offsetCounter = 0;
            for (var i = 0; i < gameModeSpecificSettings.settings.Count; i++)
            {
                var settingsInput = Instantiate(KeyValueSettingsPrefab, GameModeSpecificUIRectangle);
                settingsInput.backingSetting = gameModeSpecificSettings.settings[i];

                if (HandleBasicGameSettings && i == 0 && EventSystemForSelectingFirstFocusedElement != null)
                {
                    EventSystemForSelectingFirstFocusedElement.firstSelectedGameObject = settingsInput.gameObject;
                }

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
