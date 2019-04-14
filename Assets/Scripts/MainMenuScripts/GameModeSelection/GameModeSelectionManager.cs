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
            Debug.Log("SetFreeForAll");
            SceneManager.LoadScene(SceneNames.START_MENU);
        }

        public void SetCheckpointRace()
        {
            PlayersState.INSTANCE.selectedGameMode = GameModes.CHECKPOINT_RACE;
            Debug.Log("SetCheckpointRace");
            SceneManager.LoadScene(SceneNames.START_MENU);
        }
    }
}
