using UnityEngine;
using UnityEngine.UI;

namespace Jerre.UI
{
    [RequireComponent(typeof (RectTransform))]
    public class UIElementContainer : MonoBehaviour
    {
        private RectTransform rectTransform;
        public RectTransform Left;
        public RectTransform Right;
        public Image backgroundImage;
        public float backgroundAlpha = 0.25f;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void Start()
        {
        }

        public void SetSize(float xMin, float xMax)
        {
            rectTransform.anchorMin = new Vector2(xMin, 0);
            rectTransform.anchorMax = new Vector2(xMax, 1);
        }

        public void SetBackgroundColor(Color color)
        {
            backgroundImage.color = new Color(color.r, color.g, color.b, backgroundAlpha); ;
        }
    }
}
