using UnityEngine;

namespace Jerre.JPhysics
{
    public class PhysicsbodyRectangular : MonoBehaviour
    {
        public Renderer MeshRenderer;
        public MeshFilter meshFilter;
        public Mesh mesh;
        public bool IsStationary = false;
        public float SurfaceBounceFactor = 1f;
        public JMeshFrameInstance jMeshFrameInstance;
        public JMesh jMeshIdentity;

        public JLayer jLayer = JLayer.SCENERY;

        public bool DebugMesh = false;
        public bool DebugAABB = false;

        void Start()
        {
            if (MeshRenderer == null)
            {
                MeshRenderer = GetComponent<Renderer>();
            }

            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }

            mesh = meshFilter.mesh;

            jMeshIdentity = JMesh.CalculateJMesh(mesh.vertices, mesh.triangles);
        }

        public Vector3[] GetEdgeCoordinates()
        {
            if (jMeshFrameInstance.VerticesTransformed == null) return new Vector3[0];
            return jMeshFrameInstance.TransformedMesh.EdgeVertices;
        }

    }
}
