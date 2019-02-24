using UnityEngine;

namespace Jerre.UI
{
    public class TargetIndicatorAngles : MonoBehaviour
    {
        public float MinAngle;
        public float MaxAngle;

        void Start()
        {

            if (MinAngle > 360)
            {
                MinAngle = MinAngle - 360;
            }
            if (MaxAngle > 360)
            {
                MaxAngle = MaxAngle - 360;
            }
        }

        public bool IsWithinBounds(float angle)
        {
            if (angle < 0)
            {
                angle = 360 + angle;
            }


            if (MinAngle > MaxAngle)
            {
                return (MinAngle <= angle && angle <= 360) || (0 <= angle && angle <= MaxAngle);
            }

            return MinAngle <= angle && angle <= MaxAngle;
        }
    }
}
