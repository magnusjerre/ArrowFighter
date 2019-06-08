using UnityEngine;
using UnityEngine.UI;

namespace Jerre.Scoring
{
    public class TotalScoreInitializer : MonoBehaviour
    {
        public Text text;

        void Awake()
        {
            text.text = null;
        }
        
        void Start()
        {
            
        }

        private void Update()
        {
            if (text.text == null)
            {
                var scoreManager = GameObject.FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    text.text = scoreManager.maxScore + "";
                }
            }
        }

    }
}
