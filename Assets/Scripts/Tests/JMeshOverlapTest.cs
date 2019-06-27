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

        public static Vector3[] triangleC = new Vector3[]
        {
            new Vector3(2, 0, 3),
            new Vector3(3, 0, 1),
            new Vector3(4, 0, 4)
        };
        public static int[] trianglesC = new int[]
        {
            0, 1, 2
        };
        public static JMesh meshC = JMesh.CalculateJMesh(triangleC, trianglesC);

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

            Assert.IsFalse(JMeshOverlap.LinesCross(new Vector3(0, 0, 2), new Vector3(5, 0, 7), new Vector3(10, 0, 3), new Vector3(5, 0, 4)));
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

        [Test]
        public void Meshes_should_overlap_when_a_smaller_mesh_is_completely_inside_a_larger_mesh()
        {
            Assert.IsTrue(JMeshOverlap.MeshesOverlap(meshA, meshC));
            Assert.IsTrue(JMeshOverlap.MeshesOverlap(meshC, meshA));
        }

        [Test]
        public void Point_should_be_inside_triangle()
        {
            var mesh = JMeshPhysicsMeshes.triangleMeshIdentity;
            Vector3 point = new Vector3(0.5f, 0, 0.25f);
            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(point, mesh));
            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(new Vector3(1f, 0, 0), mesh));
        }

        [Test]
        public void Point_should_be_outside_the_triangle_on_its_right()
        {
            var mesh = JMeshPhysicsMeshes.triangleMeshIdentity;
            Vector3 point = new Vector3(1.25f, 0, 0.25f);
            Assert.IsFalse(JMeshOverlap.IsPointInsideMesh(point, mesh));
        }

        [Test]
        public void Point_should_be_outside_the_triangle_since_the_horizontal_raycast_touches_a_single_vertex()
        {
            var mesh = meshA;
            var point = new Vector3(mesh.EdgeVertices[1].x + 0.1f, 0, mesh.EdgeVertices[1].z);
            Assert.IsFalse(JMeshOverlap.IsPointInsideMesh(point, mesh));
        }

        [Test]
        public void Point_should_be_inside_the_transformed_square_mesh()
        {
            var mesh = JMeshPhysicsMeshes.squareMeshIdentity;
            var meshFrameInstance = JMesh.FromMeshAndTransform(mesh, Matrix4x4.TRS(
                new Vector3(1, 0, 1),
                Quaternion.Euler(Vector3.up * 45),
                Vector3.one * 2
            ));

            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(new Vector3(1.001f, 0, 1f), meshFrameInstance));
            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(new Vector3(1f, 0, 1f), meshFrameInstance));
        }

        [Test]
        public void Point_shold_be_inside_mesh_when_point_is_exactly_on_a_mesh_edge_vertex()
        {
            var mesh = JMeshPhysicsMeshes.squareMeshIdentity;
            var meshFrameInstance = JMesh.FromMeshAndTransform(mesh, Matrix4x4.TRS(
                new Vector3(1, 0, 1),
                Quaternion.Euler(Vector3.up * 45),
                Vector3.one * 2
            ));

            var instanceMesh = meshFrameInstance;

            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(instanceMesh.EdgeVertices[0], meshFrameInstance));
            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(instanceMesh.EdgeVertices[1], meshFrameInstance));
            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(instanceMesh.EdgeVertices[2], meshFrameInstance));
            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(instanceMesh.EdgeVertices[3], meshFrameInstance));
        }

        [Test]
        public void Point_shold_be_inside_mesh_when_point_is_on_a_mesh_edge()
        {
            var mesh = JMeshPhysicsMeshes.squareMeshIdentity;

            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(new Vector3(0.5f, 0, 0), mesh));
            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(new Vector3(1f, 0, 0.5f), mesh));
            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(new Vector3(0.5f, 0, 1), mesh));
            Assert.IsTrue(JMeshOverlap.IsPointInsideMesh(new Vector3(0, 0, 0.5f), mesh));
        }
    }
}
