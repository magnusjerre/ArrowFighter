using Jerre.UI.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.UIStuff
{
    [RequireComponent(typeof (ScaleFrom))]
    public class TextChangeAnimator : MonoBehaviour
    {
        private string theText;
        private Text text;
        private ScaleFrom scaleFrom;
        
        void Awake() 
        {
            scaleFrom = GetComponent<ScaleFrom>();
            text = GetComponent<Text>();
        }

        void Start() 
        {
            theText = text.text;
        }
        
        void Update()
        {
            if (theText != text.text) {
                theText = text.text;
                scaleFrom.DoScale();
            }
        }
    }
}
