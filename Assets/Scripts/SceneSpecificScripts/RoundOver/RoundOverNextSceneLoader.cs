using UnityEngine;
using UnityEngine.SceneManagement;
using Jerre.GameSettings;

namespace Jerre
{
    public class RoundOverNextSceneLoader : MonoBehaviour
    {
        public float SceneLoadDelay = 3f;

        void Start()
        {
            Invoke("LoadGameSceneAgain", 3f);
        }

        void LoadGameSceneAgain()
        {
            SceneManager.LoadScene(SceneNames.GAME_SCENE, LoadSceneMode.Single);
        }
    }
}