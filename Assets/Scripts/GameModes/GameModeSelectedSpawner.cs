using Jerre.GameMode.Undead;
using Jerre.UI;
using Jerre.UIStuff;
using UnityEngine;

namespace Jerre.GameMode
{
    public class GameModeSelectedSpawner : MonoBehaviour
    {
        public ScoreUIElement scoreUIElementPrefab;
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
                        scoreUIManager.scoreUIElementPrefab = scoreUIElementPrefab;
                        break;
                    }
                case GameModes.FREE_FOR_ALL:
                    {
                        var scoreMode = gameObject.AddComponent<ScoreManager>();
                        scoreMode.countDownTimerPrefab = countDownTimerPrefab;
                        scoreMode.TopBar = TopBar;

                        var scoreUIManager = gameObject.AddComponent<ScoreUIManager>();
                        scoreUIManager.scoreUIElementPrefab = scoreUIElementPrefab;
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
