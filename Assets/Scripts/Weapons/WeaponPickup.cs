using Jerre.JColliders;
using UnityEngine;

namespace Jerre.Weapons
{
    [RequireComponent(typeof (JCollider))]
    public class WeaponPickup : MonoBehaviour
    {
        public string[] WeaponsUpgradesNames;
        public Weapon[] WeaponsUpgrades;


        private JCollider jCollider;

        private void Awake()
        {
            jCollider = GetComponent<JCollider>();
            jCollider.SetOnJCollisionEnterHandler(OnEnterHandler);
        }

        
        void Start()
        {

        }

        public void OnEnterHandler(JCollider thisBody, JCollider otherBody)
        {
            var weaponSlot = otherBody.GetComponent<WeaponSlot>();
            if (weaponSlot == null) return;

            string name = weaponSlot.ActiveWeaponName;
            var index = -1;
            for (var i = 0; i < WeaponsUpgradesNames.Length; i++)
            {
                if (WeaponsUpgradesNames[i].Equals(weaponSlot.ActiveWeaponName))
                {
                    index = i;
                    break;
                }
            }


            if (index == -1)
            {
                // The current weapon isn't part of this weapons-upgrade-tree. Therefore choosing the first weapon
                weaponSlot.AttachWeapon(WeaponsUpgrades[0]);
            } else
            {
                weaponSlot.AttachWeapon(WeaponsUpgrades[(index + 1) % WeaponsUpgrades.Length]);
            }

            thisBody.NotifyDestroyCollider();
            Destroy(gameObject);
        }
    }
}
