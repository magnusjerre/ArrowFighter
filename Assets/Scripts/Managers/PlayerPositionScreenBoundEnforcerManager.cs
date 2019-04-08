using System.Collections.Generic;
using UnityEngine;

namespace Jerre
{
    public class PlayerPositionScreenBoundEnforcerManager : MonoBehaviour
    {
        public Rect screenBoundsInWorldSize;

        Dictionary<int, PlayerSettings> allPlayers;


        private void Awake()
        {
            allPlayers = new Dictionary<int, PlayerSettings>();
        }

        void Start()
        {
            var aspectRatio = 1f * Display.main.renderingWidth/ Display.main.renderingHeight;
            var halfHeight = Camera.main.orthographicSize;
            var halfWidth = halfHeight * aspectRatio;
            screenBoundsInWorldSize = new Rect(-halfWidth, -halfHeight, halfWidth * 2, halfHeight * 2);
        }
        
        void LateUpdate()
        {
            var allPlayerSettings = GameObject.FindObjectsOfType<PlayerSettings>();
            for (var i = 0; i < allPlayerSettings.Length; i++)
            {
                var player = allPlayerSettings[i];
                var playerPhysics = player.GetComponent<PlayerPhysics>();
                var adjustedX = player.transform.position.x;
                var adjustedY = player.transform.position.z;
                var adjustedSpeed = playerPhysics.Speed;
                var adjustedMovementDirection = playerPhysics.MovementDirection;
                var adjusted = false;

                if (adjustedX > screenBoundsInWorldSize.xMax)
                {
                    adjusted = true;
                    adjustedX = screenBoundsInWorldSize.xMax;
                    adjustedSpeed = Mathf.Sqrt(playerPhysics.Speed * playerPhysics.Speed - playerPhysics.MovementDirection.x * playerPhysics.MovementDirection.x);
                    adjustedMovementDirection = new Vector3(0, playerPhysics.MovementDirection.y, playerPhysics.MovementDirection.z);
                }
                else if (adjustedX < screenBoundsInWorldSize.xMin)
                {
                    adjusted = true;
                    adjustedX = screenBoundsInWorldSize.xMin;
                    adjustedSpeed = Mathf.Sqrt(playerPhysics.Speed * playerPhysics.Speed - playerPhysics.MovementDirection.x * playerPhysics.MovementDirection.x);
                    adjustedMovementDirection = new Vector3(0, playerPhysics.MovementDirection.y, playerPhysics.MovementDirection.z);
                }

                if (adjustedY > screenBoundsInWorldSize.yMax)
                {
                    adjusted = true;
                    adjustedY = screenBoundsInWorldSize.yMax;
                    adjustedSpeed = Mathf.Sqrt(playerPhysics.Speed * playerPhysics.Speed - playerPhysics.MovementDirection.z * playerPhysics.MovementDirection.z);
                    adjustedMovementDirection = new Vector3(playerPhysics.MovementDirection.x, playerPhysics.MovementDirection.y, 0);
                }
                else if (adjustedY < screenBoundsInWorldSize.yMin)
                {
                    adjusted = true;
                    adjustedY = screenBoundsInWorldSize.yMin;
                    adjustedSpeed = Mathf.Sqrt(playerPhysics.Speed * playerPhysics.Speed - playerPhysics.MovementDirection.z * playerPhysics.MovementDirection.z);
                    adjustedMovementDirection = new Vector3(playerPhysics.MovementDirection.x, playerPhysics.MovementDirection.y, 0);
                }

                if (adjusted)
                {
                    if (!float.IsNaN(adjustedSpeed))
                    {
                        playerPhysics.Speed = adjustedSpeed;
                    }

                    playerPhysics.MovementDirection = adjustedMovementDirection;
                    player.transform.position = new Vector3(adjustedX, player.transform.position.y, adjustedY);
                }
            }
        }
    }
}
