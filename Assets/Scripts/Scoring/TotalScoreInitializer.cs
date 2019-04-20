using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.Scoring
{
    public class TotalScoreInitializer : MonoBehaviour
    {
        public Text text;
        
        void Start()
        {
            text.text = GameObject.FindObjectOfType<ScoreManager>().maxScore + "";
        }

    }
}
