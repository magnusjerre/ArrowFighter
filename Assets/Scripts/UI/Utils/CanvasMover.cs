using Jerre.Utils;
using UnityEngine;

namespace Jerre.UI.Utils
{
    [RequireComponent(typeof (RectTransform))]
    public class CanvasMover : MonoBehaviour, IDo
    {
        public RectTransform TargetPosition;
        public Vector3 TargetOffset = Vector3.zero;
        public float time;
        private RectTransform rectTransform;
        private Vector3 target;
        private Vector3 initialPosition;
        private float elapsedTime = 0f;

        private bool moving = false;

        // Start is called before the first frame update
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            Debug.Log("rectTransform.position: " + rectTransform.position);
            Debug.Log("rectTranform.localPosition: " + rectTransform.localPosition);
            Debug.Log("rectTransform.anchoredPosition: " + rectTransform.anchoredPosition);
            //localPosition == anchoredPosition, tar hensyn til pivot
            //position er fra venstre nederste hjørne. Er denne som burde brukes

            initialPosition = rectTransform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (moving) {
                elapsedTime += Time.deltaTime;
                rectTransform.position = Vector3.Lerp(initialPosition, target, elapsedTime / time);

                if (elapsedTime >= time) {
                    moving = false;
                }
            }
        }

        public void MoveTo() {
            target = TargetPosition.position + TargetOffset;
            moving = true;
            elapsedTime = 0f;
        }

        public void Do() {
            MoveTo();
        }
    }
}
