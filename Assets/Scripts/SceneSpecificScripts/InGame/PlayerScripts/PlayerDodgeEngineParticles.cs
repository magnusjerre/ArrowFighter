using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof(PlayerDodge))]
    public class PlayerDodgeEngineParticles : MonoBehaviour
    {
        public ParticleSystem DodgeEngineParticles;
        public bool RightDodgeDirection = true;

        private PlayerDodge dodge;

        void Start()
        {
            dodge = GetComponent<PlayerDodge>();
            DodgeEngineParticles.Stop();
        }

        void Update()
        {
            if (isDodging() && !DodgeEngineParticles.isPlaying)
            {
                DodgeEngineParticles.Play();
            } else if (!isDodging() && DodgeEngineParticles.isPlaying)
            {
                DodgeEngineParticles.Stop();
            }
        }

        private bool isDodging()
        {
            if (RightDodgeDirection && dodge.DodgingRight()) return true;
            if (!RightDodgeDirection && dodge.DodgingLeft()) return true;
            return false;
        }
    }
}
