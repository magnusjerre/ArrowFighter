using Jerre.Utils;
using UnityEngine;

namespace Jerre.GC
{
    public class ScaleFrom : MonoBehaviour, IDo
    {
        public Vector3 From = Vector3.one * 2f;
        public Vector3 To = Vector3.one;
        public bool UseInitialScaleAsFrom = false;
        public bool UseInitialScaleAsTo = false;
        public bool ScaleOnStart = false;
        public bool ScaleBackAndForth = false;

        public float time = 0.2f;

        private bool scaling;
        private float elapsedTime = 0f;

        void Start()
        {
            if (UseInitialScaleAsFrom) {
                From = transform.localScale;
            }
            if (UseInitialScaleAsTo) {
                To = transform.localScale;
            }
            if (ScaleOnStart) {
                Do();
            }
        }

        void Update()
        {
            if (scaling) {
                elapsedTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp (From, To, elapsedTime / time);

                if (elapsedTime >= time) {
                    scaling = false;
                    elapsedTime = 0f;
                    transform.localScale = To;
                    if (ScaleBackAndForth)
                    {
                        var originalTo = To;
                        To = From;
                        From = originalTo;
                        DoScale();
                    }
                }
            }
        }

        public void DoScale() {
            scaling = true;
            elapsedTime = 0f;
            transform.localScale = From;
        }

        public void Do() {
            DoScale();
        }
    }
}