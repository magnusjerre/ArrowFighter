using UnityEngine;

namespace Jerre.Events
{
    public struct WeaponUpgradePayload
    {
        public int PlayerNumber;
        public float UpgradeProgress;
        public Color UpgradeColor;

        public WeaponUpgradePayload(int playerNumber, float upgradeProgress, Color upgradeColor)
        {
            PlayerNumber = playerNumber;
            UpgradeProgress = upgradeProgress;
            UpgradeColor = upgradeColor;
        }
    }
}
