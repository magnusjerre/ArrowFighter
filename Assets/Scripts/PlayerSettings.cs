using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerInputComponent))]
    public class PlayerSettings : MonoBehaviour
    {
        public int playerNumber;

        public float MaxSpeed = 100f;
        public float MaxAcceleration = 1000f;

        public float MaxLookRotationSpeedDegs = 360 * Mathf.Deg2Rad;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
   
    }
}
