using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jerre.MainMenu.GameMode
{
    public class GameModeSelectionManager : MonoBehaviour
    {
        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetButton(PlayerInputTags.DODGE_RIGHT + 1)
                && Input.GetButton(PlayerInputTags.DODGE_LEFT + 1)
                && Input.GetButton(PlayerInputTags.ACCEPT + 1))
            {
                SceneManager.LoadScene(SceneNames.BYE_BYE_SCENE);
            }
        }

        public void SetFreeForAll()
        {
            PlayersState.INSTANCE.selectedGameMode = GameModes.FREE_FOR_ALL;
            SceneManager.LoadScene(SceneNames.GAME_SETTINGS_SCENE);
        }

        public void SetCheckpointRace()
        {
            PlayersState.INSTANCE.selectedGameMode = GameModes.CHECKPOINT_RACE;
            SceneManager.LoadScene(SceneNames.GAME_SETTINGS_SCENE);
        }
    }
}
