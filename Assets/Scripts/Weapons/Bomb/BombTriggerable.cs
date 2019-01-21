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

                if (playerSettings != null && playerHealth != null)
                {
                    var distance = (playerSettings.transform.position - transform.position).magnitude;
                    var multiplier = 1f - distance / settings.BlastRadius;
                    var blastDamage = Mathf.Max(1, Mathf.RoundToInt(settings.BlastDamage * multiplier));
                    if (playerHealth.DoDamage(blastDamage))
                    {
                        eventManager.PostEvent(AFEvents.Kill(settings.PlayerOwnerNumber, playerSettings.playerNumber));
                    }
                }
            }

            eventManager.RemoveListener(this);
            Instantiate(explosionParticlesPrefab, transform.position, transform.rotation);
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
