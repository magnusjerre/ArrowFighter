using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof(BombSettings))]
    public class BombLifetimeIndicator : MonoBehaviour
    {
        public MeshRenderer[] meshRenderers;
        public float BlinksPerSecond = 2f;
        public float MaxBlinksPerSecond = 10f;
        public float TimeLeftBeforeBlinking = 2f;

        private BombSettings settings;
        private Color StartColor, EndColor;

        private bool DoBlink;

        private float currentBlinkDuration, minBlinkDuration;
        private float elapsedBlinkTime;
        private float totalElapsedBlinkingTime;

        void Start()
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
            settings = GetComponent<BombSettings>();
            currentBlinkDuration = 1f / BlinksPerSecond;
            minBlinkDuration = 1f / MaxBlinksPerSecond;
            StartColor = settings.color;
            EndColor = new Color(settings.color.r, settings.color.g, settings.color.b, 0f);

            Invoke("StartBlinking", settings.MaxLifeTimeWithoutExploding - TimeLeftBeforeBlinking);
        }

        void Update()
        {
            if (DoBlink)
            {
                elapsedBlinkTime += Time.deltaTime;
                totalElapsedBlinkingTime += Time.deltaTime;
                if (elapsedBlinkTime >= currentBlinkDuration)
                {
                    elapsedBlinkTime -= currentBlinkDuration;
                    currentBlinkDuration = Mathf.Max(minBlinkDuration, Mathf.Lerp(1f / BlinksPerSecond, 1f / MaxBlinksPerSecond, totalElapsedBlinkingTime / TimeLeftBeforeBlinking));
                    Color temp = StartColor;
                    StartColor = EndColor;
                    EndColor = temp;
                }
                for (var i = 0; i < meshRenderers.Length; i++)
                {
                    meshRenderers[i].material.color = Color.Lerp(StartColor, EndColor, elapsedBlinkTime / currentBlinkDuration);
                }
            }
        }

        void StartBlinking()
        {
            DoBlink = true;
        }
    }
}
