using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Jerre.MainMenu
{
    public class PlayerJoinManager : MonoBehaviour
    {
        public ParticleSystem playerExplosionParticlesPrefab;
        public float StartWaitTime = 0.25f;

        public RectTransform WaitForPlayerPrefab;
        public RectTransform PlayerJoinedPrefab;
        public RectTransform[] PlayerPositions;




        private ColorManager colorManager;

        private Dictionary<int, PlayerMenuSettings> playerNumberMap;    //int is the controller number, doesn't have to match the player number

        private void Awake()
        {
            playerNumberMap = new Dictionary<int, PlayerMenuSettings>();
        }

        void Start()
        {
            FirstTimeSetupOfWaitingForPlayers();
            colorManager = GetComponent<ColorManager>();
        }

        void Update()
        {
            for (int i = 1; i <= 4; i++)
            {
                var joinLeaveKeyName = PlayerInputTags.JOIN_LEAVE + i;
                var leaveKeyName = PlayerInputTags.FIRE2 + i;
                if (Input.GetButtonDown(joinLeaveKeyName))
                {
                    if (!playerNumberMap.ContainsKey(i))
                    {
                        AddPlayer(i);
                    }
                }
                else if (Input.GetButtonDown(leaveKeyName)) {
                    RemovePlayerIfNotReady(i);
                }
            }
        }

        void DisableAllChildren(Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        void AddPlayer(int controllerIndex)
        {
            if (playerNumberMap.ContainsKey(controllerIndex))
            {
                return;
            }

            for (var i = 0; i < PlayerPositions.Length; i++)
            {
                var playerPosition = PlayerPositions[i];
                var waitingForPlayerChild = playerPosition.GetComponentInChildren<WaitingForPlayer>();
                if (waitingForPlayerChild == null) continue;    // This transform already has a player child, can't add to it
                Destroy(waitingForPlayerChild.gameObject);

                var playerIcon = Instantiate(PlayerJoinedPrefab, playerPosition);
                var playerSettings = playerIcon.GetComponentInChildren<PlayerMenuSettings>();
                playerSettings.Color = colorManager.ExtractNextColor();
                playerSettings.Number = controllerIndex;
                var colorScript = playerIcon.GetComponentInChildren<PlayerColorScript>();
                colorScript.ColorManager = colorManager;
                var readyScript = playerIcon.GetComponentInChildren<PlayerReady>();
                readyScript.playerJoinManager = this;
                playerNumberMap.Add(controllerIndex, playerSettings);
                break;
            }
        }

        void RemovePlayerIfNotReady(int playerNumber)
        {
            if (!playerNumberMap.ContainsKey(playerNumber))
            {
                return;
            }

            var index = FindIndexOfContainerForPlayerNumber(playerNumber);
            var playerSettings = playerNumberMap[playerNumber];

            if (playerSettings.mm_Ready)
            {
                playerSettings.GetComponent<PlayerReady>().Reset();
                return;
            }
            colorManager.ReturnColor(playerSettings.Color);
            Destroy(playerSettings.gameObject);
            var playerPositionToUpdate = PlayerPositions[index];
            Instantiate(WaitForPlayerPrefab, playerPositionToUpdate);
            playerNumberMap.Remove(playerNumber);
        }

        private int FindIndexOfContainerForPlayerNumber(int playerNumber)
        {
            for (var i = 0; i < PlayerPositions.Length; i++)
            {
                var playerPosition = PlayerPositions[i];
                var playerSettings = playerPosition.GetComponentInChildren<PlayerMenuSettings>();
                Debug.Log("i: " + i + ", PlayerSettings: " + playerSettings);
                if (playerSettings != null && playerSettings.Number == playerNumber)
                {
                    return i;
                }
            }
            return -1;
        }

        public void NotifyPlayerReady(int playerNumber)
        {
            PlayersState.INSTANCE.AddPlayer(playerNumberMap[playerNumber]);
            if (PlayersState.INSTANCE.ReadyPlayersCount == playerNumberMap.Count)
            {
                foreach (var entry in playerNumberMap)
                {
                    entry.Value.mm_CanListenForInput = false;
                }
                Invoke("TriggerStart", StartWaitTime);
            }
        }
        
        void TriggerStart()
        {
            var timeUntilNextSceneLoad = 1f;
            foreach (var keyValue in playerNumberMap)
            {
                var playerSettings = keyValue.Value;

                var particles = Instantiate(playerExplosionParticlesPrefab, playerSettings.transform.position, playerSettings.transform.rotation);
                ParticleSystem.MinMaxGradient gradient = new ParticleSystem.MinMaxGradient(playerSettings.Color, playerSettings.Color);
                var mainModule = particles.main;
                mainModule.startColor = gradient;
                timeUntilNextSceneLoad = mainModule.duration;
            }
            Invoke("TriggerNextSceneLoad", timeUntilNextSceneLoad);
        }

        void TriggerNextSceneLoad()
        {
            Debug.Log("Bo! Next scene should load");
            SceneManager.LoadScene(SceneNames.GAME_SCENE, LoadSceneMode.Single);
        }

        public void NotifyPlayerNotReady(int playerNumber)
        {
            PlayersState.INSTANCE.RemovePlayer(playerNumberMap[playerNumber]);
        }

        void FirstTimeSetupOfWaitingForPlayers()
        {
            for (var i = 0; i < PlayerPositions.Length; i++)
            {
                var playerPosition = PlayerPositions[i];
                Instantiate(WaitForPlayerPrefab, playerPosition);
            }
        }
    }
}
