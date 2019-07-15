using UnityEngine;

namespace Jerre
{
    public class PlayerComponentsEnabler
    {
        public static void EnableOrDisableAllPlayersInputResponses(bool enabled) {
            var allPlayers = GameObject.FindObjectsOfType<PlayerSettings>();
            for (var i = 0; i < allPlayers.Length; i++) {
                var playerSettings = allPlayers[i];
                EnableOrDisablePlayerInputResponses(playerSettings, enabled);
            }
        }

        public static void EnableOrDisablePlayerInputResponses(PlayerSettings playerSettings, bool enabled) {
            var playerComponents = playerSettings.GetComponents<UsePlayerInput>();
            for (var i = 0; i < playerComponents.Length; i++) {
                playerComponents[i].SetUsePlayerInput(enabled);
            }
        }


        public static void EnableOrDisablePlayer(PlayerSettings playerSettings, bool enabled)
        {
            playerSettings.GetComponent<PlayerHealth>().enabled = enabled;
            playerSettings.GetComponent<Collider>().enabled = enabled;
            playerSettings.GetComponent<PlayerInputComponent>().enabled = enabled;
            SetActiveForAllChildren(playerSettings.transform, enabled);
        }

        private static void SetActiveForAllChildren(Transform someTransform, bool active)
        {
            for (var i = 0; i < someTransform.childCount; i++)
            {
                someTransform.GetChild(i).gameObject.SetActive(active);
            }
        }
    }
}
