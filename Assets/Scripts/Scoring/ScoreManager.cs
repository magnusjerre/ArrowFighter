﻿using Jerre.Events;
using Jerre.GameMode.FreeForAll;
using Jerre.GameSettings;
using Jerre.UIStuff;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class ScoreManager : MonoBehaviour, IAFEventListener
    {
        public int maxScore = 10;
        public CountDownTimer countDownTimerPrefab;
        public RectTransform TopBar;

        private Dictionary<int, int> playerScores;
        private CountDownTimer countDownTimerInstance;
        private string GameTimerName = "GameTimer";

        private FreeForAllGameSettings freeForAllSettings;


        void Awake()
        {
            playerScores = new Dictionary<int, int>();
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
            if (playerScores.ContainsKey(payload.playerNumberOfKiller))
            {
                var oldScore = playerScores[payload.playerNumberOfKiller];
                var newScore = oldScore + 1;
                playerScores[payload.playerNumberOfKiller] = newScore;
                AFEventManager.INSTANCE.PostEvent(AFEvents.Score(payload.playerNumberOfKiller, newScore, maxScore));

                if (newScore == maxScore)
                {
                    countDownTimerInstance.StopTimer();
                    HandleGameOver();
                }
            }
            else
            {
                playerScores.Remove(payload.playerNumberOfKilledPlayer);
            }

            return false;
        }

        private void HandleGameOver()
        {
            Debug.Log("Game over");
            var players = GameObject.FindObjectsOfType<PlayerSettings>();
            var scores = new List<PlayerScore>();
            for (var i = 0; i < players.Length; i++)
            {
                var p = players[i];
                scores.Add(new PlayerScore(p.playerNumber, p.color, 0, playerScores[p.playerNumber]));
            }
            scores.Sort();
            var winningPlayer = scores[0];
            PlayersState.INSTANCE.SetScores(scores);
            AFEventManager.INSTANCE.PostEvent(AFEvents.GameOver(winningPlayer.PlayerNumber, playerScores[winningPlayer.PlayerNumber], winningPlayer.PlayerColor));
        }

        private bool HandlePlayersAllCreatedEvent(PlayersAllCreatedPayload payload)
        {
            for (var i = 0; i < payload.AllPlayers.Count; i++)
            {
                playerScores.Add(payload.AllPlayers[i].playerNumber, 0);
            }
            return false;
        }

        public Dictionary<int, int> GetPlayerScores()
        {
            return playerScores;
        }

        public int GetPlayerScore(int playerNumber)
        {
            return playerScores[playerNumber];
        }
    }
}
