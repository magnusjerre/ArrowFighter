using UnityEngine;

namespace Jerre
{
    public class BombSettings : MonoBehaviour
    {
        public int BlastDamage = 2;
        public float BlastAcceleration = 1000f;
        public float BlastRadius = 10f;
        public float MaxLifeTimeWithoutExploding = 10f;
        public int PlayerOwnerNumber;
        public Color color;
        public SpriteRenderer explosionRadiusRenderer;

        void Awake()
        {
            var gameSettings = PlayersState.INSTANCE.gameSettings;
            BlastDamage = gameSettings.bombDamage;
            BlastRadius = gameSettings.bombExplosionRadius;
            MaxLifeTimeWithoutExploding = gameSettings.bombMaxLifeTime;
        }

        void Start()
        {
            var children = GetComponentsInChildren<SpriteRenderer>();
            for (var i = 0; i < children.Length; i++)
            {
                children[i].color = color;
            }
            explosionRadiusRenderer.transform.localScale = Vector3.one * (BlastRadius / 10f);
        }
    }
}
