using UnityEngine;

namespace Jerre.Weapons
{
    public class Weapon : MonoBehaviour, IWeapon
    {
        public string WeaponName;
        public Transform[] Muzzles;
        public float FireRate = 4;  // Rounds per second
        public int PlayerNumber;
        public bool InfiniteBullets;
        public int MaxRounds = 5;

        public BulletSettings bulletPrefab;
        public Color bulletColor;

        private float timeSinceLastFire;
        private int RoundsLeft;

        void Start()
        {
            timeSinceLastFire = 1f / FireRate - 0.5f;   // Adding a half-second delay for firing the first round
            RoundsLeft = MaxRounds;
        }

        public bool Fire()
        {
            if ((timeSinceLastFire < 1f / FireRate) || (RoundsLeft == 0 && !InfiniteBullets))
            {
                return false;
            }

            for (var i = 0; i < Muzzles.Length; i++)
            {
                var muzzle = Muzzles[i];
                var bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
                bullet.color = bulletColor;
                bullet.PlayerOwnerNumber = PlayerNumber;
                timeSinceLastFire = 0f;
            }
            

            if (InfiniteBullets)
            {
                RoundsLeft = MaxRounds; 
            } else
            {
                RoundsLeft--;
            }
            return true;
        }

        public bool IsSpent()
        {
            if (InfiniteBullets) return false;
            return RoundsLeft == 0;
        }

        public bool Refill(int rounds)
        {
            RoundsLeft = Mathf.Min(MaxRounds, rounds);
            return true;
        }

        void Update()
        {
            timeSinceLastFire += Time.deltaTime;
        }
    }
}
