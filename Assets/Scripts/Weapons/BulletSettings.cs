using UnityEngine;

namespace Jerre
{
    public class BulletSettings : MonoBehaviour
    {
        public float Speed = 200f;
        public float TimeToLive = 2f;
        public int Damage = 1;
        public int PlayerOwnerNumber;
        public Color color;

        public ParticleSystem hitParticlesPrefab;

        void Awake()
        {
            Damage = PlayersState.INSTANCE.gameSettings.fireDamage;
        }
        // Start is called before the first frame update
        void Start()
        {
            GetComponentInChildren<SpriteRenderer>().color = color;
        }
    }
}
