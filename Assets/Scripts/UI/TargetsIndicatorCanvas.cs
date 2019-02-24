using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.UI
{
    [RequireComponent(typeof (TargetsIndicatorCanvasSettings)), RequireComponent(typeof (Canvas))]
    public class TargetsIndicatorCanvas : MonoBehaviour
    {
        private TargetsIndicatorCanvasSettings settings;
        private Canvas canvas;

        private int OldNumberOfSlicesPerTopBottomEdge;
        private int OldNumberOfSlicesPerRightLeftEdge;

        private float angleTop, angleRight;

        private List<TargetIndicatorAngles> topIndicators, bottomIndicators, rightIndicators, leftIndicators;

        private PlayerSettings[] players;
        public Camera camera;

        private void Awake()
        {
            settings = GetComponent<TargetsIndicatorCanvasSettings>();
            canvas = GetComponent<Canvas>();

            OldNumberOfSlicesPerRightLeftEdge = settings.NumberOfSlicesPerRightLeftEdge;
            OldNumberOfSlicesPerTopBottomEdge = settings.NumberOfSlicesPerTopBottomEdge;

            topIndicators = new List<TargetIndicatorAngles>();
            bottomIndicators = new List<TargetIndicatorAngles>();
            rightIndicators = new List<TargetIndicatorAngles>();
            leftIndicators = new List<TargetIndicatorAngles>();
        }

        void Start()
        {
            players = GameObject.FindObjectsOfType<PlayerSettings>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = camera;

            var canvasRectTransform = canvas.GetComponent<RectTransform>();
            var halfWidth = canvasRectTransform.rect.width / 2f;
            var halfHeight = canvasRectTransform.rect.height / 2f;
            angleTop = Mathf.Atan(halfWidth / halfHeight) * Mathf.Rad2Deg * 2;
            angleRight = Mathf.Atan(halfHeight / halfWidth) * Mathf.Rad2Deg * 2;

            SetupIndicators();
        }

        void Update()
        {
            if (OldNumberOfSlicesPerRightLeftEdge != settings.NumberOfSlicesPerRightLeftEdge)
            {
                OldNumberOfSlicesPerRightLeftEdge = settings.NumberOfSlicesPerRightLeftEdge;
                ClearIndicators(rightIndicators);
                ClearIndicators(leftIndicators);
                SetupRightSideIndicators();
                SetupLeftSideIndicators();
            }

            if (OldNumberOfSlicesPerTopBottomEdge != settings.NumberOfSlicesPerTopBottomEdge)
            {
                OldNumberOfSlicesPerTopBottomEdge = settings.NumberOfSlicesPerTopBottomEdge;
                ClearIndicators(topIndicators);
                ClearIndicators(bottomIndicators);
                SetupTopIndicators();
                SetupBottomIndicators();
            }

            ResetColors(topIndicators);
            ResetColors(rightIndicators);
            ResetColors(bottomIndicators);
            ResetColors(leftIndicators);

            SetHighlightColors();

        }

        private void SetHighlightColors()
        {
            players = GameObject.FindObjectsOfType<PlayerSettings>();
            var pos = camera.transform.position;
            var pos2D = new Vector2(pos.x, pos.z);
            for (var i = 0; i < players.Length; i++)
            {
                if (players[i].playerNumber == settings.PlayerNumber) continue;

                var playerHealth = players[i].GetComponent<PlayerHealth>();
                if (playerHealth.HealthLeft < 1) continue;

                var playerPos3D = players[i].transform.position;
                var playerPos2D = new Vector2(playerPos3D.x, playerPos3D.z);
                var angle = -Vector2.SignedAngle(Vector2.up, playerPos2D - pos2D);
                Debug.Log("angle: " + angle);

                var highlighted = HighlightIfInside(angle, topIndicators);
                if (!highlighted)
                {
                    highlighted = HighlightIfInside(angle, rightIndicators);
                }
                if (!highlighted)
                {
                    highlighted = HighlightIfInside(angle, bottomIndicators);
                }
                if (!highlighted)
                {
                    highlighted = HighlightIfInside(angle, leftIndicators);
                }
            }
        }

        private bool HighlightIfInside(float angle, List<TargetIndicatorAngles> indicatorAngles)
        {
            for (var i = 0; i < indicatorAngles.Count; i++)
            {
                if (indicatorAngles[i].IsWithinBounds(angle))
                {
                    var image = indicatorAngles[i].GetComponent<Image>();
                    image.color = settings.HighlightColor;
                    return true;
                }
            }
            return false;
        }

        private void ResetColors(List<TargetIndicatorAngles> indicators)
        {
            for (var i = 0; i < indicators.Count; i++)
            {
                var image = indicators[i].GetComponent<Image>();
                image.color = settings.BaseColor;
            }
        }

        private void ClearIndicators(List<TargetIndicatorAngles> indicators)
        {
            for (var i = 0; i < indicators.Count; i++)
            {
                Destroy(indicators[i].gameObject);
            }

            indicators.Clear();
        }

        private void SetupIndicators()
        {
            SetupTopIndicators();
            SetupBottomIndicators();
            SetupLeftSideIndicators();
            SetupRightSideIndicators();
        }

        private void SetupTopIndicators()
        {
            var size = 1f / settings.NumberOfSlicesPerTopBottomEdge;
            var deltaAngle = angleTop / settings.NumberOfSlicesPerTopBottomEdge;
            var currentStartAngle = 360 - (angleTop / 2f);
            for (var i = 0; i < settings.NumberOfSlicesPerTopBottomEdge; i++)
            {
                var instance = Instantiate(settings.IndicatorPrefab, transform);
                var image = instance.GetComponent<Image>();
                var rectTransform = image.rectTransform;
                rectTransform.pivot = new Vector2(0.5f, 1);
                rectTransform.sizeDelta = new Vector2(0, settings.Thickness);
                rectTransform.anchorMin = new Vector2(i * size, 1);
                rectTransform.anchorMax = new Vector2((i + 1) * size, 1);
                image.color = settings.BaseColor;

                instance.MinAngle = currentStartAngle;
                instance.MaxAngle = currentStartAngle + deltaAngle;
                currentStartAngle = instance.MaxAngle;

                topIndicators.Add(instance);
            }
        }

        private void SetupBottomIndicators()
        {
            var size = 1f / settings.NumberOfSlicesPerTopBottomEdge;

            var deltaAngle = angleTop / settings.NumberOfSlicesPerTopBottomEdge;
            var currentStartAngle = (angleTop / 2f) + angleRight + angleTop;
            for (var i = 0; i < settings.NumberOfSlicesPerTopBottomEdge; i++)
            {
                var instance = Instantiate(settings.IndicatorPrefab, transform);
                var image = instance.GetComponent<Image>();
                var rectTransform = image.rectTransform;
                rectTransform.pivot = new Vector2(0.5f, 0);
                rectTransform.sizeDelta = new Vector2(0, settings.Thickness);
                rectTransform.anchorMin = new Vector2(i * size, 0);
                rectTransform.anchorMax = new Vector2((i + 1) * size, 0);
                image.color = settings.BaseColor;

                instance.MaxAngle = currentStartAngle;
                instance.MinAngle = currentStartAngle - deltaAngle;
                currentStartAngle = instance.MinAngle;

                bottomIndicators.Add(instance);
            }
        }

        private void SetupRightSideIndicators()
        {
            var size = 1f / settings.NumberOfSlicesPerRightLeftEdge;

            var deltaAngle = angleRight * size;
            var currentStartAngle = angleTop / 2 + angleRight;

            for (var i = 0; i < settings.NumberOfSlicesPerRightLeftEdge; i++)
            {
                var instance = Instantiate(settings.IndicatorPrefab, transform);
                var image = instance.GetComponent<Image>();
                var rectTransform = image.rectTransform;
                rectTransform.pivot = new Vector2(1, 0);
                rectTransform.sizeDelta = new Vector2(settings.Thickness, 0);
                rectTransform.anchorMin = new Vector2(1, i * size);
                rectTransform.anchorMax = new Vector2(1, (i + 1) * size);
                image.color = settings.BaseColor;

                instance.MaxAngle = currentStartAngle;
                instance.MinAngle = currentStartAngle - deltaAngle;
                currentStartAngle = instance.MinAngle;

                rightIndicators.Add(instance);
            }
        }

        private void SetupLeftSideIndicators()
        {
            var size = 1f / settings.NumberOfSlicesPerRightLeftEdge;

            var deltaAngle = angleRight * size;
            var currentStartAngle = (angleTop / 2) + angleRight + angleTop;

            for (var i = 0; i < settings.NumberOfSlicesPerRightLeftEdge; i++)
            {
                var instance = Instantiate(settings.IndicatorPrefab, transform);
                var image = instance.GetComponent<Image>();
                var rectTransform = image.rectTransform;
                rectTransform.pivot = new Vector2(0, 0);
                rectTransform.sizeDelta = new Vector2(settings.Thickness, 0);
                rectTransform.anchorMin = new Vector2(0, i * size);
                rectTransform.anchorMax = new Vector2(0, (i + 1) * size);
                image.color = settings.BaseColor;

                instance.MinAngle = currentStartAngle;
                instance.MaxAngle = currentStartAngle + deltaAngle;
                currentStartAngle = instance.MaxAngle;

                leftIndicators.Add(instance);
            }
        }
    }
}
