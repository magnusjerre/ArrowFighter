using UnityEngine;

namespace Jerre
{
    public class BombSettings : MonoBehaviour
    {
        public int BlastDamage = 2;
        public float BlastSpeed = 1000f;
        public float BlastRadius = 10f;
        public float MaxLifeTimeWithoutExploding = 10f;
        public int PlayerOwnerNumber;

        void Start()
        {

        }
    }
}
