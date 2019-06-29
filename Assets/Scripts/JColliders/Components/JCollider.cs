using UnityEngine;

namespace Jerre.JColliders
{
    public class JCollider : MonoBehaviour
    {
        public bool CheckOnlyForOverlap;    // Don't calculate push
        public JLayer jLayer;
        public MeshFilter meshFilter;
        public JMesh meshIdentity;
        public JMesh meshFrame;

        public bool debugMesh = false;
        public bool debugAABB = false;

        public delegate void CollisionHandler(JCollider thisBody, JCollider  otherBody);
        private CollisionHandler handler;

        private void Awake()
        {
            JColliderContainer.INSTANCE.Add(this);
        }

        void Start()
        {
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
                if (meshFilter == null)
                {
                    throw new System.Exception("JCollider needs a reference to a mesh filter");
                }
            }

            meshIdentity = JMesh.CalculateJMesh(meshFilter.mesh.vertices, meshFilter.mesh.triangles);
            meshFrame = meshIdentity;
        }

        public void SetHandler(CollisionHandler handler)
        {
            this.handler = handler;
        }

        public void OnJCollsion(JCollider otherCollider)
        {
            handler?.Invoke(this, otherCollider);
        }

        public void OnDestroy()
        {
            JColliderContainer.INSTANCE.Remove(this);
        }

        public void NotifyDestroyCollider()
        {
            JColliderContainer.INSTANCE.Remove(this);
        }
    }
}
