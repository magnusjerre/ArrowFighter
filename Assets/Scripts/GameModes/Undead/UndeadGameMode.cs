using Jerre.Events;
using Jerre.GameSettings;
using Jerre.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.GameMode.Undead
{
    public class UndeadGameMode : MonoBehaviour, IAFEventListener
    {
        public int GamePlayTimeInSeconds = 30;
        public int NumberOfGameRounds = 2;
        public int NumberOfStartingUndead = 1;
        public int UndeadRespawnTimeInSeconds = 1;
        public int AliveKillPoints = 1;
        public int UndeadKillPoints = 2;
        public Color UndeadColor = Color.gray;
        // TODO: Gi straff for å drepe team mates?


        WholeGameUndeadScore score;

        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
            score = new WholeGameUndeadScore();

            var settings = (UndeadGameSettings)GameSettingsState.INSTANCE.GameModeSettings;
            GamePlayTimeInSeconds = settings.GamePlayerTimeInSeconds;
            NumberOfGameRounds = settings.NumberOfGameRounds;
            NumberOfStartingUndead = settings.StartingUndead;
            UndeadRespawnTimeInSeconds = settings.UndeadRespawnTimeInSeconds;
            AliveKillPoints = settings.AliveKillPoints;
            UndeadKillPoints = settings.UndeadKillPoints;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int[] SelectPlayersToStartAsUndead(PlayerSettings[] playerSettings)
        {



            return new int[] { 1 };
        }

        public List<PlayerScore> GeneratePlayerScores()
        {
            var totalScore = score.CalcualtePlayerScoresWithoutPositionOrColorForRound(score.roundScores);
            var scoresSorted = new List<PlayerScore>();
            foreach (var keyValue in totalScore)
            {
                scoresSorted.Add(new PlayerScore(keyValue.Key, PlayersState.INSTANCE.GetPlayerColor(keyValue.Key), -1, keyValue.Value));
            }
            scoresSorted.Sort();

            var scores = new List<PlayerScore>();
            for (var i = 0; i < scoresSorted.Count; i++)
            {
                var score = scoresSorted[i];
                scores.Add(new PlayerScore(score.PlayerNumber, score.PlayerColor, i + 1, score.Score));
            }

            return scores;
        }

        void HandleGameStart()
        {
            var playerSettings = GameObject.FindObjectsOfType<PlayerSettings>();
            var undeadPlayerNumbers = SelectPlayersToStartAsUndead(playerSettings);
            foreach (var ps in playerSettings)
            {
                var currentRoundScore = score.GetCurrentRoundScoreForPlayer(ps.playerNumber);
                MakeLiving(ps);
                for (var i = 0; i < undeadPlayerNumbers.Length; i++)
                {
                    if (ps.playerNumber == undeadPlayerNumbers[i])
                    {
                        currentRoundScore.Undead = true;
                        currentRoundScore.StartedAsUndead = true;
                        MakeUndead(ps);
                    }
                }
            }
        }

        void MakeUndead(PlayerSettings playerSettings)
        {
            var mainEngineParticles = playerSettings.GetComponent<PlayerMainEngineParticles>();
            mainEngineParticles.ChangeColor(UndeadColor);
            var playerWeapon = playerSettings.GetComponent<PlayerWeapon>();
            playerWeapon.enabled = false;
        }

        void MakeLiving(PlayerSettings playerSettings)
        {
            var mainEngineParticles = playerSettings.GetComponent<PlayerMainEngineParticles>();
            mainEngineParticles.ChangeColor(playerSettings.color);
            var playerWeapon = playerSettings.GetComponent<PlayerWeapon>();
            playerWeapon.enabled = true;
        }


        public bool HandleEvent(AFEvent afEvent)
        {
            switch(afEvent.type)
            {
                case AFEventType.KILLED:
                    {
                        var payload = (KilledEventPayload)afEvent.payload;
                        var killerScore = score.GetCurrentRoundScoreForPlayer(payload.playerNumberOfKiller);
                        var killedScore = score.GetCurrentRoundScoreForPlayer(payload.playerNumberOfKilledPlayer);

                        int scoreForKill = killedScore.Undead != killerScore.Undead && killerScore.PlayerNumber != killedScore.PlayerNumber ? (killerScore.Undead ? UndeadKillPoints : AliveKillPoints) : 0;
                        killerScore.Score += scoreForKill;
                        if (scoreForKill != 0)
                        {
                            AFEventManager.INSTANCE.PostEvent(AFEvents.Score(payload.playerNumberOfKiller, killerScore.Score, killerScore.Score));
                        }

                        killedScore.Undead = true;
                        killedScore.Deaths++;
                        Debug.Log("number of game rounds: " + NumberOfGameRounds);
                        Debug.Log("game state, current round number: " + GameSettingsState.INSTANCE.RoundState.CurrentRoundNumber);
                        if (score.AllPlayersDead() || killerScore.Score >= GameSettingsState.INSTANCE.pointsToWin)
                        {
                            if (GameSettingsState.INSTANCE.RoundState.CurrentRoundNumber == NumberOfGameRounds)
                            {
                                Debug.Log("Game over, all players are undead!");
                                var scores = GeneratePlayerScores();
                                var winningScore = scores[0];
                                PlayersState.INSTANCE.SetScores(scores);
                                GameSettingsState.INSTANCE.RoundState.roundScores.Add(score);
                                AFEventManager.INSTANCE.PostEvent(AFEvents.GameOver(winningScore.PlayerNumber, winningScore.Score, winningScore.PlayerColor));
                            }
                            else
                            {
                                Debug.Log("Round over, all players are undead!");
                                var scores = GeneratePlayerScores();
                                var winningScore = scores[0];
                                PlayersState.INSTANCE.SetScores(scores);
                                GameSettingsState.INSTANCE.RoundState.roundScores.Add(score);
                                AFEventManager.INSTANCE.PostEvent(AFEvents.RoundOver(winningScore.PlayerNumber, winningScore.Score, winningScore.PlayerColor));
                            }
                        }
                        else
                        {
                            var playerSettings = PlayerFetcher.FindPlayerByPlayerNumber(killedScore.PlayerNumber);
                            MakeUndead(playerSettings);
                        }

                        return true;
                    }
                case AFEventType.PLAYER_MENU_BAR_UI_CREATED:
                    {
                        HandleGameStart();
                        return true;
                    }
            }
            return false;
        }
    }
}
