using UnityEngine;

namespace Jerre.GameMode.Undead
{
    [RequireComponent(typeof (PlayerSettings)), RequireComponent(typeof (PlayerMainEngineParticles))]
    public class UndeadPlayerColor : MonoBehaviour
    {
        public Color UndeadColor;
        public Color OriginalPlayerColor;

        private PlayerSettings playerSettings;
        private PlayerColor playerColor;
        private PlayerMainEngineParticles playerMainEngineParticles;


        void Awake()
        {
            playerSettings = GetComponent<PlayerSettings>();
            playerColor = GetComponent<PlayerColor>();
            playerMainEngineParticles = GetComponent<PlayerMainEngineParticles>();
            OriginalPlayerColor = playerSettings.color;
        }

        void Update()
        {
            //playerMainEngineParticles.ChangeColor(Color.gray);
        }

    }
}