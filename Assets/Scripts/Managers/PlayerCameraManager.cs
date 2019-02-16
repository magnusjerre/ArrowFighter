using UnityEngine;
using System.Collections;
using Jerre.Events;
using System.Collections.Generic;

namespace Jerre
{
    public class PlayerCameraManager : MonoBehaviour, IAFEventListener
    {

        public CameraSettings PlayerCameraSettingsPrefab;

        //int : PlayerNumber, not cameraNumber
        private Dictionary<int, CameraSettings> playerCameraDictionary;
        private AFEventManager eventManager;

        private int nextCameraNumber = 1;

        void Awake()
        {
            playerCameraDictionary = new Dictionary<int, CameraSettings>();
            eventManager = GetComponent<AFEventManager>();
            eventManager.AddListener(this);
        }

        void Start()
        {

        }

        public bool HandleEvent(AFEvent afEvent)
        {

            switch (afEvent.type)
            {
                case AFEventType.PLAYER_JOIN:
                    {
                        var payload = (PlayerJoinPayload)afEvent.payload;
                        CreateCamera(payload.playerNumber);
                        return false;
                    }
                case AFEventType.PLAYER_LEAVE:
                    {
                        var payload = (PlayerLeavePayload)afEvent.payload;
                        RemoveCamera(payload.playerNumber);
                        return false;
                    }
            }
            return false;
        }

        private void CreateCamera(int playerNumber)
        {
            if (playerCameraDictionary.ContainsKey(playerNumber))
            {
                return;
            }

            var playerCamera = Instantiate(PlayerCameraSettingsPrefab);
            playerCamera.TotalNumberOfCameras = playerCameraDictionary.Count + 1;
            playerCamera.CameraNumber = nextCameraNumber++;
            playerCamera.PlayerNumber = playerNumber;
            playerCameraDictionary.Add(playerNumber, playerCamera);

            UpdateCameras();
        }

        private void RemoveCamera(int playerNumber)
        {
            if (!playerCameraDictionary.ContainsKey(playerNumber))
            {
                return;
            }

            Destroy(playerCameraDictionary[playerNumber].gameObject);
            UpdateCameras();
        }

        private void UpdateCameras()
        {
            nextCameraNumber = 1;       // Reset camera number since this will be repopulated

            foreach (var keyvalue in playerCameraDictionary)
            {
                var settings = keyvalue.Value;
                settings.CameraNumber = nextCameraNumber++;
                settings.TotalNumberOfCameras = playerCameraDictionary.Count;

                settings.GetComponent<CameraSize>().UpdateCameraSize();
            }
        }
    }
}
