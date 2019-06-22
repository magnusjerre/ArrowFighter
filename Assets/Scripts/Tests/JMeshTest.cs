using Jerre.JPhysics;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class JMeshTest
    {
        [Test]
        public void Extract_triangle_edge_vertices_test()
        {
            var vertices = JMesh.ExtractEdgeVertices(JMeshPhysicsMeshes.triangle, JMeshPhysicsMeshes.triangleTriangles);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.triangle[0], vertices[0]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.triangle[1], vertices[1]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.triangle[2], vertices[2]);
        }

        [Test]
        public void Extract_square_edge_vertices_test()
        {
            var vertices = JMesh.ExtractEdgeVertices(JMeshPhysicsMeshes.square, JMeshPhysicsMeshes.squareTriangles);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.square[0], vertices[0]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.square[1], vertices[1]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.square[2], vertices[2]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.square[3], vertices[3]);
        }

        [Test]
        public void Extract_square_edge_vertices_for_reversed_triangle_faces_test()
        {
            var vertices = JMesh.ExtractEdgeVertices(JMeshPhysicsMeshes.square, JMeshPhysicsMeshes.squareTrianglesDifferentTriangles);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.square[0], vertices[0]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.square[1], vertices[1]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.square[2], vertices[2]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.square[3], vertices[3]);
        }

        [Test]
        public void Extract_square_five_edge_vertices_test()
        {
            var vertices = JMesh.ExtractEdgeVertices(JMeshPhysicsMeshes.squareFive, JMeshPhysicsMeshes.squareFiveTriangles);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareFive[0], vertices[0]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareFive[1], vertices[1]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareFive[3], vertices[2]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareFive[4], vertices[3]);
        }

        [Test]
        public void Calculate_outward_normals_of_triangle()
        {
            var vertices = JMesh.ExtractEdgeVertices(JMeshPhysicsMeshes.triangle, JMeshPhysicsMeshes.triangleTriangles);
            var normals = JMesh.CalculateOutwardNormals(vertices);
            Assert.AreEqual(3, normals.Length);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.triangleNormals[0], normals[0]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.triangleNormals[1], normals[1]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.triangleNormals[2], normals[2]);
        }

        [Test]
        public void Calculate_outwards_normals_of_tall_triangle()
        {
            var vertices = JMesh.ExtractEdgeVertices(JMeshPhysicsMeshes.triangleTall, JMeshPhysicsMeshes.triangleTallTriangles);
            var normals = JMesh.CalculateOutwardNormals(vertices);
            Assert.AreEqual(3, normals.Length);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.triangleTallNormals[0], normals[0]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.triangleTallNormals[1], normals[1]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.triangleTallNormals[2], normals[2]);
        }

        [Test]
        public void Calculate_outward_normals_of_square()
        {
            var vertices = JMesh.ExtractEdgeVertices(JMeshPhysicsMeshes.square, JMeshPhysicsMeshes.squareTriangles);
            var normals = JMesh.CalculateOutwardNormals(vertices);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareNormals[0], normals[0]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareNormals[1], normals[1]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareNormals[2], normals[2]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareNormals[3], normals[3]);
        }

        [Test]
        public void Calculate_outward_normals_of_square_different_triangles_should_give_same_as_normal_since_vertex_order_is_the_same()
        {
            var vertices = JMesh.ExtractEdgeVertices(JMeshPhysicsMeshes.square, JMeshPhysicsMeshes.squareTrianglesDifferentTriangles);
            var normals = JMesh.CalculateOutwardNormals(vertices);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareFiveNormals[0], normals[0]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareFiveNormals[1], normals[1]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareFiveNormals[2], normals[2]);
            TestMethods.AreEqualIsh(JMeshPhysicsMeshes.squareFiveNormals[3], normals[3]);
        }

        [Test]
        public void Calculate_AABB()
        {
            var bounds = JMesh.CalculateBounds(JMeshPhysicsMeshes.triangle);
            TestMethods.AreEqualIsh(bounds.center, new Vector3(0.5f, 0, 0.5f));
            TestMethods.AreEqualIsh(bounds.size, new Vector3(1f, 0, 1f));
        }


        [Test]
        public void Check_calculated_tall_triangle_JMesh()
        {
            var triangle = JMeshPhysicsMeshes.triangleTallMeshIdentity;
            TestMethods.AreEqualIsh(new Vector3(1f, 0, 2f), triangle.AABB.size);
            TestMethods.AreEqualIsh(new Vector3(0, 0, -1), triangle.EdgeOutwardNormals[0]);
            TestMethods.AreEqualIsh(new Vector3(1, 0, 0), triangle.EdgeOutwardNormals[1]);
            TestMethods.AreEqualIsh(new Vector3(-2, 0, 1).normalized, triangle.EdgeOutwardNormals[2]);

            TestMethods.AreEqualIsh(new Vector3(0, 0, 0), triangle.EdgeVertices[0]);
            TestMethods.AreEqualIsh(new Vector3(1, 0, 0), triangle.EdgeVertices[1]);
            TestMethods.AreEqualIsh(new Vector3(1, 0, 2), triangle.EdgeVertices[2]);
        }
    }
}
