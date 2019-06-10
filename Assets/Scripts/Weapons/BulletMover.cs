using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (BulletSettings))]
    public class BulletMover : MonoBehaviour
    {
        BulletSettings settings;
        public float Speed;

        // Start is called before the first frame update
        void Start()
        {
            settings = GetComponent<BulletSettings>();
            Speed = settings.Speed;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * settings.Speed * Time.deltaTime);
        }
    }
}
