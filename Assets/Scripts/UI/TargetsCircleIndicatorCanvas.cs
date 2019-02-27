using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.UI
{
    [RequireComponent(typeof(TargetsCircleIndicatorCanvasSettings)), RequireComponent(typeof(Canvas))]
    public class TargetsCircleIndicatorCanvas : MonoBehaviour
    {
        private Canvas canvas;
        private TargetsCircleIndicatorCanvasSettings settings;

        public Camera camera;
        private PlayerSettings[] players;

        private List<TargetCircleIndicator> indicators;

        private int OldNumberOfSectors;

        private void Awake()
        {
            settings = GetComponent<TargetsCircleIndicatorCanvasSettings>();
            canvas = GetComponent<Canvas>();

            indicators = new List<TargetCircleIndicator>();
        }

        void Start()
        {
            players = GameObject.FindObjectsOfType<PlayerSettings>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;

            OldNumberOfSectors = settings.NumberOfSectors;

            SetupIndicators();
        }

        void Update()
        {
            if (OldNumberOfSectors != settings.NumberOfSectors)
            {
                OldNumberOfSectors = settings.NumberOfSectors;
                ClearIndicators();
                SetupIndicators();
            }

            ResetColors(indicators);
            SetHighlightColors();
        }

        private void SetupIndicators()
        {
            var sectorSizeInDegs = (1f / settings.NumberOfSectors) * 360f;
            var currentStartAngle = 0f;

            for (var i = 0; i < settings.NumberOfSectors; i++)
            {
                var instance = Instantiate(settings.CircleIndicatorPrefab, canvas.transform);
                instance.SetColor(settings.BaseColor);
                instance.MinAngle = currentStartAngle;
                instance.MaxAngle = currentStartAngle + sectorSizeInDegs;
                instance.Angle = sectorSizeInDegs;
                currentStartAngle = instance.MaxAngle;

                indicators.Add(instance);
            }
        }

        private void ClearIndicators()
        {
            for (var i = 0; i < indicators.Count; i++)
            {
                Destroy(indicators[i].gameObject);
            }

            indicators.Clear();
        }

        private void ResetColors(List<TargetCircleIndicator> indicators)
        {
            for (var i = 0; i < indicators.Count; i++)
            {
                indicators[i].SetColor(settings.BaseColor);
            }
        }

        private void HighlightIfInside(float angle, List<TargetCircleIndicator> indicators)
        {
            for (var i = 0; i < indicators.Count; i++)
            {
                if (indicators[i].IsWithinBounds(angle))
                {
                    indicators[i].SetColor(settings.HighlightColor);
                }
            }
        }

        private void SetHighlightColors()
        {
            players = GameObject.FindObjectsOfType<PlayerSettings>();
            var pos = camera.transform.position;
            var pos2D = new Vector2(pos.x, pos.z);
            
            for (var i = 0; i < players.Length; i++)
            {
                if (players[i].playerNumber == settings.PlayerNumber) continue;
                if (players[i].GetComponent<PlayerHealth>().HealthLeft < 1) continue;

                var playerPos3D = players[i].transform.position;
                var playerPos2D = new Vector2(playerPos3D.x, playerPos3D.z);
                var angle = -Vector2.SignedAngle(Vector2.up, playerPos2D - pos2D);

                HighlightIfInside(angle, indicators);
            }
        }
    }
}
