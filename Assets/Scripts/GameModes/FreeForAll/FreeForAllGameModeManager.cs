using Jerre.Events;
using Jerre.GameMode.FreeForAll;
using Jerre.GameSettings;
using UnityEngine;

namespace Jerre
{
    public class FreeForAllGameModeManager : MonoBehaviour, IAFEventListener
    {
        public int maxScore = 10;
        public CountDownTimer countDownTimerPrefab;
        public RectTransform TopBar;

        private CountDownTimer countDownTimerInstance;
        private string GameTimerName = "GameTimer";

        private FreeForAllGameSettings freeForAllSettings;
        private SingleRoundScores<IScore> currentRoundScores;

        void Awake()
        {
            // Setting up scoring resources
            currentRoundScores = PlayersState.INSTANCE.gameScores.StartNewRound();
            PlayersState.INSTANCE.gameScoresAccumulator = (acc, current) => SimpleScore.Accumulate((SimpleScore)acc, (SimpleScore)current);

            AFEventManager.INSTANCE.AddListener(this);
            freeForAllSettings = (FreeForAllGameSettings)GameSettingsState.INSTANCE.GameModeSettings;
            Debug.Log("pointsToWin:" + freeForAllSettings.MaxScore);
            maxScore = freeForAllSettings.MaxScore;
        }

        void Start()
        {
            countDownTimerInstance = Instantiate(countDownTimerPrefab, TopBar);
            countDownTimerInstance.TimerName = GameTimerName;
            countDownTimerInstance.TimeInSeconds = GameSettingsState.INSTANCE.BasicGameSettings.PlayTime;
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch (afEvent.type)
            {
                case AFEventType.KILLED:
                    {
                        return HandleKilledEvent((KilledEventPayload)afEvent.payload);
                    }
                case AFEventType.PLAYERS_ALL_CREATED:
                    {
                        return HandlePlayersAllCreatedEvent((PlayersAllCreatedPayload)afEvent.payload);
                    }
                case AFEventType.GAME_START:
                    {
                        countDownTimerInstance.StartTimer();
                        return false;
                    }
                case AFEventType.COUNT_DOWN_FINISHED:
                    {
                        var payload = (CountDownFinishedPayload)afEvent.payload;
                        if (payload.TimerName.Equals(GameTimerName))
                        {
                            HandleGameOver();
                        }
                        return false;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        private bool HandleKilledEvent(KilledEventPayload payload)
        {
            if (payload.playerNumberOfKiller == payload.playerNumberOfKilledPlayer)
            {
                // Player killed itself, no score event generated for this...
                return false;
            }

            var playerScore = currentRoundScores.GetScoreForPlayer<SimpleScore>(payload.playerNumberOfKiller);
            var newScore = playerScore.IncreaseScoreBy(1);
            
            AFEventManager.INSTANCE.PostEvent(AFEvents.Score(payload.playerNumberOfKiller, newScore, maxScore));
            if (newScore == maxScore)
            {
                countDownTimerInstance.StopTimer();
                HandleGameOver();
            }

            //if (playerScores.ContainsKey(payload.playerNumberOfKiller))
            //{
            //    var oldScore = playerScores[payload.playerNumberOfKiller];
            //    var newScore = oldScore + 1;
            //    playerScores[payload.playerNumberOfKiller] = newScore;
            //    AFEventManager.INSTANCE.PostEvent(AFEvents.Score(payload.playerNumberOfKiller, newScore, maxScore));

            //    if (newScore == maxScore)
            //    {
            //        countDownTimerInstance.StopTimer();
            //        HandleGameOver();
            //    }
            //}
            //else
            //{
            //    playerScores.Remove(payload.playerNumberOfKilledPlayer);
            //}

            return false;
        }

        private void HandleGameOver()
        {
            Debug.Log("Game over");

            var winningPlayer = currentRoundScores.SortedByDescendingScores()[0];
            AFEventManager.INSTANCE.PostEvent(AFEvents.GameOver(winningPlayer.PlayerNumber(), winningPlayer.Score(), winningPlayer.PlayerColor()));
        }

        private bool HandlePlayersAllCreatedEvent(PlayersAllCreatedPayload payload)
        {
            foreach (var player in payload.AllPlayers)
            {
                currentRoundScores.AddScoreForPlayer(new SimpleScore(player.color, player.playerNumber, 0));
            }
            return false;
        }
    }
}
