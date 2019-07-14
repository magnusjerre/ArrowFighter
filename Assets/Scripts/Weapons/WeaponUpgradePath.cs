using UnityEngine;

namespace Jerre.Weapons
{
    public struct WeaponUpgradePath
    {
        public Color UpgradeColor;
        public int TotalNumberOfUpgrades;
        public int CurrentUpgradeNumber;    // 1-indexed

        public WeaponUpgradePath(Color upgradeColor, int totalNumberOfUpgrades, int currentUpgradeNumber)
        {
            UpgradeColor = upgradeColor;
            TotalNumberOfUpgrades = totalNumberOfUpgrades;
            CurrentUpgradeNumber = currentUpgradeNumber;
        }

        public float UpgradeProgress
        {
            get
            {
                return 1f * CurrentUpgradeNumber / TotalNumberOfUpgrades;
            }
        }
    }
}
