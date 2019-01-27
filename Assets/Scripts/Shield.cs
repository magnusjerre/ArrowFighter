using UnityEngine;

namespace Jerre
{
    public class Shield : MonoBehaviour
    {
        public Color BackgroundColor;
        public SpriteRenderer BackgroundSprite;
        public SpriteRenderer ForegroundSprite;

        private PlayerSettings settings;
        private PlayerHealth health;

        private SpriteMask mask;

        void Start()
        {
            mask = GetComponentInChildren<SpriteMask>();
            health = GetComponentInParent<PlayerHealth>();
            settings = health.GetComponent<PlayerSettings>();


            mask.backSortingOrder = settings.playerNumber * 100;
            mask.frontSortingOrder = settings.playerNumber * 100 + 1;

            BackgroundSprite.sortingOrder = mask.backSortingOrder;
            ForegroundSprite.sortingOrder = mask.frontSortingOrder;

            BackgroundSprite.color = BackgroundColor;
            ForegroundSprite.color = settings.color;
        }

        private void Update()
        {
            BackgroundSprite.color = BackgroundColor;
            ForegroundSprite.color = settings.color;
            if (health.MaxHealth > 0)
            {
                mask.alphaCutoff = 1f - (1f * health.HealthLeft / health.MaxHealth);
            }
        }
    }
}
