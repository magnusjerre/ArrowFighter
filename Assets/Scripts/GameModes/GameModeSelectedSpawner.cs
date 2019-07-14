using Jerre.GameMode.Undead;
using Jerre.UIStuff;
using Jerre.UI.InGame;
using UnityEngine;

namespace Jerre.GameMode
{
    public class GameModeSelectedSpawner : MonoBehaviour
    {
        public CountDownTimer countDownTimerPrefab;
        public RectTransform TopBar;

        private void Awake()
        {
            var selectedGameMode = PlayersState.INSTANCE.selectedGameMode;
            Debug.Log("selectedGameMode" + selectedGameMode);
            switch (selectedGameMode)
            {
                case GameModes.UNDEAD:
                    {
                        var undeadMode = gameObject.AddComponent<UndeadGameMode>();
                        undeadMode.countDownTimerPrefab = countDownTimerPrefab;
                        undeadMode.TopBar = TopBar;

                        var scoreUIManager = gameObject.AddComponent<ScoreUIManager>();
                        break;
                    }
                case GameModes.FREE_FOR_ALL:
                    {
                        var scoreMode = gameObject.AddComponent<FreeForAllGameModeManager>();
                        scoreMode.countDownTimerPrefab = countDownTimerPrefab;
                        scoreMode.TopBar = TopBar;

                        var scoreUIManager = gameObject.AddComponent<ScoreUIManager>();
                        break;
                    }
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
