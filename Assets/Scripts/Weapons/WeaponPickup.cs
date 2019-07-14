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
