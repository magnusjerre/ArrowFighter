using UnityEngine;
using Jerre.Events;
using Jerre.Pause;
using Jerre.Managers;

namespace Jerre
{
    [RequireComponent(typeof(PlayerSettings))]
    public class PauseMenuTriggerer : MonoBehaviour
    {
        private PlayerSettings playerSettings;
        private PlayerInputComponent playerInputComponent;
        private GameStartAndEndDelayManager startDelayManager;

        private void Awake()
        {
            playerSettings = GetComponent<PlayerSettings>();
            playerInputComponent = GetComponent<PlayerInputComponent>();
            startDelayManager = GameObject.FindObjectOfType<GameStartAndEndDelayManager>();
        }

        void Update()
        {
            var input = playerInputComponent.input;
            if (!startDelayManager.IsAwaiting && input.JoinLeave)
            {
                AFEventManager.INSTANCE.PostEvent(AFEvents.PauseMenuEnable(playerSettings.playerNumber, playerSettings.color));
            }
        }
    }
}
