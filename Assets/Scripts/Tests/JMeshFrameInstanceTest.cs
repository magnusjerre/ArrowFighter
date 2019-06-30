using Jerre.JColliders;
using Jerre.JPhysics;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class JMeshFrameInstanceTest
    {
        [Test]
        public void CheckMatrixTransformation()
        {
            var diff = 0.0001f;
            var matrix = Matrix4x4.TRS(Vector3.one, Quaternion.Euler(Vector3.up * 45), Vector3.one * 2);
            
            TestMethods.AreEqualIsh(1.41421f, matrix.m00, diff); 
            TestMethods.AreEqualIsh(0f, matrix.m01, diff);  //Antar at dette er bortover
            TestMethods.AreEqualIsh(1.41421f, matrix.m02, diff);
            TestMethods.AreEqualIsh(1f, matrix.m03, diff);

            TestMethods.AreEqualIsh(0f, matrix.m10, diff);
            TestMethods.AreEqualIsh(2f, matrix.m11, diff);
            TestMethods.AreEqualIsh(0f, matrix.m12, diff);
            TestMethods.AreEqualIsh(1f, matrix.m13, diff);

            TestMethods.AreEqualIsh(-1.41421f, matrix.m20, diff);
            TestMethods.AreEqualIsh(0f, matrix.m21, diff);
            TestMethods.AreEqualIsh(1.41421f, matrix.m22, diff);
            TestMethods.AreEqualIsh(1f, matrix.m23, diff);

            TestMethods.AreEqualIsh(0f, matrix.m30, diff);
            TestMethods.AreEqualIsh(0f, matrix.m31, diff);
            TestMethods.AreEqualIsh(0f, matrix.m32, diff);
            TestMethods.AreEqualIsh(1f, matrix.m33, diff);
        }

        [Test]
        public void Calculate_frame_instance_for_triangle_rotated_90_degrees_anti_clockwise_with_translation()
        {
            var startMesh = JMeshPhysicsMeshes.triangleMeshIdentity;
            var jMesh = JMesh.FromMeshAndTransform(startMesh, Matrix4x4.TRS(
                    new Vector3(1, 0, 1),
                    Quaternion.Euler(Vector3.up * -90),
                    Vector3.one
                ));

            TestMethods.AreEqualIsh(Vector3.right, startMesh.EdgeOutwardNormals[1], "bc normal");

            var vcs = jMesh.EdgeVertices;
            TestMethods.AreEqualIsh(new Vector3(1, 0, 1), vcs[0], "v0");
            TestMethods.AreEqualIsh(new Vector3(1, 0, 2), vcs[1], "v1");
            TestMethods.AreEqualIsh(new Vector3(0, 0, 2), vcs[2], "v2");

            var ns = jMesh.EdgeOutwardNormals;
            TestMethods.AreEqualIsh(Vector3.right, ns[0], "ns0");
            TestMethods.AreEqualIsh(Vector3.forward, ns[1], "ns1");
            TestMethods.AreEqualIsh(new Vector3(-1, 0, -1).normalized, ns[2], "ns2");

            var bounds = jMesh.AABB;
            var center = bounds.center;
            var min = bounds.min;
            var max = bounds.max;
            TestMethods.AreEqualIsh(new Vector3(0.5f, 0, 1.5f), center, "center");
            TestMethods.AreEqualIsh(0f, min.x, TestMethods.VECTOR_DIFF);
            TestMethods.AreEqualIsh(1f, min.z, TestMethods.VECTOR_DIFF);
            TestMethods.AreEqualIsh(1f, max.x, TestMethods.VECTOR_DIFF);
            TestMethods.AreEqualIsh(2f, max.z, TestMethods.VECTOR_DIFF);
        }

        [Test]
        public void Calculate_frame_instance_for_square_rotated_45_degrees_clockwise()
        {
            var startMesh = JMeshPhysicsMeshes.squareMeshIdentity;
            var jMesh = JMesh.FromMeshAndTransform(startMesh, Matrix4x4.TRS(
                    new Vector3(1, 0, 1),
                    Quaternion.Euler(Vector3.up * 45),
                    Vector3.one * 2
                ));

            var halfHeight = Mathf.Sqrt(2);
            var upRightDirection = new Vector3(1, 0, 1).normalized;

            var vcs = jMesh.EdgeVertices;
            TestMethods.AreEqualIsh(new Vector3(1, 0, 1), vcs[0], "v0");
            TestMethods.AreEqualIsh(new Vector3(1 + halfHeight, 0, 1 - halfHeight), vcs[1], "v1");
            TestMethods.AreEqualIsh(new Vector3(1 + 2 * halfHeight, 0, 1), vcs[2], "v2");
            TestMethods.AreEqualIsh(new Vector3(1 + halfHeight, 0, 1 + halfHeight), vcs[3], "v3");

            var ns = jMesh.EdgeOutwardNormals;
            TestMethods.AreEqualIsh(-upRightDirection, ns[0], "ns0");
            TestMethods.AreEqualIsh(new Vector3(upRightDirection.x, 0, -upRightDirection.z), ns[1], "ns1");
            TestMethods.AreEqualIsh(upRightDirection, ns[2], "ns2");
            TestMethods.AreEqualIsh(new Vector3(-upRightDirection.x, 0, upRightDirection.z), ns[3], "ns3");
        }

        [Test]
        public void Calculate_frame_instance_for_nonunformly_scaled_triangle()
        {
            var transformed = JMesh.FromMeshAndTransform(JMeshPhysicsMeshes.triangleMeshIdentity, Matrix4x4.Scale(new Vector3(1, 1, 2)));
            var expected = JMeshPhysicsMeshes.triangleTallMeshIdentity;

            var tMesh = transformed;
            TestMethods.AreEqualIsh(expected.AABB.size, tMesh.AABB.size);
            TestMethods.AreEqualIsh(expected.EdgeOutwardNormals[0], tMesh.EdgeOutwardNormals[0]);
            TestMethods.AreEqualIsh(expected.EdgeOutwardNormals[1], tMesh.EdgeOutwardNormals[1]);
            TestMethods.AreEqualIsh(expected.EdgeOutwardNormals[2], tMesh.EdgeOutwardNormals[2]);

            TestMethods.AreEqualIsh(expected.EdgeVertices[0], tMesh.EdgeVertices[0]);
            TestMethods.AreEqualIsh(expected.EdgeVertices[1], tMesh.EdgeVertices[1]);
            TestMethods.AreEqualIsh(expected.EdgeVertices[2], tMesh.EdgeVertices[2]);
        }
    }
}
