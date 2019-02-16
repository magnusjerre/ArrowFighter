using UnityEngine;
using System;

namespace Jerre
{
    [RequireComponent(typeof (Camera)), RequireComponent(typeof(CameraSettings))]
    public class CameraSize : MonoBehaviour
    {
        private CameraSettings settings;
        private Camera camera;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            camera.orthographic = true;
            //camera.orthographicSize = 350;

            settings = GetComponent<CameraSettings>();
        }

        // Use this for initialization
        void Start()
        {
            UpdateCameraSize();
        }

        public void UpdateCameraSize()
        {
            if (settings.TotalNumberOfCameras == 1)
            {
                camera.rect = GetRectForPlayerWhenOnlyOnePlayer();
            }
            else if (settings.TotalNumberOfCameras == 2)
            {
                camera.rect = GetRectForPlayerNumberWhenTwoPlayers(settings.CameraNumber);
            }
            else if (settings.TotalNumberOfCameras == 3 || settings.TotalNumberOfCameras == 4)
            {
                camera.rect = GetRectForPlayerNumberWhenThreeOrFourPlayers(settings.CameraNumber);
            }
            else
            {
                throw new NotImplementedException("Something other than 2 players are playing, not currently supported.");
            }
        }

        public Rect GetRectForPlayerWhenOnlyOnePlayer()
        {
            return new Rect(0, 0, 1, 1);
        }

        public Rect GetRectForPlayerNumberWhenTwoPlayers(int cameraNumber)
        {
            var y = cameraNumber == 1 ? 0.5f : 0f;
            return new Rect(0, y, 1, 0.5f);
        }

        public Rect GetRectForPlayerNumberWhenThreeOrFourPlayers(int cameraNumber)
        {
            var x = cameraNumber == 1 || cameraNumber == 3 ? 0f : 0.5f;
            var y = cameraNumber == 1 || cameraNumber == 2 ? 0.5f : 0f;
            return new Rect(x, y, 0.5f, 0.5f);
        }

    }
}
