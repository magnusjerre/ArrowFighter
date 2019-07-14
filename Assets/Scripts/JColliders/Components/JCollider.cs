using UnityEngine;

namespace Jerre.JColliders
{
    public class JCollider : MonoBehaviour
    {
        public bool IsTrigger;
        public bool IsStationary;    // Don't calculate push
        public JLayer jLayer;
        public MeshFilter meshFilter;
        public JMesh meshIdentity;
        public JMesh meshFrame;
        public ulong IdGenerated;

        public bool debugMesh = false;
        public bool debugAABB = false;

        public delegate void CollisionHandler(JCollider thisBody, JCollider  otherBody);
        private CollisionHandler onStayHandler;
        private CollisionHandler onEnterHandler;
        private CollisionHandler onExitHandler;

        private void Awake()
        {
            JColliderContainer.INSTANCE.Add(this);
            IdGenerated = JColliderContainer.INSTANCE.NextId();

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

        public void SetOnJCollisionStayHandler(CollisionHandler handler)
        {
            this.onStayHandler = handler;
        }

        public void SetOnJCollisionEnterHandler(CollisionHandler handler)
        {
            this.onEnterHandler = handler;
        }

        public void SetOnJCollisionExitHandler(CollisionHandler handler)
        {
            this.onExitHandler = handler;
        }

        public void OnJCollsionStay(JCollider otherCollider)
        {
            onStayHandler?.Invoke(this, otherCollider);
        }

        public void OnJCollisionEnter(JCollider otherCollider)
        {
            onEnterHandler?.Invoke(this, otherCollider);
        }

        public void OnJCollsionExit(JCollider otherCollider)
        {
            onExitHandler?.Invoke(this, otherCollider);
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
