using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof(PlayerBoost))]
    public class PlayerBoostEngineParticles : MonoBehaviour
    {
        public ParticleSystem BoostEngineParticleSystem;

        private PlayerBoost boostSystem;

        // Start is called before the first frame update
        void Start()
        {
            boostSystem = GetComponent<PlayerBoost>();
            if (boostSystem.boosting)
            {
                BoostEngineParticleSystem.Play();
            } else
            {
                BoostEngineParticleSystem.Stop();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (boostSystem.boosting && !BoostEngineParticleSystem.isPlaying)
            {
                BoostEngineParticleSystem.Play();
            }
            else if (!boostSystem.boosting && BoostEngineParticleSystem.isPlaying)
            {
                BoostEngineParticleSystem.Stop();
            }
        }
    }
}
