using Jerre.Events;
using UnityEngine;

namespace Jerre.Weapons
{
    [RequireComponent(typeof(PlayerSettings)), RequireComponent(typeof(PlayerInputComponent))]
    public class WeaponSlot : MonoBehaviour, UsePlayerInput
    {
        public Weapon defaultWeaponPrefab;
        public WeaponPickup DefaultWeaponPickup;
        private WeaponUpgradePath DefaultWeaponUpgradePath;

        private Weapon activeWeaponInstance;
        public string ActiveWeaponName
        {
            get { return activeWeaponInstance?.WeaponName; }
        }
        private WeaponUpgradePath ActiveWeaponUpgradePath;

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
            var defaultPickupInstance = Instantiate(DefaultWeaponPickup);
            DefaultWeaponUpgradePath = defaultPickupInstance.weaponUpgradePathInitial();
            Destroy(defaultPickupInstance.gameObject);
            AttachWeapon(defaultWeaponPrefab, DefaultWeaponUpgradePath);
        }

        void Update()
        {
            var tryToFire = UsePlayerInput ? playerInput.input.Fire : false;
            if (tryToFire)
            {
                if (activeWeaponInstance.Fire() && activeWeaponInstance.IsSpent())
                {
                    AttachWeapon(defaultWeaponPrefab, DefaultWeaponUpgradePath);
                }
            }
        }

        public void AttachWeapon(Weapon weaponPrefab, WeaponUpgradePath upgradePath)
        {
            var weaponInstance = Instantiate(weaponPrefab, transform);
            weaponInstance.bulletColor = settings.color;
            weaponInstance.PlayerNumber = settings.playerNumber;

            if(activeWeaponInstance != null)
            {
                Destroy(activeWeaponInstance.gameObject);
            }
            activeWeaponInstance = weaponInstance;
            ActiveWeaponUpgradePath = upgradePath;

            AFEventManager.INSTANCE.PostEvent(AFEvents.WeaponUpgrade(
                    settings.playerNumber,
                    upgradePath.UpgradeProgress,
                    upgradePath.UpgradeColor
                ));
        }

    }
}
