using UnityEngine;

namespace Jerre
{
    public class Rotator : MonoBehaviour
    {
        public float DegreesPerSecond = 10f;
        public Vector3 RotationAxis = Vector3.forward;

        void Update()
        {
            transform.Rotate(RotationAxis, DegreesPerSecond * Time.unscaledDeltaTime);
        }
    }
}
