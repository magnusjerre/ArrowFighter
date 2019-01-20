using Jerre.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        public PlayerSettings playerPrefab;

        private Dictionary<int, PlayerSettings> playerNumberMap;
        private SpawnPointManager spawnPointManager;
        private ScoreUIManager scoreUIManager;

        private Color[] playerColors;
        private int indexOfNextColor = 0;

        private void Awake()
        {
            playerNumberMap = new Dictionary<int, PlayerSettings>();
            playerColors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow };
        }

        // Start is called before the first frame update
        void Start()
        {
            spawnPointManager = GameObject.FindObjectOfType<SpawnPointManager>();
            scoreUIManager = GameObject.FindObjectOfType<ScoreUIManager>();
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
            newPlayer.playerNumber = playerNumber;
            playerNumberMap.Add(playerNumber, newPlayer);
            var playerColor = NextColor();
            newPlayer.color = playerColor;
            scoreUIManager.AddScoreForPlayer(0, 10, playerNumber, playerColor);
        }

        private void RemovePlayer(int playerNumber)
        {
            var playerToRemove = playerNumberMap[playerNumber];
            if (playerNumberMap.Remove(playerNumber))
            {
                Destroy(playerToRemove.gameObject);
            }
            scoreUIManager.RemoveScoreForPlayer(playerNumber);
        }

        private Color NextColor()
        {
            var color = playerColors[indexOfNextColor];
            indexOfNextColor = (indexOfNextColor + 1) % playerColors.Length;
            return color;
        }
    }
}
