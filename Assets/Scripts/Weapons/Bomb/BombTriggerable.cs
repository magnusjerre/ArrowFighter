using Jerre.Events;
using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (BombSettings))]
    public class BombTriggerable : MonoBehaviour, IAFEventListener
    {
        public ParticleSystem explosionParticlesPrefab;
        private BombSettings settings;
        private bool AlreadyTriggered = false;

        void Start()
        {
            settings = GetComponent<BombSettings>();
            AFEventManager.INSTANCE.AddListener(this);
            Invoke("TriggerExplosionDueToLifeTime", settings.MaxLifeTimeWithoutExploding);
        }

        public void TriggerExplosion()
        {
            if (AlreadyTriggered) return;
            
            AlreadyTriggered = true;

            var colliders = Physics.OverlapSphere(transform.position, settings.BlastRadius);
            var playerCollidersCount = 0;
            for (var i = 0; i < colliders.Length; i++)
            {
                var playerSettings = colliders[i].GetComponent<PlayerSettings>();
                var playerHealth = colliders[i].GetComponent<PlayerHealth>();
                var playerPhysics = colliders[i].GetComponent<PlayerPhysics>();

                if (playerSettings != null && playerHealth != null && playerPhysics != null)
                {
                    playerCollidersCount++;
                    var explosionDirection = (playerSettings.transform.position - transform.position);
                    var distance = explosionDirection.magnitude;
                    var centerDistanceMultiplier = 1f - distance / settings.BlastRadius;
                    var blastDamage = Mathf.Max(1, Mathf.RoundToInt(settings.BlastDamage * centerDistanceMultiplier));
                    var blastDamageMultiplier = settings.BlastDamage / settings.MaxBlastDamageSetting;
                    var resultingSpeed = playerPhysics.Speed * playerPhysics.MovementDirection + explosionDirection.normalized * settings.BlastAcceleration * settings.BlastAccelerationDuration * centerDistanceMultiplier * blastDamageMultiplier;
                    playerPhysics.Speed = resultingSpeed.magnitude;
                    playerPhysics.MovementDirection = resultingSpeed.normalized;

                    if (playerHealth.DoDamage(blastDamage))
                    {
                        AFEventManager.INSTANCE.PostEvent(AFEvents.Kill(settings.PlayerOwnerNumber, playerSettings.playerNumber));
                    }
                }
            }

            AFEventManager.INSTANCE.RemoveListener(this);
            var explosionParticles = Instantiate(explosionParticlesPrefab, transform.position, transform.rotation);
            var particlesMainModule = explosionParticles.main;
            particlesMainModule.startColor = settings.color;
            Destroy(gameObject);
        }

        private void TriggerExplosionDueToLifeTime()
        {
            if (AlreadyTriggered) return;

            AFEventManager.INSTANCE.PostEvent(AFEvents.BombTrigger(settings.PlayerOwnerNumber, false));
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
            } else if (afEvent.type == AFEventType.KILLED)
            {
                var payload = (KilledEventPayload)afEvent.payload;
                if (payload.playerNumberOfKilledPlayer == settings.PlayerOwnerNumber)
                {
                    TriggerExplosionDueToLifeTime();
                }
            }
            return false;
        }
    }
}
