using UnityEngine;
using UnityEngine.UI;

namespace Jerre.UI
{
    [RequireComponent(typeof (Image))]
    public class TargetCircleIndicator : MonoBehaviour
    {
        public float MinAngle;
        public float MaxAngle;
        public float Angle;

        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();

            if (MinAngle > 360)
            {
                MinAngle = MinAngle - 360;
            }
            if (MaxAngle > 360)
            {
                MaxAngle = MaxAngle - 360;
            }
        }

        void Start()
        {
            image.fillMethod = Image.FillMethod.Radial360;
            image.fillAmount = Angle / 360f;
            image.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -MinAngle);
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

        public void SetColor(Color color)
        {
            image.color = color;
        }
    }
}
