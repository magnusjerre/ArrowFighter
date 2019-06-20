using UnityEngine;

namespace Jerre.JPhysics
{
    public struct JMeshFrameInstance
    {
        public Bounds Aabb;
        public Vector3[] VerticesTransformed;
        public Vector3[] NormalsTransformed;

        public JMesh TransformedMesh;

        public JMeshFrameInstance(Bounds aabb, Vector3[] verticesTransformed, Vector3[] normalsTransformed, JMesh transformedMesh)
        {
            Debug.Log("JMeshFrameInstance: aab: " + aabb + ", verticesTransformed: " + verticesTransformed + ", normalsTransformed. " + normalsTransformed);
            Aabb = aabb;
            VerticesTransformed = verticesTransformed;
            NormalsTransformed = normalsTransformed;
            TransformedMesh = transformedMesh;
        }

        public static JMeshFrameInstance FromMeshAndTransform(JMeshDefintion mesh, Transform transform)
        {
            var transformMatrix = transform.localToWorldMatrix;

            var vertices = mesh.Vertices;
            var verticesLength = vertices.Length;
            var transformedVertices = new Vector3[verticesLength];

            var maxX = float.MinValue;
            var minX = float.MaxValue;
            var maxY = float.MinValue;
            var minY = float.MaxValue;
            var maxZ = float.MinValue;
            var minZ = float.MaxValue;
            for (var i = 0; i < verticesLength; i++)
            {
                var transformedVertex = transformMatrix.MultiplyPoint3x4(vertices[i]);
                transformedVertices[i] = transformedVertex;
                var newX = transformedVertex.x;
                var newY = transformedVertex.y;
                var newZ = transformedVertex.z;
                if (newX > maxX)
                {
                    maxX = newX;
                }
                if (newX < minX)
                {
                    minX = newX;
                }
                if (newY > maxY)
                {
                    maxY = newY;
                } else if (newY < minY)
                {
                    minY = newY;
                }
                if (newZ > maxZ)
                {
                    maxZ = newZ;
                }
                if (newZ < minZ)
                {
                    minZ = newZ;
                }
            }

            var normals = mesh.Normals;
            var normalsLength = normals.Length;
            var transformedNormals = new Vector3[normalsLength];
            for (var i = 0; i < normalsLength; i++)
            {
                transformedNormals[i] = transformMatrix.MultiplyVector(normals[i]);
            }

            return new JMeshFrameInstance(
                new Bounds(transform.position, new Vector3(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY), Mathf.Abs(maxZ - minZ))),
                transformedVertices,
                transformedNormals,
                new JMesh(transformedVertices, transformedNormals));
        }

        public static JMeshFrameInstance FromMeshAndTransform(JMesh meshIdentity, Matrix4x4 transformMatrix)
        {
            var vertices = meshIdentity.EdgeVertices;
            var verticesLength = vertices.Length;
            var transformedVertices = new Vector3[verticesLength];

            var maxX = float.MinValue;
            var minX = float.MaxValue;
            var maxY = float.MinValue;
            var minY = float.MaxValue;
            var maxZ = float.MinValue;
            var minZ = float.MaxValue;
            for (var i = 0; i < verticesLength; i++)
            {
                var transformedVertex = transformMatrix.MultiplyPoint3x4(vertices[i]);
                transformedVertices[i] = transformedVertex;
                var newX = transformedVertex.x;
                var newY = transformedVertex.y;
                var newZ = transformedVertex.z;
                if (newX > maxX)
                {
                    maxX = newX;
                }
                if (newX < minX)
                {
                    minX = newX;
                }
                if (newY > maxY)
                {
                    maxY = newY;
                }
                else if (newY < minY)
                {
                    minY = newY;
                }
                if (newZ > maxZ)
                {
                    maxZ = newZ;
                }
                if (newZ < minZ)
                {
                    minZ = newZ;
                }
            }

            var normals = meshIdentity.EdgeOutwardNormals;
            var normalsLength = normals.Length;
            var transformedNormals = new Vector3[normalsLength];
            var rotationMatrix = Matrix4x4.Rotate(transformMatrix.rotation);
            for (var i = 0; i < normalsLength; i++)
            {
                // Need to only use the rotation matrix to keep the normalized length, not translate or scale.
                transformedNormals[i] = rotationMatrix.MultiplyVector(normals[i]);
            }

            var width = Mathf.Abs(maxX - minX);
            var depth = Mathf.Abs(maxY - minY);
            var height = Mathf.Abs(maxZ - minZ);
            var centerX = minX + width / 2f;
            var centerY = minY + depth / 2f;
            var centerZ = minZ + height / 2f;
            

            return new JMeshFrameInstance(
                new Bounds(
                    new Vector3(
                        centerX,
                        centerY,
                        centerZ
                    ), 
                    new Vector3(width, depth, height)
                    ),
                transformedVertices,
                transformedNormals,
                new JMesh(transformedVertices, transformedNormals));
        }

    }
}
