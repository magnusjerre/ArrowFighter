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
        public JMeshDefintion jMeshDefintion;
        public JMeshFrameInstance jMeshFrameInstance;

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

            jMeshDefintion = JMeshDefintion.FromMesh(mesh);
        }

        public Vector3[] GetEdgeCoordinates()
        {
            if (jMeshFrameInstance.VerticesTransformed == null) return new Vector3[0];
            var coordinates = new Vector3[jMeshDefintion.EdgeIndices.Length];
            var length = coordinates.Length;
            var instanceVertices = jMeshFrameInstance.VerticesTransformed;
            var edgeIndices = jMeshDefintion.EdgeIndices;
            for (var i = 0; i < length; i++)
            {
                coordinates[i] = instanceVertices[edgeIndices[i]];
            }

            return coordinates;
        }

    }
}
