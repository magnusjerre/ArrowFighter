using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jerre
{
    public class GameOverNextSceneLoader : MonoBehaviour
    {
        public float LoadDelay = 3f;

        void Start()
        {
            Invoke("LoadFirstMenuScene", LoadDelay);
        }

        void LoadFirstMenuScene()
        {
            PlayersState.INSTANCE.Reset();
            SceneManager.LoadScene(SceneNames.GAME_MODE_SELECTION, LoadSceneMode.Single);
        }
    }
}
