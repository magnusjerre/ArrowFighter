using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jerre.MainMenu.GameMode
{
    public class GameModeSelectionManager : MonoBehaviour
    {
        void Start()
        {

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
