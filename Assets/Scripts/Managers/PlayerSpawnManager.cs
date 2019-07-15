using Jerre.Events;
using Jerre.GameSettings;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        public PlayerSettings playerPrefab;
        public Transform PlayersContainer;
        public Transform SpawnPointsParent;

        private int nextSpawnIndex = 0;

        void Start()
        {
            var playersToCreate = PlayersState.INSTANCE.GetPlayers();
            if (playersToCreate.Count == 0)
            {
                Debug.Log("0 Players joined the game");
                return;
            }

            //var spawnPointManager = GameObject.FindObjectOfType<SpawnPointManager>();
            var allPlayerSettings = new List<PlayerSettings>(playersToCreate.Count);
            for (var i = 0; i < playersToCreate.Count; i++)
            {
                var playerMenuSettings = playersToCreate[i];
                allPlayerSettings.Add(
                    AddPlayer(
                        playerMenuSettings.Number, 
                        playerMenuSettings.Color, 
                        GetNextSpawnPoint()
                    )
                );
            }
            AFEventManager.INSTANCE.PostEvent(AFEvents.PlayersAllCreated(allPlayerSettings));
        }

        private PlayerSettings AddPlayer(int playerNumber, Color color, Transform spawnPoint)
        {
            var newPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            newPlayer.name = "Player_" + playerNumber;
            newPlayer.playerNumber = playerNumber;
            newPlayer.color = color;
            var basicGameSettings = GameSettingsState.INSTANCE.BasicGameSettings;
            var basicWeaponsSettings = GameSettingsState.INSTANCE.BasicWeaponsSettings;
            newPlayer.MaxSpeed = basicGameSettings.Speed;
            newPlayer.MaxAcceleration = basicGameSettings.MaxAcceleration;
            newPlayer.BoostSpeed = basicGameSettings.BoostSpeed;
            newPlayer.BoostDuration = basicGameSettings.BoostDuration;
            newPlayer.BoostPauseDuration = basicGameSettings.BoostPause;
            newPlayer.MaxHealth = basicGameSettings.Health;
            newPlayer.FireRate = basicWeaponsSettings.FireRate;
            newPlayer.BombPauseTime = basicWeaponsSettings.BombPauseTime;
            newPlayer.transform.parent = PlayersContainer;

            var mainEngineParticles = newPlayer.GetComponent<PlayerMainEngineParticles>();
            mainEngineParticles.ParticleColor = color;
            return newPlayer;
        }

        public Transform GetNextSpawnPoint()
        {
            var spawnPoint = SpawnPointsParent.GetChild(nextSpawnIndex);
            nextSpawnIndex = (nextSpawnIndex + 1) % SpawnPointsParent.childCount;
            return spawnPoint;
        }
    }
}
