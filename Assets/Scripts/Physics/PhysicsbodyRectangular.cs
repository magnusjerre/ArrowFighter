using UnityEngine;

namespace Jerre.JPhysics
{
    public class PhysicsbodyRectangular : MonoBehaviour
    {
        public Renderer MeshRenderer;
        public bool IsStationary = false;
        public float SurfaceBounceFactor = 1f;

        void Start()
        {
            if (MeshRenderer == null)
            {
                MeshRenderer = GetComponent<Renderer>();
            }
        }
        
    }
}
