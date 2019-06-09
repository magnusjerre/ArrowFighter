using Jerre;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.GameMode
{
    [RequireComponent(typeof (Text))]
    public class SetTextToMatchameMode : MonoBehaviour
    {
        public string prefix = "";
        public string postfix = "";

        void Start()
        {
            var selectedGameMode = PlayersState.INSTANCE.selectedGameMode;
            string gameModeText = selectedGameMode == GameModes.FREE_FOR_ALL ? "FREE FOR ALL" : (selectedGameMode == GameModes.UNDEAD ? "UNDEAD" : "UNKNWON");
            GetComponent<Text>().text = prefix + gameModeText + postfix;
        }

    }
}