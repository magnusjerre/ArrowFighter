using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerSettings)), RequireComponent(typeof(PlayerPhysics))]
    public class PlayerMainEngineParticles : MonoBehaviour
    {
        public ParticleSystem MainEngineParticleSystem;
        public float MaxEmissionRate = 100;
        public float MinEmissionRate = 10;
        public Color ParticleColor;

        private PlayerSettings settings;
        private PlayerPhysics physics;
        private ParticleSystem.EmissionModule emission;

        void Start()
        {
            settings = GetComponent<PlayerSettings>();
            physics = GetComponent<PlayerPhysics>();
            emission = MainEngineParticleSystem.emission;
            var mainModule = MainEngineParticleSystem.main;
            mainModule.startColor = ParticleColor;
        }

        void Update()
        {
            emission.rateOverTime = Mathf.Max((physics.Speed / settings.MaxSpeed) * MaxEmissionRate, MinEmissionRate);
            if (!MainEngineParticleSystem.isPlaying)
            {
                MainEngineParticleSystem.Play();
            }
        }

        public void ChangeColor(Color newColor)
        {
            ParticleColor = newColor;
            if (MainEngineParticleSystem != null)
            {
                var mainModule = MainEngineParticleSystem.main;
                mainModule.startColor = ParticleColor;
            }
        }
    }
}
