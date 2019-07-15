using Jerre.GameSettings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Jerre
{
    public class GameSettingsManager : MonoBehaviour
    {
        public ValueChanger KeyValueSettingsPrefab;
        public RectTransform GameModeSpecificUIRectangle;
        public bool HandleBasicGameSettings;
        public EventSystem EventSystemForSelectingFirstFocusedElement;
        public Text SettingsName;

        private string currentSettingsName = null;

        void Start()
        {
            currentSettingsName = GameSettingsState.INSTANCE.BasicGameSettings.GetSettingsName();
            SetSettings(GameSettingsState.INSTANCE.BasicGameSettings);
        }

        void Update()
        {
            if (Input.GetButtonDown(PlayerInputTags.ACCEPT + "1"))
            {
                SceneManager.LoadScene(SceneNames.START_MENU);
            } else if (Input.GetButtonDown(PlayerInputTags.FIRE2 + "1"))
            {
                SceneManager.LoadScene(SceneNames.GAME_MODE_SELECTION);
            } else if (Input.GetButtonDown(PlayerInputTags.CYCLE_SETTINGS + "1"))
            {
                CycleSettings();
            }
        }

        void CycleSettings()
        {
            if (currentSettingsName.Equals(GameSettingsState.INSTANCE.BasicGameSettings.GetSettingsName()))
            {
                ClearOldSettingsFromView();
                SetSettings(GameSettingsState.INSTANCE.BasicWeaponsSettings);
            } else if (currentSettingsName.Equals(GameSettingsState.INSTANCE.BasicWeaponsSettings.GetSettingsName()))
            {
                ClearOldSettingsFromView();
                if (GameSettingsState.INSTANCE.GameModeSettings != null)
                {
                    SetSettings(GameSettingsState.INSTANCE.GameModeSettings);
                }
                else
                {
                    SetSettings(GameSettingsState.INSTANCE.BasicGameSettings);
                }
            } else
            {
                ClearOldSettingsFromView();
                SetSettings(GameSettingsState.INSTANCE.BasicGameSettings);
            }
        }

        void SetSettings(GameModeSettingsBase gameModeSpecificSettings)
        {
            currentSettingsName = gameModeSpecificSettings.GetSettingsName();
            SettingsName.text = gameModeSpecificSettings.GetSettingsName();

            var offsetCounter = 0;
            for (var i = 0; i < gameModeSpecificSettings.settings.Count; i++)
            {
                var settingsInput = Instantiate(KeyValueSettingsPrefab, GameModeSpecificUIRectangle);
                settingsInput.backingSetting = gameModeSpecificSettings.settings[i];
                settingsInput.gameObject.name = settingsInput.backingSetting.Name;

                if (HandleBasicGameSettings && i == 0 && EventSystemForSelectingFirstFocusedElement != null)
                {
                    settingsInput.Select();
                }

                var topPosition = -offsetCounter * settingsInput.GetHeight();
                settingsInput.SetHeightPosition(0, topPosition);
                offsetCounter++;
            }
        }

        void ClearOldSettingsFromView()
        {
            for (var i = 0; i < GameModeSpecificUIRectangle.childCount; i++)
            {
                Destroy(GameModeSpecificUIRectangle.GetChild(i).gameObject);
            }
        }
    }
}
