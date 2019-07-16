using Jerre.Events;
using Jerre.GameSettings;
using Jerre.Utils;
using Jerre.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.GameMode.Undead
{
    public class UndeadGameMode : MonoBehaviour, IAFEventListener
    {
        public int NumberOfGameRounds = 2;
        public int NumberOfStartingUndead = 1;
        public int UndeadRespawnTimeInSeconds = 1;
        public int AliveKillPoints = 1;
        public int UndeadKillPoints = 2;
        public Color UndeadColor = Color.gray;
        public CountDownTimer countDownTimerPrefab;
        public RectTransform TopBar;

        private string RoundTimerName = "RoundTimer";
        private CountDownTimer countDownTimerInstance;

        private UndeadGameSettings undeadSettings;

        private SingleRoundScores<IScore> currentRoundScores;

        // Called as part of gameObject.AddComponent<...>(). Can therefore not have initialization here that is added after addComponent, use Start() instead
        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
            currentRoundScores = PlayersState.INSTANCE.gameScores.StartNewRound();

            undeadSettings = (UndeadGameSettings)GameSettingsState.INSTANCE.GameModeSettings;
            NumberOfGameRounds = undeadSettings.NumberOfGameRounds;
            NumberOfStartingUndead = undeadSettings.StartingUndead;
            UndeadRespawnTimeInSeconds = undeadSettings.UndeadRespawnTimeInSeconds;
            AliveKillPoints = undeadSettings.AliveKillPoints;
            UndeadKillPoints = undeadSettings.UndeadKillPoints;
        }

        void Start()
        {
            countDownTimerInstance = Instantiate(countDownTimerPrefab, TopBar);
            countDownTimerInstance.TimerName = RoundTimerName;
            countDownTimerInstance.TimeInSeconds = GameSettingsState.INSTANCE.BasicGameSettings.PlayTime;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int[] SelectPlayersToStartAsUndead(List<PlayerSettings> playerSettings)
        {



            return new int[] { 1 };
        }

        void HandleGameStart(List<PlayerSettings> allPlayers)
        {
            var undeadPlayerNumbers = SelectPlayersToStartAsUndead(allPlayers);
            foreach (var ps in allPlayers)
            {
                var playerScore = new SingleRoundUndeadScore(ps.playerNumber, ps.color);
                currentRoundScores.AddScoreForPlayer(playerScore);
                MakeLiving(ps);
                for (var i = 0; i < undeadPlayerNumbers.Length; i++)
                {
                    if (ps.playerNumber == undeadPlayerNumbers[i])
                    {
                        playerScore.Undead = true;
                        playerScore.StartedAsUndead = true;
                        MakeUndead(ps);
                    }
                }
            }
        }

        void MakeUndead(PlayerSettings playerSettings)
        {
            var mainEngineParticles = playerSettings.GetComponent<PlayerMainEngineParticles>();
            mainEngineParticles.ChangeColor(UndeadColor);
            var weaponSlot = playerSettings.GetComponent<WeaponSlot>();
            weaponSlot.enabled = false;
        }

        void MakeLiving(PlayerSettings playerSettings)
        {
            var mainEngineParticles = playerSettings.GetComponent<PlayerMainEngineParticles>();
            mainEngineParticles.ChangeColor(playerSettings.color);
            var weaponSlot = playerSettings.GetComponent<WeaponSlot>();
            weaponSlot.enabled = true;
        }

        void HandlePlayerKilledEvent(KilledEventPayload payload)
        {
            var killerScore = currentRoundScores.GetScoreForPlayer<SingleRoundUndeadScore>(payload.playerNumberOfKiller);
            var killedScore = currentRoundScores.GetScoreForPlayer<SingleRoundUndeadScore>(payload.playerNumberOfKilledPlayer);

            if (killerScore.Undead && killedScore.Undead)
            {
                // Undead player killed another undead player. 
                // This is uninteresting. Won't generate a Scoring event
                return;
            }

            if (payload.playerNumberOfKilledPlayer == payload.playerNumberOfKiller)
            {
                // Player killed itself, should become undead. Won't generate a Scoring event
                killedScore.Undead = true;
                MakeUndead(PlayerFetcher.FindPlayerByPlayerNumber(killedScore.PlayerNumber()));
            }
            else 
            {
                // Player killed another player. Will generate a Scoring event
                var scoreForKillV2 = killerScore.Undead ? UndeadKillPoints : AliveKillPoints;
                killerScore.IncreaseScoreBy(scoreForKillV2);

                if (!killedScore.Undead)
                {
                    killedScore.Undead = true;
                    MakeUndead(PlayerFetcher.FindPlayerByPlayerNumber(killedScore.PlayerNumber()));
                }
                AFEventManager.INSTANCE.PostEvent(AFEvents.Score(payload.playerNumberOfKiller, killerScore.Score(), killerScore.Score()));
            }
            killedScore.Deaths++;

            if (currentRoundScores.DoTrueForAllCheck(score => ((SingleRoundUndeadScore)score).Undead))
            {
                if (IsEntireGameOver())
                {
                    Debug.Log("Game over, all players are undead!");
                    var winningScoreForRound = currentRoundScores.SortedByDescendingScores()[0];
                    AFEventManager.INSTANCE.PostEvent(AFEvents.GameOver(winningScoreForRound.PlayerNumber(), winningScoreForRound.Score(), winningScoreForRound.PlayerColor()));
                }
                else
                {
                    Debug.Log("Round over, all players are undead!");
                    var winningScore = currentRoundScores.SortedByDescendingScores()[0];
                    AFEventManager.INSTANCE.PostEvent(AFEvents.RoundOver(winningScore.PlayerNumber(), winningScore.Score(), winningScore.PlayerColor()));
                }
            }
        }

        private bool IsEntireGameOver()
        {
            return GameSettingsState.INSTANCE.RoundState.CurrentRoundNumber == NumberOfGameRounds;
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type)
            {
                case AFEventType.KILLED:
                    {
                        HandlePlayerKilledEvent((KilledEventPayload)afEvent.payload);
                        return true;
                    }
                case AFEventType.PLAYERS_ALL_CREATED:
                    {
                        var payload = (PlayersAllCreatedPayload)afEvent.payload;
                        HandleGameStart(payload.AllPlayers);
                        return true;
                    }
                case AFEventType.COUNT_DOWN_FINISHED:
                    {
                        var payload = (CountDownFinishedPayload)afEvent.payload;
                        if (!payload.TimerName.Equals(RoundTimerName))
                        {
                            break;
                        }
                        countDownTimerInstance.StopTimer();
                        if (GameSettingsState.INSTANCE.RoundState.CurrentRoundNumber == NumberOfGameRounds)
                        {
                            Debug.Log("Game over, time ran out!");
                            var winningScore = currentRoundScores.SortedByDescendingScores()[0];
                            AFEventManager.INSTANCE.PostEvent(AFEvents.GameOver(winningScore.PlayerNumber(), winningScore.Score(), winningScore.PlayerColor()));
                        }
                        else
                        {
                            Debug.Log("Round over, time ran out!");
                            var winningScore = currentRoundScores.SortedByDescendingScores()[0];
                            AFEventManager.INSTANCE.PostEvent(AFEvents.RoundOver(winningScore.PlayerNumber(), winningScore.Score(), winningScore.PlayerColor()));
                        }
                        break;
                    }
                case AFEventType.GAME_START:
                    {
                        countDownTimerInstance.StartTimer();
                        break;
                    }
            }
            return false;
        }
    }
}
