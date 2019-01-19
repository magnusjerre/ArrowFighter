using UnityEngine;

namespace Jerre
{
    public class LifteTime : MonoBehaviour
    {
        public float timeToLive = 2f;

        // Start is called before the first frame update
        void Start()
        {
            Invoke("DestroyBullet", timeToLive);
        }

        void DestroyBullet()
        {
            Destroy(gameObject);
        }

    }
}
