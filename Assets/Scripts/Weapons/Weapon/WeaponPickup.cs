using Jerre.Events;
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
        
        void Start()
        {
            jCollider = GetComponent<JCollider>();
            jCollider.SetOnJCollisionEnterHandler(OnEnterHandler);
            jCollider.SetOnJCollisionStayHandler(OnStayHandler);
            Debug.Log("WeaponPickup.Start(). colliderId: " + jCollider.IdGenerated);
        }

        public void OnStayHandler(JCollider thisBody, JCollider otherBody)
        {
            Debug.Log("OnStayHandler for weaponPickup: " + gameObject.name);
        }

        public void OnEnterHandler(JCollider thisBody, JCollider otherBody)
        {
            Debug.Log("OnEnterHandler for WeaponPickup: " + gameObject.name);
            var weaponSlot = otherBody.GetComponent<WeaponSlot>();
            if (weaponSlot == null || !weaponSlot.enabled) return;

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

            var nextWeaponIndex = (index + 1) % WeaponsUpgrades.Length;
            var upgradeColor = GetComponentInChildren<Renderer>().material.color;
            
            // nextWeaponIndex + 1 since WeaponUpgradePath.CurrentUpgradeNumber is 1-indexed and nextWeaponIndex is 0-indexed
            weaponSlot.AttachWeapon(WeaponsUpgrades[nextWeaponIndex], new WeaponUpgradePath(upgradeColor, WeaponsUpgrades.Length, nextWeaponIndex + 1)); 

            thisBody.NotifyDestroyCollider();
            Destroy(gameObject);
        }

        public WeaponUpgradePath weaponUpgradePathInitial()
        {
            return new WeaponUpgradePath(
                GetComponentInChildren<Renderer>().material.color,
                WeaponsUpgrades.Length,
                1
            );
        }
    }
}
