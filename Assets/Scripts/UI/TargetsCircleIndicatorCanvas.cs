using Jerre.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.UI
{
    [RequireComponent(typeof(TargetsCircleIndicatorCanvasSettings)), RequireComponent(typeof(Canvas))]
    public class TargetsCircleIndicatorCanvas : MonoBehaviour, IAFEventListener
    {
        private Canvas canvas;
        private TargetsCircleIndicatorCanvasSettings settings;

        public Camera camera;
        private List<PlayerSettings> allPlayersButThis;

        private List<TargetCircleIndicator> indicators;

        private int OldNumberOfSectors;

        private void Awake()
        {
            settings = GetComponent<TargetsCircleIndicatorCanvasSettings>();
            canvas = GetComponent<Canvas>();

            allPlayersButThis = new List<PlayerSettings>();
            indicators = new List<TargetCircleIndicator>();
        }

        void Start()
        {
            AFEventManager.INSTANCE.AddListener(this);

            var players = GameObject.FindObjectsOfType<PlayerSettings>();
            for (var i = 0; i < players.Length; i++)
            {
                if (players[i].playerNumber != settings.PlayerNumber)
                {
                    allPlayersButThis.Add(players[i]);
                }
            }

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
            var pos = camera.transform.position;
            var pos2D = new Vector2(pos.x, pos.z);
            
            for (var i = 0; i < allPlayersButThis.Count; i++)
            {
                if (allPlayersButThis[i].playerNumber == settings.PlayerNumber) continue;
                if (allPlayersButThis[i].GetComponent<PlayerHealth>().HealthLeft < 1) continue;

                var playerPos3D = allPlayersButThis[i].transform.position;
                var playerPos2D = new Vector2(playerPos3D.x, playerPos3D.z);
                var angle = -Vector2.SignedAngle(Vector2.up, playerPos2D - pos2D);

                HighlightIfInside(angle, indicators);
            }
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch (afEvent.type)
            {
                case AFEventType.PLAYER_JOIN:
                    {
                        var payload = (PlayerJoinPayload)afEvent.payload;
                        var players = GameObject.FindObjectsOfType<PlayerSettings>();
                        for (var i = 0; i < players.Length; i++)
                        {
                            if (players[i].playerNumber == payload.playerNumber)
                            {
                                allPlayersButThis.Add(players[i]);
                                return false;
                            }
                        }
                        return false;
                    }
                case AFEventType.PLAYER_LEAVE:
                    {
                        var payload = (PlayerLeavePayload)afEvent.payload;
                        var indexToRemove = -1;
                        for (var i = 0; i < allPlayersButThis.Count; i++)
                        {
                            if (allPlayersButThis[i].playerNumber == payload.playerNumber)
                            {
                                indexToRemove = i;
                                break;
                            }
                        }

                        if (indexToRemove > -1)
                        {
                            allPlayersButThis.RemoveAt(indexToRemove);
                        }
                        return false;
                    }
            }
            return false;
        }

        public void CleanUpBeforeDestroy()
        {
            AFEventManager.INSTANCE.RemoveListener(this);
        }
    }
}
