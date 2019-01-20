using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerInputComponent))]
    public class PlayerSettings : MonoBehaviour
    {
        public int playerNumber;
        public int MaxHealth;

        public float MaxSpeed = 100f;
        public float MaxAcceleration = 1000f;

        public float MaxLookRotationSpeedDegs = 360 * Mathf.Deg2Rad;
        public float FireRate = 4;

        public Color color;

        // Start is called before the first frame update
        void Start()
        {
            var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            for (var i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].color = color;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
   
    }
}
