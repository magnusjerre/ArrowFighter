using Jerre.JPhysics;
using NUnit.Framework;
using UnityEngine;
using static Jerre.JPhysics.Intersection;

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


        [Test]
        public void Check_that_the_slopes_intersect()
        {
            var verticalFromOrigo = Slope.FromPoints(Vector3.zero, Vector3.forward);
            var horizontalFromOrigo = Slope.FromPoints(Vector3.zero, Vector3.right);

            var verticalOffset = Slope.FromPoints(new Vector3(1, 0, 0), new Vector3(1, 0, 1));
            var horizontalOffset = Slope.FromPoints(new Vector3(0, 0, 1), new Vector3(2, 0, 1));

            var upRightFromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, 1));
            var upRightOffsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 2), new Vector3(2, 0, 3));

            var downRightFromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, -1));
            var downRightOffsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 1), new Vector3(2, 0, 0));

            var fromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, 3));
            var offsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 1), new Vector3(2, 0, 4));

            checkIntersect(verticalFromOrigo, horizontalFromOrigo, Vector3.zero);
            checkIntersect(verticalFromOrigo, horizontalOffset, new Vector3(0, 0, 1));
            checkIntersect(verticalFromOrigo, upRightFromOrigo, Vector3.zero);
            checkIntersect(verticalFromOrigo, upRightOffsetFromOrigo, new Vector3(0, 0, 1));
            checkIntersect(verticalFromOrigo, downRightFromOrigo, Vector3.zero);
            checkIntersect(verticalFromOrigo, downRightOffsetFromOrigo, new Vector3(0, 0, 2));
            checkIntersect(verticalFromOrigo, fromOrigo, Vector3.zero);
            checkIntersect(verticalFromOrigo, offsetFromOrigo, new Vector3(0, 0, -2));
            checkIntersect(horizontalFromOrigo, verticalFromOrigo, Vector3.zero);
            checkIntersect(horizontalFromOrigo, upRightFromOrigo, Vector3.zero);
            checkIntersect(horizontalFromOrigo, upRightOffsetFromOrigo, new Vector3(-1, 0, 0));
            checkIntersect(horizontalFromOrigo, downRightFromOrigo, Vector3.zero);
            checkIntersect(horizontalFromOrigo, fromOrigo, Vector3.zero);
            checkIntersect(verticalOffset, upRightFromOrigo, new Vector3(1, 0, 1));
            checkIntersect(verticalOffset, downRightFromOrigo, new Vector3(1, 0, -1));
            checkIntersect(verticalOffset, fromOrigo, new Vector3(1, 0, 3));
            checkIntersect(upRightFromOrigo, downRightFromOrigo, Vector3.zero);
            checkIntersect(upRightFromOrigo, offsetFromOrigo, new Vector3(1, 0, 1));
        }

        private void checkIntersect(Slope slopeA, Slope slopeB, Vector3 intersectionPoint)
        {
            var intersection = slopeA.CalculateIntersection(slopeB);
            Assert.AreEqual(IntersectionType.INTERSECT, intersection.Type);
            TestMethods.AreEqualIsh(intersectionPoint, intersection.Point);
        }

        [Test]
        public void Check_that_slopes_overlap()
        {
            var verticalFromOrigo = Slope.FromPoints(Vector3.zero, Vector3.forward);
            var horizontalFromOrigo = Slope.FromPoints(Vector3.zero, Vector3.right);

            var verticalOffset = Slope.FromPoints(new Vector3(1, 0, 0), new Vector3(1, 0, 1));
            var horizontalOffset = Slope.FromPoints(new Vector3(0, 0, 1), new Vector3(2, 0, 1));

            var upRightFromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, 1));
            var upRightOffsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 2), new Vector3(2, 0, 3));

            var downRightFromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, -1));
            var downRightOffsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 1), new Vector3(2, 0, 0));

            var fromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, 3));
            var offsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 1), new Vector3(2, 0, 4));

            checkOverlapsSelf(verticalFromOrigo);
            checkOverlapsSelf(horizontalFromOrigo);
            checkOverlapsSelf(verticalOffset);
            checkOverlapsSelf(horizontalOffset);
            checkOverlapsSelf(upRightFromOrigo);
            checkOverlapsSelf(upRightOffsetFromOrigo);
            checkOverlapsSelf(downRightFromOrigo);
            checkOverlapsSelf(downRightOffsetFromOrigo);
            checkOverlapsSelf(fromOrigo);
            checkOverlapsSelf(offsetFromOrigo);
        }

        private void checkOverlapsSelf(Slope slope)
        {
            Assert.AreEqual(IntersectionType.OVERLAP, slope.CalculateIntersection(slope).Type);
        }

        [Test]
        public void Check_that_slopes_do_not_overlap()
        {
            var verticalFromOrigo = Slope.FromPoints(Vector3.zero, Vector3.forward);
            var horizontalFromOrigo = Slope.FromPoints(Vector3.zero, Vector3.right);

            var verticalOffset = Slope.FromPoints(new Vector3(1, 0, 0), new Vector3(1, 0, 1));
            var horizontalOffset = Slope.FromPoints(new Vector3(0, 0, 1), new Vector3(2, 0, 1));

            var upRightFromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, 1));
            var upRightOffsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 2), new Vector3(2, 0, 3));

            var downRightFromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, -1));
            var downRightOffsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 1), new Vector3(2, 0, 0));

            var fromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, 3));
            var offsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 1), new Vector3(2, 0, 4));

            checkNotOverlap(verticalFromOrigo, horizontalFromOrigo);
            checkNotOverlap(verticalFromOrigo, verticalOffset);
            checkNotOverlap(verticalFromOrigo, horizontalOffset);
            checkNotOverlap(verticalFromOrigo, upRightFromOrigo);
            checkNotOverlap(verticalFromOrigo, upRightOffsetFromOrigo);
            checkNotOverlap(verticalFromOrigo, upRightFromOrigo);
            checkNotOverlap(verticalFromOrigo, upRightOffsetFromOrigo);
            checkNotOverlap(verticalFromOrigo, downRightFromOrigo);
            checkNotOverlap(verticalFromOrigo, downRightOffsetFromOrigo);
            checkNotOverlap(verticalFromOrigo, fromOrigo);
            checkNotOverlap(verticalFromOrigo, offsetFromOrigo);
        }

        private void checkNotOverlap(Slope slopeA, Slope slopeB)
        {
            Assert.AreNotEqual(IntersectionType.OVERLAP, slopeA.CalculateIntersection(slopeB).Type);
        }

        [Test]
        public void Check_that_slopes_do_not_intersect_or_overlap()
        {
            var verticalFromOrigo = Slope.FromPoints(Vector3.zero, Vector3.forward);
            var verticalOffset = Slope.FromPoints(new Vector3(1, 0, 0), new Vector3(1, 0, 1));

            var horizontalFromOrigo = Slope.FromPoints(Vector3.zero, Vector3.right);
            var horizontalOffset = Slope.FromPoints(new Vector3(0, 0, 1), new Vector3(2, 0, 1));

            var upRightFromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, 1));
            var upRightOffsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 2), new Vector3(2, 0, 3));

            var downRightFromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, -1));
            var downRightOffsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 1), new Vector3(2, 0, 0));

            var fromOrigo = Slope.FromPoints(Vector3.zero, new Vector3(1, 0, 3));
            var offsetFromOrigo = Slope.FromPoints(new Vector3(1, 0, 1), new Vector3(2, 0, 4));

            checkIntersectionNone(verticalFromOrigo, verticalOffset);
            checkIntersectionNone(horizontalFromOrigo, horizontalOffset);
            checkIntersectionNone(upRightFromOrigo, upRightOffsetFromOrigo);
            checkIntersectionNone(downRightFromOrigo, downRightOffsetFromOrigo);
            checkIntersectionNone(fromOrigo, offsetFromOrigo);
        }

        private void checkIntersectionNone(Slope slopeA, Slope slopeB)
        {
            Assert.AreEqual(IntersectionType.NONE, slopeA.CalculateIntersection(slopeB).Type);
        }

    }
}
