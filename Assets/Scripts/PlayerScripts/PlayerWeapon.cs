using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerSettings)), RequireComponent(typeof(PlayerInputComponent))]
    public class PlayerWeapon : MonoBehaviour
    {
        public Transform Muzzle;
        public BulletSettings bulletPrefab;

        private PlayerSettings settings;
        private PlayerInputComponent playerInput;
        private float minTimeBetweenFire;
        private float timeSinceLastFire;

        // Use this for initialization
        void Start()
        {
            settings = GetComponent<PlayerSettings>();
            minTimeBetweenFire = 1f / settings.FireRate;

            playerInput = GetComponent<PlayerInputComponent>();

        }

        // Update is called once per frame
        void Update()
        {
            var input = playerInput.input;
            timeSinceLastFire += Time.deltaTime;

            if (input.Fire && timeSinceLastFire >= minTimeBetweenFire)
            {
                var bullet = Instantiate(bulletPrefab, Muzzle.position, Muzzle.rotation);
                bullet.PlayerOwnerNumber = settings.playerNumber;
                timeSinceLastFire = 0f;
            }
        }
    }
}
