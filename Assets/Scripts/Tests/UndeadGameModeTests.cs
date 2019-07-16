using Jerre;
using Jerre.GameMode.Undead;
using Jerre.Utils;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class UndeadGameModeTests
    {

        [Test]
        public void SelectPlayersToStartRound_should_choose_randomly_from_all_since_none_have_started_as_undead()
        {
            CompleteGameScores<IScore> gameScores = new CompleteGameScores<IScore>();
            gameScores.StartNewRound();
            var round = gameScores.GetCurrentRoundScores();
            round.AddScoreForPlayer(new UndeadScore(Color.white, 1, 0, 0, false, false));
            round.AddScoreForPlayer(new UndeadScore(Color.white, 2, 0, 0, false, false));

            var playersToStart = UndeadGameMode.SelectPlayersToStartAsUndead(gameScores, 1);
            Assert.AreEqual(playersToStart.Length, 1);
        }

        [Test]
        public void SelectPLayerstoStartRoundV2()
        {
            CompleteGameScores<IScore> gameScores = new CompleteGameScores<IScore>();
            gameScores.StartNewRound();
            var round1 = gameScores.GetCurrentRoundScores();
            round1.AddScoreForPlayer(new UndeadScore(Color.white, 1, 0, 0, false, true));
            round1.AddScoreForPlayer(new UndeadScore(Color.white, 2, 0, 0, false, false));

            gameScores.StartNewRound();
            var round2 = gameScores.GetCurrentRoundScores();
            round2.AddScoreForPlayer(new UndeadScore(Color.white, 1, 0, 0, false, false));
            round2.AddScoreForPlayer(new UndeadScore(Color.white, 2, 0, 0, false, false));

            var playersToStartAsUndead = UndeadGameMode.SelectPlayersToStartAsUndead(gameScores, 1);
            Assert.AreEqual(playersToStartAsUndead.Length, 1);
            Assert.AreEqual(playersToStartAsUndead[0], 2);
        }

        [Test]
        public void SelectPLayerstoStartRound_multiple()
        {
            CompleteGameScores<IScore> gameScores = new CompleteGameScores<IScore>();
            gameScores.StartNewRound();
            var round1 = gameScores.GetCurrentRoundScores();
            round1.AddScoreForPlayer(new UndeadScore(Color.white, 1, 0, 0, false, true));
            round1.AddScoreForPlayer(new UndeadScore(Color.white, 2, 0, 0, false, false));

            gameScores.StartNewRound();
            var round2 = gameScores.GetCurrentRoundScores();
            round2.AddScoreForPlayer(new UndeadScore(Color.white, 1, 0, 0, false, false));
            round2.AddScoreForPlayer(new UndeadScore(Color.white, 2, 0, 0, false, true));

            gameScores.StartNewRound();
            var round3 = gameScores.GetCurrentRoundScores();
            round3.AddScoreForPlayer(new UndeadScore(Color.white, 1, 0, 0, false, false));
            round3.AddScoreForPlayer(new UndeadScore(Color.white, 2, 0, 0, false, false));

            var playersToStartAsUndead = UndeadGameMode.SelectPlayersToStartAsUndead(gameScores, 1);
            Assert.AreEqual(playersToStartAsUndead.Length, 1);
        }

        [Test]
        public void SelectPLayerstoStartRound_three_players_round_two()
        {
            CompleteGameScores<IScore> gameScores = new CompleteGameScores<IScore>();
            gameScores.StartNewRound();
            var round1 = gameScores.GetCurrentRoundScores();
            round1.AddScoreForPlayer(new UndeadScore(Color.white, 1, 0, 0, false, true));
            round1.AddScoreForPlayer(new UndeadScore(Color.white, 2, 0, 0, false, false));
            round1.AddScoreForPlayer(new UndeadScore(Color.white, 3, 0, 0, false, true));

            gameScores.StartNewRound();
            var round2 = gameScores.GetCurrentRoundScores();
            round2.AddScoreForPlayer(new UndeadScore(Color.white, 1, 0, 0, false, false));
            round2.AddScoreForPlayer(new UndeadScore(Color.white, 2, 0, 0, false, false));
            round2.AddScoreForPlayer(new UndeadScore(Color.white, 2, 0, 0, false, false));

            var playersToStartAsUndead = UndeadGameMode.SelectPlayersToStartAsUndead(gameScores, 2);
            Assert.AreEqual(playersToStartAsUndead.Length, 2);
            Assert.IsTrue(ArrayUtils.Contains(2, playersToStartAsUndead));
            Assert.AreEqual(1, ArrayUtils.CountOccurrences(2, playersToStartAsUndead));
        }
    }
}
