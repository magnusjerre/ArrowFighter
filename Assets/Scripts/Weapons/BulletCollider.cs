using Jerre.Events;
using Jerre.JColliders;
using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (BulletSettings)), RequireComponent(typeof (Rigidbody))]
    public class BulletCollider : MonoBehaviour
    {
        BulletSettings settings;
        JCollider collision;

        // Start is called before the first frame update
        void Start()
        {
            settings = GetComponent<BulletSettings>();
            collision = GetComponent<JCollider>();
            collision.SetOnJCollisionEnterHandler(CollisionHandler);
        }

        private void CollisionHandler(JCollider thisBody, JCollider otherBody)
        {
            var playerHealth = otherBody.GetComponent<PlayerHealth>();
            var otherColor = settings.color;
            collision.NotifyDestroyCollider();
            if (playerHealth != null)
            {
                var playerSettings = otherBody.GetComponent<PlayerSettings>();
                otherColor = playerSettings.color;
                Debug.Log("Player " + settings.PlayerOwnerNumber + " hit player " + playerSettings.playerNumber + "! Doing damage: " + settings.Damage);
                if (playerHealth.DoDamage(settings.Damage))
                {
                    AFEventManager.INSTANCE.PostEvent(AFEvents.Kill(settings.PlayerOwnerNumber, playerSettings.playerNumber));
                }
            }
            else
            {
                Debug.Log("Bullet hit something else..");
            }

            if (playerHealth != null || settings.DestroyOnAnyOverlap)
            {
                var hitParticles = Instantiate(settings.hitParticlesPrefab, transform.position, transform.rotation);
                var colorModule = hitParticles.colorOverLifetime;
                colorModule.enabled = true;
                ParticleSystem.MinMaxGradient gradient = new ParticleSystem.MinMaxGradient(settings.color, otherColor);
                var mainModule = hitParticles.main;
                mainModule.startColor = gradient;
                Destroy(gameObject);
            }
        }
    }
}
