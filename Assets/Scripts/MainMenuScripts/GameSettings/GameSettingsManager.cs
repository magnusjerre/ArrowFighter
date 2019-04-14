using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jerre.MainMenu.GameSettings
{
    public class GameSettingsManager : MonoBehaviour
    {
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
