using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerSettings)), RequireComponent(typeof(PlayerInputComponent))]
    public class PlayerWeapon : MonoBehaviour, UsePlayerInput
    {
        public Transform Muzzle;
        public BulletSettings bulletPrefab;

        private PlayerSettings settings;
        private PlayerInputComponent playerInput;
        private float minTimeBetweenFire;
        private float timeSinceLastFire;

        private bool UsePlayerInput = true;

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
            var tryToFire = UsePlayerInput ? playerInput.input.Fire : false;
            timeSinceLastFire += Time.deltaTime;

            if (tryToFire && timeSinceLastFire >= minTimeBetweenFire)
            {
                var bullet = Instantiate(bulletPrefab, Muzzle.position, Muzzle.rotation);
                bullet.PlayerOwnerNumber = settings.playerNumber;
                bullet.color = settings.color;
                timeSinceLastFire = 0f;
            }
        }

        public void SetUsePlayerInput(bool usePlayerInput)
        {
            this.UsePlayerInput = usePlayerInput;
        }
    }
}
