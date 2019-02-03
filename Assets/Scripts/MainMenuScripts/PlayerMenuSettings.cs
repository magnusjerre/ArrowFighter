using UnityEngine;

namespace Jerre.MainMenu
{
    public class PlayerMenuSettings : MonoBehaviour
    {
        public int Number;
        public Color Color;

        // "mm_" indicates that the values are only relevant to the main menu, not the spawning
        // of players in game
        public bool mm_Ready;
        public bool mm_CanListenForInput = true;

        void Start()
        {

        }
    }
}
