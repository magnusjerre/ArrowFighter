using Jerre.GameMode.Undead;
using UnityEngine;

namespace Jerre.GameMode
{
    public class GameModeInstantiator : MonoBehaviour
    {
        public CountDownTimer countDownTimerPrefab;
        public RectTransform TopBar;
        public RectTransform MainUi;
        public GameObject RoundLeaderScorePrefab;
        public GameObject LeaderAndMaxScorePrefab;

        private void Awake()
        {
            var selectedGameMode = PlayersState.INSTANCE.selectedGameMode;
            switch (selectedGameMode)
            {
                case GameModes.UNDEAD:
                    {
                        var undeadMode = gameObject.AddComponent<UndeadGameMode>();
                        undeadMode.countDownTimerPrefab = countDownTimerPrefab;
                        undeadMode.TopBar = TopBar;
                        Instantiate(RoundLeaderScorePrefab, MainUi);
                        break;
                    }
                case GameModes.FREE_FOR_ALL:
                    {
                        var scoreMode = gameObject.AddComponent<FreeForAllGameModeManager>();
                        scoreMode.countDownTimerPrefab = countDownTimerPrefab;
                        scoreMode.TopBar = TopBar;
                        Instantiate(LeaderAndMaxScorePrefab, MainUi);
                        break;
                    }
            }
        }
    }
}
