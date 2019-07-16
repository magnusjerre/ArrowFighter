using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    public class RoundMaxScore : MonoBehaviour
    {
        public Text totalScoreText;

        void Start()
        {
            var scoreManager = GameObject.FindObjectOfType<FreeForAllGameModeManager>();
            if (scoreManager != null)
            {
                totalScoreText.text = scoreManager.maxScore + "";
            } else
            {
                totalScoreText.text = "Missing score manager";
            }
        }
    }
}