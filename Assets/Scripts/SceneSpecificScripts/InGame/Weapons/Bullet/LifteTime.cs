using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof(BulletSettings))]
    public class LifteTime : MonoBehaviour
    {
        public float timeToLive = 2f;

        private BulletSettings settings;

        // Start is called before the first frame update
        void Start()
        {
            settings = GetComponent<BulletSettings>();
            timeToLive = settings.TimeToLive;
            Invoke("DestroyBullet", timeToLive);
        }

        void DestroyBullet()
        {
            var particles = Instantiate(settings.hitParticlesPrefab, transform.position, transform.rotation);
            var mainModule = particles.main;
            mainModule.startColor = settings.color;
            Destroy(gameObject);
        }

    }
}
