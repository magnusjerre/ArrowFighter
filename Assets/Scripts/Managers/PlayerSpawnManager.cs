using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        public PlayerSettings playerPrefab;

        private Dictionary<int, PlayerSettings> playerNumberMap;
        private SpawnPointManager spawnPointManager;

        private void Awake()
        {
            playerNumberMap = new Dictionary<int, PlayerSettings>();
        }

        // Start is called before the first frame update
        void Start()
        {
            spawnPointManager = GameObject.FindObjectOfType<SpawnPointManager>();
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 1; i <= 4; i++)
            {
                var joinLeaveKeyName = PlayerInputTags.JOIN_LEAVE + i;
                if (Input.GetButtonDown(joinLeaveKeyName)) {
                    if (!playerNumberMap.ContainsKey(i))
                    {
                        AddPlayer(i);
                    } else
                    {
                        RemovePlayer(i);
                    }
                }
            }
        }

        private void AddPlayer(int playerNumber)
        {
            var spawnPoint = spawnPointManager.GetNextSpawnPoint();
            var newPlayer = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            newPlayer.SetPlayerNumber(playerNumber);
            playerNumberMap.Add(playerNumber, newPlayer);
        }

        private void RemovePlayer(int playerNumber)
        {
            var playerToRemove = playerNumberMap[playerNumber];
            if (playerNumberMap.Remove(playerNumber))
            {
                Destroy(playerToRemove.gameObject);
            }
        }
    }
}
