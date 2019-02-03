using System.Collections.Generic;
using UnityEngine;

namespace Jerre.MainMenu
{
    public class PlayerJoinManager : MonoBehaviour
    {
        public RectTransform WaitingForPlayerJoin;
        public RectTransform JoinedPlayers;
        public ParticleSystem playerExplosionParticlesPrefab;
        public float StartWaitTime = 0.25f;

        private ColorManager colorManager;

        private Dictionary<int, PlayerMenuSettings> playerNumberMap;    //int is the controller number, doesn't have to match the player number

        private void Awake()
        {
            playerNumberMap = new Dictionary<int, PlayerMenuSettings>();
        }

        void Start()
        {
            DisableAllChildren(JoinedPlayers);
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

            for (var i = 0; i < JoinedPlayers.childCount; i++)
            {
                var child = JoinedPlayers.GetChild(i).gameObject;
                if (!child.activeInHierarchy)
                {
                    WaitingForPlayerJoin.GetChild(i).gameObject.SetActive(false);
                    child.SetActive(true);
                    var playerSettings = child.GetComponentInChildren<PlayerMenuSettings>();
                    playerSettings.Color = colorManager.ExtractNextColor();
                    playerSettings.Number = controllerIndex;
                    playerSettings.Ready = false;
                    var colorScript = child.GetComponentInChildren<PlayerColorScript>();
                    colorScript.ColorManager = colorManager;
                    colorScript.UpdateColor();
                    var readyScript = child.GetComponentInChildren<PlayerReady>();
                    readyScript.playerJoinManager = this;
                    readyScript.Reset();
                    playerNumberMap.Add(controllerIndex, playerSettings);
                    break;
                }
            }

        }

        void RemovePlayerIfNotReady(int playerNumber)
        {
            if (!playerNumberMap.ContainsKey(playerNumber))
            {
                return;
            }

            var index = FindIndexForPlayerNumber(playerNumber, JoinedPlayers);
            var playerChild = JoinedPlayers.GetChild(index);
            var playerSettings = playerChild.GetComponentInChildren<PlayerMenuSettings>();
            if (playerSettings.Ready)
            {
                playerSettings.GetComponent<PlayerReady>().Reset();
                return;
            }
            colorManager.ReturnColor(playerSettings.Color);
            playerChild.gameObject.SetActive(false);
            WaitingForPlayerJoin.GetChild(index).gameObject.SetActive(true);
            playerNumberMap.Remove(playerNumber);
        }

        private int FindIndexForPlayerNumber(int playerNumber, RectTransform playerContainer)
        {
            for (var i = 0; i < playerContainer.childCount; i++)
            {
                var settings = playerContainer.GetChild(i).GetComponentInChildren<PlayerMenuSettings>();
                if (settings.Number == playerNumber) return i;
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
                    entry.Value.CanListenForInput = false;
                }
                Invoke("TriggerStart", StartWaitTime);
            }
        }
        
        void TriggerStart()
        {
            var timeUntilNextSceneLoad = 1f;
            for (var i = 0; i < JoinedPlayers.childCount; i++)
            {
                var child = JoinedPlayers.GetChild(i);
                if (child.gameObject.activeSelf)
                {
                    var playerSettings = child.GetComponentInChildren<PlayerMenuSettings>();
                    DisableAllChildren(child);
                    var particles = Instantiate(playerExplosionParticlesPrefab, child.transform.position, child.transform.rotation);
                    ParticleSystem.MinMaxGradient gradient = new ParticleSystem.MinMaxGradient(playerSettings.Color, playerSettings.Color);
                    var mainModule = particles.main;
                    mainModule.startColor = gradient;
                    timeUntilNextSceneLoad = mainModule.duration;
                }
            }
            Invoke("TriggerNextSceneLoad", timeUntilNextSceneLoad);
        }

        void TriggerNextSceneLoad()
        {
            Debug.Log("Bo! Next scene should load");
        }

        public void NotifyPlayerNotReady(int playerNumber)
        {
            PlayersState.INSTANCE.RemovePlayer(playerNumberMap[playerNumber]);
        }
    }
}
