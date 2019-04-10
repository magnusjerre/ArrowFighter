using UnityEngine;

namespace Jerre.UI
{
    [RequireComponent(typeof (RectTransform))]
    public class UIElementContainer : MonoBehaviour
    {
        private RectTransform rectTransform;
        public RectTransform Left;
        public RectTransform Right;

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
    }
}
