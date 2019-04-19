using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.UIStuff
{
    public class OffsetMaintainer : MonoBehaviour
    {
        public RectTransform TargetToMaintainOffsetTo;

        private Vector3 offset;
        private RectTransform rectTransform;
        
        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            offset = rectTransform.position - TargetToMaintainOffsetTo.position;
        }

        void LateUpdate()
        {
            rectTransform.position = TargetToMaintainOffsetTo.position + offset;
        }
    }
}
