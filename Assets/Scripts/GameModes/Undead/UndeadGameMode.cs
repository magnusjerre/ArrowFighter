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
            
            // Setting up scoring resources
            currentRoundScores = PlayersState.INSTANCE.gameScores.StartNewRound();
            PlayersState.INSTANCE.gameScoresAccumulator = (acc, current) => UndeadScore.Accumulator((UndeadScore)acc, (UndeadScore)current);

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

        public static int[] SelectPlayersToStartAsUndead(CompleteGameScores<IScore> gameScores, int nStartingUndead)
        {
            var undeadAndNot = ExtractWhichPlayersHaveStartedAsUndeadAndNot(gameScores);
            var nPlayersThatShouldStartAsUndead = Mathf.Min(nStartingUndead, Mathf.Max(undeadAndNot[true].Count + undeadAndNot[false].Count - 1, 0));
            var nPlayersThatHaventStartedAsUndeadYet = Mathf.Min(nPlayersThatShouldStartAsUndead, undeadAndNot[false].Count);
            var nPlayersThatHaveStartedAsUndeadYet = Mathf.Min(Mathf.Max(0, nPlayersThatShouldStartAsUndead - nPlayersThatHaventStartedAsUndeadYet), undeadAndNot[true].Count);

            var extractedNeverStartedAsUndead = CollectionUtils.ExtractRandomValues(undeadAndNot[false], nPlayersThatHaventStartedAsUndeadYet);
            var extractedHasStartedAsUndead = CollectionUtils.ExtractRandomValues(undeadAndNot[true], nPlayersThatHaveStartedAsUndeadYet);

            return ArrayUtils.Merge(extractedNeverStartedAsUndead, extractedHasStartedAsUndead);
        }

        private static Dictionary<bool, List<int>> ExtractWhichPlayersHaveStartedAsUndeadAndNot(CompleteGameScores<IScore> gameScores)
        {
            Dictionary<int, bool> PlayerHasStartedAsUndead = new Dictionary<int, bool>();
            foreach (var round in gameScores.roundScores)
            {
                foreach (var playerScore in round.Scores)
                {
                    if (PlayerHasStartedAsUndead.ContainsKey(playerScore.PlayerNumber()))
                    {
                        PlayerHasStartedAsUndead[playerScore.PlayerNumber()] = PlayerHasStartedAsUndead[playerScore.PlayerNumber()] || ((UndeadScore)playerScore).StartedAsUndead;
                    }
                    else
                    {
                        PlayerHasStartedAsUndead[playerScore.PlayerNumber()] = ((UndeadScore)playerScore).StartedAsUndead;
                    }
                }
            }

            var notStarted = new List<int>();
            var hasStarted = new List<int>();

            foreach (var keyvalue in PlayerHasStartedAsUndead)
            {
                if (keyvalue.Value)
                {
                    hasStarted.Add(keyvalue.Key);
                } else
                {
                    notStarted.Add(keyvalue.Key);
                }
            }
            var output = new Dictionary<bool, List<int>>
            {
                { true, hasStarted },
                { false, notStarted }
            };
            return output;
        }
        

        void HandleGameStart(List<PlayerSettings> allPlayers)
        {
            foreach (var ps in allPlayers)
            {
                var playerScore = new UndeadScore(ps.playerNumber, ps.color);
                currentRoundScores.AddScoreForPlayer(playerScore);
            }

            var undeadPlayerNumbers = SelectPlayersToStartAsUndead(PlayersState.INSTANCE.gameScores, NumberOfStartingUndead);
            foreach (var playerScore in currentRoundScores.Scores)
            { 
                if (ArrayUtils.Contains(playerScore.PlayerNumber(), undeadPlayerNumbers))
                {
                    ((UndeadScore)playerScore).Undead = true;
                    ((UndeadScore)playerScore).StartedAsUndead = true;
                    MakeUndead(allPlayers.Find(ps => ps.playerNumber == playerScore.PlayerNumber()));
                } else
                {
                    MakeLiving(allPlayers.Find(ps => ps.playerNumber == playerScore.PlayerNumber()));
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
            var killerScore = currentRoundScores.GetScoreForPlayer<UndeadScore>(payload.playerNumberOfKiller);
            var killedScore = currentRoundScores.GetScoreForPlayer<UndeadScore>(payload.playerNumberOfKilledPlayer);

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

            if (currentRoundScores.DoTrueForAllCheck(score => ((UndeadScore)score).Undead))
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
            return PlayersState.INSTANCE.gameScores.CurrentRoundNumber == NumberOfGameRounds;
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
                        if (IsEntireGameOver())
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
