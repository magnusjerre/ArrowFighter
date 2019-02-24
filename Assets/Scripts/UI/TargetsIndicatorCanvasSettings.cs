using UnityEngine;

namespace Jerre.UI
{
    public class TargetsIndicatorCanvasSettings : MonoBehaviour
    {

        public Color BaseColor;
        public Color HighlightColor;

        public int NumberOfSlicesPerTopBottomEdge = 4;
        public int NumberOfSlicesPerRightLeftEdge = 3;
        public int Thickness = 16;

        public int PlayerNumber;

        public TargetIndicatorAngles IndicatorPrefab;

        void Start()
        {

        }        
    }
}
