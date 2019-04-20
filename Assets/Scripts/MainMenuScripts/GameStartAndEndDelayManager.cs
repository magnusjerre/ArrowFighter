using Jerre.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jerre.Managers
{
    public class GameStartAndEndDelayManager : MonoBehaviour, IAFEventListener
    {
        PlayerComponentsManager playerCompManager;
        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
            playerCompManager = GameObject.FindObjectOfType<PlayerComponentsManager>();
        }

        void Start()
        {
            //TODO: Add this manager to the game scene
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type) {
                case AFEventType.PLAYER_MENU_BAR_UI_CREATED: {
                    playerCompManager.EnableOrDisableAllPlayersInputResponses(false);
                    Debug.Log("Delaying player input start");
                    Invoke("ReEnableAllPlayersInputResponses", PlayersState.INSTANCE.WaitTimeForPlayersToStart);
                    break;
                }
                case AFEventType.GAME_OVER: {
                    playerCompManager.EnableOrDisableAllPlayersInputResponses(false);
                    //TODO: Move the score to the center of the screen
                    Invoke("LoadGameOverScene", PlayersState.INSTANCE.WaitTimeToDisplayGameOver);
                    break;
                }
            }
            return false;
        }

        void ReEnableAllPlayersInputResponses()
        {
            playerCompManager.EnableOrDisableAllPlayersInputResponses(true);
            Debug.Log("Player input is now processed");
        }

        void LoadGameOverScene()
        {
            AFEventManager.INSTANCE.RemoveAllListeners();
            SceneManager.LoadScene(SceneNames.GAME_OVER_SCENE, LoadSceneMode.Single);
        }
    }
}