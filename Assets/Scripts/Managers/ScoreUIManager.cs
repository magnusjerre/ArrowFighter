using UnityEngine;
using Jerre.UI;

namespace Jerre
{
    public class ScoreUIManager : MonoBehaviour
    {
        public ScoreUICanvas ScoreUICanvasPrefab;
        private ScoreUICanvas singletonCanvas;
        private ScoreManager scoreManager;

        private void Awake()
        {
            singletonCanvas = Instantiate(ScoreUICanvasPrefab);

            scoreManager = GameObject.FindObjectOfType<ScoreManager>();
            singletonCanvas.ScoreManager = scoreManager;
        }

        void Start()
        {
            
        }
    }
}
