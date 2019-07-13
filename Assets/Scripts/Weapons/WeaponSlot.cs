using UnityEngine;

namespace Jerre.Weapons
{
    [RequireComponent(typeof(PlayerSettings)), RequireComponent(typeof(PlayerInputComponent))]
    public class WeaponSlot : MonoBehaviour, UsePlayerInput
    {
        public Weapon defaultWeaponPrefab;

        private Weapon activeWeaponInstance;
        private PlayerSettings settings;
        private PlayerInputComponent playerInput;

        public bool resetToDefaultWhenSpent;

        private bool UsePlayerInput = true;
        public void SetUsePlayerInput(bool UsePlayerInput)
        {
            this.UsePlayerInput = UsePlayerInput;
        }

        void Awake()
        {
            settings = GetComponent<PlayerSettings>();
            playerInput = GetComponent<PlayerInputComponent>();
        }

        void Start()
        {
            AttachWeapon(defaultWeaponPrefab);
        }

        void Update()
        {
            var tryToFire = UsePlayerInput ? playerInput.input.Fire : false;
            if (tryToFire)
            {
                activeWeaponInstance.Fire();
            }
        }

        void AttachWeapon(Weapon weaponPrefab)
        {
            var weaponInstance = Instantiate(weaponPrefab, transform);
            weaponInstance.bulletColor = settings.color;
            weaponInstance.PlayerNumber = settings.playerNumber;

            Destroy(activeWeaponInstance);
            activeWeaponInstance = weaponInstance;
        }

    }
}
