using UnityEngine;
using System.Collections;
using NUnit.Framework;
using Jerre.JPhysics;

namespace Tests
{
    public class JMeshOverlapTest
    {
        public static Vector3[] triangleA = new Vector3[]
        {
            new Vector3(0, 0, 2),
            new Vector3(4, 0, 0),
            new Vector3(5, 0, 7)
        };
        public static int[] trianglesA = new int[] { 0, 1, 2 };
        public static JMesh meshA = JMesh.CalculateJMesh(triangleA, trianglesA);

        public static Vector3[] triangleB = new Vector3[]
        {
            new Vector3(1, 0, 4),
            new Vector3(5, 0, 1),
            new Vector3(6, 0, 3)
        };
        public static int[] trianglesB = new int[] { 0, 1, 2 };
        public static JMesh meshB = JMesh.CalculateJMesh(triangleB, trianglesB);

        public static Vector3[] square = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1)
        };
        public static int[] squareTriangles = new int[]
        {
            0, 1, 2,
            2, 3, 0
        };
        public static JMesh squareMesh = JMesh.CalculateJMesh(square, squareTriangles);

        [Test]
        public void AABB_should_not_overlap_for_squares()
        {
            var mesh = squareMesh;
            var rightOf = JMesh.FromMeshAndTransform(mesh, Matrix4x4.Translate(new Vector3(2, 0, 0)));
            var above = JMesh.FromMeshAndTransform(mesh, Matrix4x4.Translate(new Vector3(0, 0, 2)));
            var upperRight = JMesh.FromMeshAndTransform(mesh, Matrix4x4.Translate(new Vector3(2, 0, 2)));
            var lowerRight = JMesh.FromMeshAndTransform(mesh, Matrix4x4.Translate(new Vector3(2, 0, -2)));

            Assert.IsFalse(JMeshOverlap.AABBOverlap(mesh.AABB, rightOf.AABB));
            Assert.IsFalse(JMeshOverlap.AABBOverlap(rightOf.AABB, mesh.AABB));

            Assert.IsFalse(JMeshOverlap.AABBOverlap(mesh.AABB, above.AABB));
            Assert.IsFalse(JMeshOverlap.AABBOverlap(above.AABB, mesh.AABB));

            Assert.IsFalse(JMeshOverlap.AABBOverlap(mesh.AABB, upperRight.AABB));
            Assert.IsFalse(JMeshOverlap.AABBOverlap(upperRight.AABB, mesh.AABB));

            Assert.IsFalse(JMeshOverlap.AABBOverlap(mesh.AABB, lowerRight.AABB));
            Assert.IsFalse(JMeshOverlap.AABBOverlap(lowerRight.AABB, mesh.AABB));
        }

        [Test]
        public void AABB_should_overlap()
        {
            var mesh = squareMesh;
            var rightOf = JMesh.FromMeshAndTransform(mesh, Matrix4x4.Translate(new Vector3(0.5f, 0, 0)));
            var above = JMesh.FromMeshAndTransform(mesh, Matrix4x4.Translate(new Vector3(0, 0, 0.5f)));
            var upperRight = JMesh.FromMeshAndTransform(mesh, Matrix4x4.Translate(new Vector3(0.5f, 0, 0.5f)));
            var lowerRight = JMesh.FromMeshAndTransform(mesh, Matrix4x4.Translate(new Vector3(0.5f, 0, -0.5f)));

            Assert.IsTrue(JMeshOverlap.AABBOverlap(mesh.AABB, rightOf.AABB));
            Assert.IsTrue(JMeshOverlap.AABBOverlap(rightOf.AABB, mesh.AABB));

            Assert.IsTrue(JMeshOverlap.AABBOverlap(mesh.AABB, above.AABB));
            Assert.IsTrue(JMeshOverlap.AABBOverlap(above.AABB, mesh.AABB));

            Assert.IsTrue(JMeshOverlap.AABBOverlap(mesh.AABB, upperRight.AABB));
            Assert.IsTrue(JMeshOverlap.AABBOverlap(upperRight.AABB, mesh.AABB));

            Assert.IsTrue(JMeshOverlap.AABBOverlap(mesh.AABB, lowerRight.AABB));
            Assert.IsTrue(JMeshOverlap.AABBOverlap(lowerRight.AABB, mesh.AABB));

            Assert.IsTrue(JMeshOverlap.AABBOverlap(mesh.AABB, mesh.AABB));
        }

        [Test]
        public void Slopes_cross()
        {
            Assert.IsTrue(JMeshOverlap.LinesCross(Vector3.zero, new Vector3(3, 0, 2), new Vector3(2, 0, 3), new Vector3(3, 0, 0)));
            Assert.IsTrue(JMeshOverlap.LinesCross(new Vector3(3, 0, 2), Vector3.zero, new Vector3(3, 0, 0), new Vector3(2, 0, 3)));

            Assert.IsTrue(JMeshOverlap.LinesCross(Vector3.zero, new Vector3(3, 0, 0), new Vector3(1, 0, -1), new Vector3(2, 0, 0)));
            Assert.IsTrue(JMeshOverlap.LinesCross(new Vector3(3, 0, 0), Vector3.zero, new Vector3(2, 0, 0), new Vector3(1, 0, -1)));

            Assert.IsTrue(JMeshOverlap.LinesCross(Vector3.zero, new Vector3(0, 0, 2), new Vector3(-1, 0, 1), new Vector3(1, 0, 0)));
            Assert.IsTrue(JMeshOverlap.LinesCross(new Vector3(0, 0, 2), Vector3.zero, new Vector3(1, 0, 0), new Vector3(-1, 0, 1)));
        }

        [Test]
        public void Slopes_should_not_cross()
        {
            Assert.IsFalse(JMeshOverlap.LinesCross(Vector3.zero, Vector3.right, new Vector3(0, 0, 1), new Vector3(1, 0, 1)));
            Assert.IsFalse(JMeshOverlap.LinesCross(Vector3.right, Vector3.zero, new Vector3(1, 0, 1), new Vector3(0, 0, 1)));

            Assert.IsFalse(JMeshOverlap.LinesCross(Vector3.zero, Vector3.forward, new Vector3(1, 0, 0), new Vector3(1, 0, 1)));
            Assert.IsFalse(JMeshOverlap.LinesCross(Vector3.forward, Vector3.zero, new Vector3(1, 0, 1), new Vector3(1, 0, 0)));

            Assert.IsFalse(JMeshOverlap.LinesCross(Vector3.zero, Vector3.one, new Vector3(-1.5f, 0, 1.5f), new Vector3(1.5f, 0, 1.5f)));
            Assert.IsFalse(JMeshOverlap.LinesCross(Vector3.one, Vector3.zero, new Vector3(1.5f, 0, 1.5f), new Vector3(-1.5f, 0, 1.5f)));

            Assert.IsFalse(JMeshOverlap.LinesCross(new Vector3(5, 0, 4), new Vector3(9, 0, 1), new Vector3(4, 0, 0), new Vector3(5, 0, 7)));
            Assert.IsFalse(JMeshOverlap.LinesCross(new Vector3(4, 0, 0), new Vector3(5, 0, 7), new Vector3(5, 0, 4), new Vector3(9, 0, 1)));
        }

        [Test]
        public void Meshes_should_not_overlap()
        {
            Assert.IsFalse(JMeshOverlap.MeshesOverlap(meshA, JMesh.FromMeshAndTransform(meshB, Matrix4x4.Translate(new Vector3(10, 0, 0)))));
            Assert.IsFalse(JMeshOverlap.MeshesOverlap(meshA, JMesh.FromMeshAndTransform(meshB, Matrix4x4.Translate(new Vector3(4, 0, 0)))));
        }

        [Test]
        public void Meshes_should_overlap_without_having_a_point_inside_the_other()
        {
            Assert.IsTrue(JMeshOverlap.MeshesOverlap(meshA, meshB));
        }
    }
}
