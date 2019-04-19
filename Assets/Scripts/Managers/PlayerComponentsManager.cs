using UnityEngine;
using System.Collections;
using Jerre.Events;

namespace Jerre
{
    public class PlayerComponentsManager : MonoBehaviour
    {
        void Start()
        {
            
        }

        public void EnableOrDisableAllPlayersInputResponses(bool enabled) {
            var allPlayers = GameObject.FindObjectsOfType<PlayerSettings>();
            for (var i = 0; i < allPlayers.Length; i++) {
                var playerSettings = allPlayers[i];
                EnableOrDisablePlayerInputResponses(playerSettings, enabled);
            }
        }

        public void EnableOrDisablePlayerInputResponses(PlayerSettings playerSettings, bool enabled) {
            var playerComponents = playerSettings.GetComponents<UsePlayerInput>();
            for (var i = 0; i < playerComponents.Length; i++) {
                playerComponents[i].SetUsePlayerInput(enabled);
            }
        }


        public void EnableOrDisablePlayer(PlayerSettings playerSettings, bool enabled)
        {
            playerSettings.GetComponent<PlayerHealth>().enabled = enabled;
            playerSettings.GetComponent<Collider>().enabled = enabled;
            playerSettings.GetComponent<PlayerInputComponent>().enabled = enabled;
            SetActiveForAllChildren(playerSettings.transform, enabled);
        }

        private void SetActiveForAllChildren(Transform someTransform, bool active)
        {
            for (var i = 0; i < someTransform.childCount; i++)
            {
                someTransform.GetChild(i).gameObject.SetActive(active);
            }
        }
    }
}
