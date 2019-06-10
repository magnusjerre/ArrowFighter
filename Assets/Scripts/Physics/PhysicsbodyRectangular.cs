using UnityEngine;

namespace Jerre.JPhysics
{
    public class PhysicsbodyRectangular : MonoBehaviour
    {
        public Renderer MeshRenderer;
        public bool IsStationary = false;
        public float SurfaceBounceFactor = 1f;

        public JLayer jLayer = JLayer.SCENERY;

        void Start()
        {
            if (MeshRenderer == null)
            {
                MeshRenderer = GetComponent<Renderer>();
            }
        }
        
    }
}
