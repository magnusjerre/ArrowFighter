using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class ColorManager : MonoBehaviour
    {
        [SerializeField]
        private Color[] PossiblePlayerColors;

        private List<Color> availableColors;

        private void Awake()
        {
            availableColors = new List<Color>(PossiblePlayerColors);
        }

        void Start()
        {

        }

        public Color SwapForNextColor(Color currentColor)
        {
            if (!availableColors.Contains(currentColor) && availableColors.Count > 0)
            {
                var swapColor = availableColors[0];
                availableColors.RemoveAt(0);
                availableColors.Add(currentColor);
                return swapColor;
            }
            return currentColor;
        }

        public Color ExtractNextColor()
        {
            var newColor = availableColors[0];
            availableColors.RemoveAt(0);
            return newColor;
        }

        public void ReturnColor(Color color)
        {
            if (!availableColors.Contains(color))
            {
                availableColors.Add(color);
            }
        }
    }
}
