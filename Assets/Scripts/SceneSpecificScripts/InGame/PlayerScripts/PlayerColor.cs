using UnityEngine;

namespace Jerre
{
    public class PlayerColor : MonoBehaviour
    {
        
        void Start()
        {
            var playerSettings = GetComponent<PlayerSettings>();
            if (playerSettings == null)
            {
                playerSettings = GetComponentInParent<PlayerSettings>();
            }

            

            var renderer = GetComponent<Renderer>();
            if (renderer == null)
            {
                renderer = GetComponentInParent<Renderer>();
            }

            renderer.material.color = playerSettings.color;
        }
    }
}
