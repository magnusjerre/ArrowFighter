using UnityEngine;

namespace Jerre.Utils
{
    public class CameraHelper 
    {
        public static Bounds GetCameraWorldCoordinateBounds()
        {
            var aspectRatio = 1f * Display.main.renderingWidth / Display.main.renderingHeight;
            var halfHeight = Camera.main.orthographicSize;
            var halfWidth = halfHeight * aspectRatio;

            return new Bounds(Vector3.zero, new Vector3(2 * halfWidth, 0, 2 * halfHeight));
        }

        public static void PaintBoundsOnScreen(Bounds bounds, RectTransform rect)
        {
            var worldBounds = GetCameraWorldCoordinateBounds();

            var screenHeight = bounds.size.z / worldBounds.size.z * Display.main.renderingHeight;
            var screenWidth = bounds.size.x / worldBounds.size.x * Display.main.renderingWidth;
            var positionX = bounds.center.x / worldBounds.size.x * Display.main.renderingWidth;
            var positionY = bounds.center.z / worldBounds.size.z * Display.main.renderingHeight;

            //Debug.Log("themesh.width: " + bounds.size.x);
            //Debug.Log("themesh.center: " + bounds.center);

            //Debug.Log("camera info: " + Camera.main.rect);
            //Debug.Log("camera orthosize: " + Camera.main.orthographicSize);
            //Debug.Log("display width: " + Display.main.renderingWidth);
            //Debug.Log("display height: " + Display.main.renderingHeight);

            rect.anchoredPosition = new Vector2(positionX, positionY);
            rect.sizeDelta = new Vector2(screenWidth, screenHeight);
            rect.gameObject.SetActive(true);
        }
    }
}
