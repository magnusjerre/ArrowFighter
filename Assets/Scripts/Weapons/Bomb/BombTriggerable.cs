using Jerre.Events;
using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (BombSettings))]
    public class BombTriggerable : MonoBehaviour, IAFEventListener
    {
        public ParticleSystem explosionParticlesPrefab;
        private BombSettings settings;
        private AFEventManager eventManager;
        private bool AlreadyTriggered = false;

        void Start()
        {
            settings = GetComponent<BombSettings>();
            eventManager = GameObject.FindObjectOfType<AFEventManager>();
            eventManager.AddListener(this);
            Invoke("TriggerExplosionDueToLifeTime", settings.MaxLifeTimeWithoutExploding);
        }

        public void TriggerExplosion()
        {
            if (AlreadyTriggered) return;
            
            AlreadyTriggered = true;

            var colliders = Physics.OverlapSphere(transform.position, settings.BlastRadius);
            for (var i = 0; i < colliders.Length; i++)
            {
                var playerSettings = colliders[i].GetComponent<PlayerSettings>();
                var playerHealth = colliders[i].GetComponent<PlayerHealth>();
                var playerPhysics = colliders[i].GetComponent<PlayerPhysics>();

                if (playerSettings != null && playerHealth != null && playerPhysics != null)
                {
                    var explosionDirection = (playerSettings.transform.position - transform.position);
                    var distance = explosionDirection.magnitude;
                    var multiplier = 1f - distance / settings.BlastRadius;
                    var blastDamage = Mathf.Max(1, Mathf.RoundToInt(settings.BlastDamage * multiplier));

                    var resultingSpeed = playerPhysics.Speed * playerPhysics.MovementDirection + explosionDirection.normalized * settings.BlastAcceleration * Time.deltaTime;
                    playerPhysics.Speed = resultingSpeed.magnitude;
                    playerPhysics.MovementDirection = resultingSpeed.normalized;

                    if (playerHealth.DoDamage(blastDamage))
                    {
                        eventManager.PostEvent(AFEvents.Kill(settings.PlayerOwnerNumber, playerSettings.playerNumber));
                    }
                }
            }

            eventManager.RemoveListener(this);
            var explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, transform.rotation);
            var particlesMainModule = explosionParticles.main;
            particlesMainModule.startColor = settings.color;
            Destroy(gameObject);
        }

        private void TriggerExplosionDueToLifeTime()
        {
            if (AlreadyTriggered) return;

            eventManager.PostEvent(AFEvents.BombTrigger(settings.PlayerOwnerNumber, false));
            TriggerExplosion();
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            if (afEvent.type == AFEventType.BOMB_TRIGGER)
            {
                var payload = (BombTriggerPayload)afEvent.payload;
                if (payload.OwnerPlayerNumber == settings.PlayerOwnerNumber && payload.TriggeredByPlayerInput)
                {
                    TriggerExplosion();
                }
            }
            return false;
        }
    }
}
