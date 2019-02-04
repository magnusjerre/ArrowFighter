using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    public class ScoreEntry : MonoBehaviour
    {

        public Color PlayerColor;
        public int Position;
        public int Score;

        public bool Debug = false;

        [SerializeField]
        private Transform PlayerContainer;
        [SerializeField]
        private Text PlayerPositionText;
        [SerializeField]
        private Text PlayerScoreText;

        void Start()
        {
            var playerImages = PlayerContainer.GetComponentsInChildren<Image>();
            for (var i = 0; i < playerImages.Length; i++)
            {
                playerImages[i].color = PlayerColor;
            }

            PlayerPositionText.text = "" + Position;
            PlayerPositionText.color = PlayerColor;

            PlayerScoreText.text = "" + Score;
            PlayerScoreText.color = PlayerColor;

            SetDebugView(Debug);
        }

        private void SetDebugView(bool showDebugView)
        {
            GetComponent<Image>().enabled = showDebugView;
            for (var i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).GetComponent<Image>().enabled = showDebugView;
            }
        }

        public void SetHeightAnchoredPosition(float minY, float maxY)
        {
            var rectTransf = GetComponent<RectTransform>();
            rectTransf.anchorMin = new Vector2(rectTransf.anchorMin.x, minY);
            rectTransf.anchorMax = new Vector2(rectTransf.anchorMax.x, maxY);
        }
        
        public float GetHeight()
        {
            var rectTransf = GetComponent<RectTransform>();
            return rectTransf.anchorMax.y - rectTransf.anchorMin.y;
        }
    }
}
