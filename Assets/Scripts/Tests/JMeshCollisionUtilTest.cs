﻿using Jerre.JPhysics;
using Jerre.Utils;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static Jerre.JPhysics.JMeshCollisionUtil;

namespace Tests
{
    public class JMeshCollisionUtilTest
    {
        [Test]
        public void Point_should_be_inside_triangle()
        {
            var mesh = JMeshPhysicsMeshes.triangleMeshIdentity;
            Vector3 point = new Vector3(0.5f, 0, 0.25f);
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(point, mesh));
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(new Vector3(1f, 0, 0), mesh));
        }

        [Test]
        public void Point_should_be_outside_the_triangle_on_its_right()
        {
            var mesh = JMeshPhysicsMeshes.triangleMeshIdentity;
            Vector3 point = new Vector3(1.25f, 0, 0.25f);
            Assert.IsFalse(JMeshCollisionUtil.IsPointInsideMesh(point, mesh));
        }

        [Test]
        public void Point_should_be_inside_the_transformed_square_mesh()
        {
            var mesh = JMeshPhysicsMeshes.squareMeshIdentity;
            var meshFrameInstance = JMeshFrameInstance.FromMeshAndTransform(mesh, Matrix4x4.TRS(
                new Vector3(1, 0, 1),
                Quaternion.Euler(Vector3.up * 45),
                Vector3.one * 2
            ));

            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(new Vector3(1.001f, 0, 1f), meshFrameInstance.TransformedMesh));
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(new Vector3(1f, 0, 1f), meshFrameInstance.TransformedMesh));
        }

        [Test]
        public void Point_shold_be_inside_mesh_when_point_is_exactly_on_a_mesh_edge_vertex()
        {
            var mesh = JMeshPhysicsMeshes.squareMeshIdentity;
            var meshFrameInstance = JMeshFrameInstance.FromMeshAndTransform(mesh, Matrix4x4.TRS(
                new Vector3(1, 0, 1),
                Quaternion.Euler(Vector3.up * 45),
                Vector3.one * 2
            ));

            var instanceMesh = meshFrameInstance.TransformedMesh;

            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(instanceMesh.EdgeVertices[0], meshFrameInstance.TransformedMesh));
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(instanceMesh.EdgeVertices[1], meshFrameInstance.TransformedMesh));
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(instanceMesh.EdgeVertices[2], meshFrameInstance.TransformedMesh));
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(instanceMesh.EdgeVertices[3], meshFrameInstance.TransformedMesh));
        }

        [Test]
        public void Point_shold_be_inside_mesh_when_point_is_on_a_mesh_edge()
        {
            var mesh = JMeshPhysicsMeshes.squareMeshIdentity;

            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(new Vector3(0.5f, 0, 0), mesh));
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(new Vector3(1f, 0, 0.5f), mesh));
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(new Vector3(0.5f, 0, 1), mesh));
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(new Vector3(0, 0, 0.5f), mesh));
        }

        [Test]
        public void Calculate_slope_simple()
        {
            var slope = JMeshCollisionUtil.CalculateSlope(Vector3.zero, Vector3.one);
            TestMethods.AreEqualIsh(1f, slope.a, 0.000001f);
            TestMethods.AreEqualIsh(0f, slope.b, 0.000001f);
        }

        [Test]
        public void Calculate_slope_simple_reversed()
        {
            var slope = JMeshCollisionUtil.CalculateSlope(Vector3.one, Vector3.zero);
            TestMethods.AreEqualIsh(1f, slope.a, 0.000001f);
            TestMethods.AreEqualIsh(0f, slope.b, 0.000001f);
        }

        [Test]
        public void Calculate_slope_not_starting_in_zero()
        {
            var slope = JMeshCollisionUtil.CalculateSlope(Vector3.one, new Vector3(3, 0, 2));
            TestMethods.AreEqualIsh(0.5f, slope.a, 0.00001f);
            TestMethods.AreEqualIsh(0.5f, slope.b, 0.00001f);
        }

        [Test]
        public void Calculate_slope_not_starting_in_zero_reversed()
        {
            var slope = JMeshCollisionUtil.CalculateSlope(new Vector3(3, 0, 2), Vector3.one);
            TestMethods.AreEqualIsh(0.5f, slope.a, 0.00001f);
            TestMethods.AreEqualIsh(0.5f, slope.b, 0.00001f);
        }

        [Test]
        public void Calculate_vertical_slope()
        {
            var slope = JMeshCollisionUtil.CalculateSlope(new Vector3(1, 0, 1), new Vector3(1, 0, 2));
            Assert.IsTrue(slope.isVertical);
            Assert.AreEqual(1f, slope.b);
        }

        [Test]
        public void Calculate_horizontal_slope()
        {
            var slope = JMeshCollisionUtil.CalculateSlope(new Vector3(1, 0, 1), new Vector3(2, 0, 1));
            Assert.AreEqual(0f, slope.a);
            Assert.AreEqual(1f, slope.b);
        }

        [Test]
        public void Point_should_be_on_vertical_slope()
        {
            var slope = new Slope(true, 1f);
            Assert.IsTrue(slope.IsPointOnLineInXZPlane(new Vector3(1, 0, 100), POINT_MAX_DIFF));
            Assert.IsTrue(slope.IsPointOnLineInXZPlane(new Vector3(1, 0, -50), POINT_MAX_DIFF));
        }

        [Test]
        public void Point_should_be_on_horizontal_slope()
        {
            var slope = new Slope(0f, 1f);
            Assert.IsTrue(slope.IsPointOnLineInXZPlane(new Vector3(100, 0, 1), POINT_MAX_DIFF));
            Assert.IsTrue(slope.IsPointOnLineInXZPlane(new Vector3(-10, 0, 1), POINT_MAX_DIFF));
        }

        [Test]
        public void Point_should_be_on_slanted_slope()
        {
            var slope = new Slope(1f, 0f);
            Assert.IsTrue(slope.IsPointOnLineInXZPlane(new Vector3(1f, 0, 1f), POINT_MAX_DIFF));
        }

        [Test]
        public void Point_should_not_be_on_slanted_slope()
        {
            var slope = new Slope(1f, 0f);
            Assert.IsFalse(slope.IsPointOnLineInXZPlane(new Vector3(2f, 0, 1f), POINT_MAX_DIFF));
        }

        [Test]
        public void FindMaxesForEachValueTest()
        {
            var values = new List<float[]>();
            values.Add(new float[] { 1, 2, 3, 4 });
            values.Add(new float[] { 5, 1, 7, 0 });
            values.Add(new float[] { 3, 8, 2, 4 });

            var result = JMeshCollisionUtil.FindMaxesForEachValue(values);
            Assert.AreEqual(5, result[0]);
            Assert.AreEqual(8, result[1]);
            Assert.AreEqual(7, result[2]);
            Assert.AreEqual(4, result[3]);
        }

        [Test]
        public void IndexOfSmallestValueTest()
        {
            Assert.AreEqual(2, JMeshCollisionUtil.IndexOfSmallestValue(new float[] { 5, 8, 2, 3, 4}));
        }

        [Test]
        public void CaclulatePushLenghtsForPoint_inside_triangle()
        {
            TestMethods.AreEqualIsh(
                new float[] { 0.25f, 0.25f, Mathf.Sqrt(0.25f*0.25f + 0.25f*0.25f) },
                JMeshCollisionUtil.CalculatePushLengthsForPoint(JMeshPhysicsMeshes.triangleMeshIdentity, new Vector3(0.75f, 0, 0.25f)), 
                0.0001f
            );
            TestMethods.AreEqualIsh(
                new float[] { 0.25f, 0.5f, Mathf.Sqrt(0.25f * 0.25f / 2) },
                JMeshCollisionUtil.CalculatePushLengthsForPoint(JMeshPhysicsMeshes.triangleMeshIdentity, new Vector3(0.5f, 0, 0.25f)), 
                0.0001f
            );
        }

        [Test]
        public void FindMinimumPushFromAToB_for_triangle_and_square()
        {
            var triangle = JMeshFrameInstance.FromMeshAndTransform(JMeshPhysicsMeshes.triangleMeshIdentity, Matrix4x4.Translate(new Vector3(0.5f, 0, 0.25f)));
            var square = JMeshPhysicsMeshes.squareMeshIdentity;

            var push = FindMinimumPushFromAToB(square, triangle.TransformedMesh);
            TestMethods.AreEqualIsh(0.25f, push.Magnitude, POINT_MAX_DIFF);
            TestMethods.AreEqualIsh(new Vector3(0, 0, -1), push.Direction);
        }

        [Test]
        public void FindMinimumPushFromATob_for_rotated_triangle_with_multiple_points_inside_square()
        {
            var triangle = JMeshFrameInstance.FromMeshAndTransform(JMeshPhysicsMeshes.triangleMeshIdentity, Matrix4x4.TRS(
                new Vector3(0.5f, 0, 0.75f),
                Quaternion.Euler(Vector3.up * 45),
                Vector3.one * 0.5f
            ));
            var square = JMeshPhysicsMeshes.squareMeshIdentity;

            var push = FindMinimumPushFromAToB(square, triangle.TransformedMesh);
            TestMethods.AreEqualIsh(0.5f, push.Magnitude, POINT_MAX_DIFF);
            TestMethods.AreEqualIsh(new Vector3(1, 0, 0), push.Direction);
        }

        [Test]
        public void FindMinimumPushFromATob_for_rotated_square_with_multiple_points_inside_square()
        {
            var size = 0.5f;
            var triangle = JMeshFrameInstance.FromMeshAndTransform(JMeshPhysicsMeshes.squareMeshIdentity, Matrix4x4.TRS(
                new Vector3(0, 0, 0.5f),
                Quaternion.Euler(Vector3.up * 45),
                Vector3.one * size
            ));
            var square = JMeshPhysicsMeshes.squareMeshIdentity;

            var push = FindMinimumPushFromAToB(square, triangle.TransformedMesh);
            TestMethods.AreEqualIsh(Mathf.Sqrt(size * size + size * size), push.Magnitude, POINT_MAX_DIFF);
            TestMethods.AreEqualIsh(new Vector3(-1, 0, 0), push.Direction);
        }

        [Test]
        public void FindMinimumPushFromAToB_for_small_square_inside_triangle()
        {
            var triangle = JMeshFrameInstance.FromMeshAndTransform(JMeshPhysicsMeshes.triangleMeshIdentity, Matrix4x4.Scale(new Vector3(1, 1f, 2f)));
            var square = JMeshFrameInstance.FromMeshAndTransform(JMeshPhysicsMeshes.squareMeshIdentity, Matrix4x4.TRS(new Vector3(0, 0, 0.5f), Quaternion.identity, Vector3.one * 0.5f));
            Debug.Log("triangle.size: " + triangle.TransformedMesh.AABB.size);
            Debug.Log("triangles.vertices: " + Logging.AsString(triangle.TransformedMesh.EdgeVertices));
            Debug.Log("triangles.normals: " + Logging.AsString(triangle.TransformedMesh.EdgeOutwardNormals));


            Debug.Log("square.size: " + square.TransformedMesh.AABB.size);
            Debug.Log("square.vertices: " + Logging.AsString(square.TransformedMesh.EdgeVertices));
            Debug.Log("square.normals: " + Logging.AsString(square.TransformedMesh.EdgeOutwardNormals));
            var push = FindMinimumPushFromAToB(triangle.TransformedMesh, square.TransformedMesh);
            Debug.Log("pushMagnitude: " + push.Magnitude);
            var size = triangle.TransformedMesh.AABB.size;
            var triangleHypothenus = Mathf.Sqrt(size.x * size.x + size.z * size.z);
            var angle = Mathf.Acos(size.z / triangleHypothenus);

            var expecedPushMagnitude = Mathf.Sin(angle) * square.TransformedMesh.AABB.size.x;
            Assert.IsTrue(JMeshCollisionUtil.IsPointInsideMesh(square.TransformedMesh.EdgeVertices[1], triangle.TransformedMesh));
            TestMethods.AreEqualIsh(new Vector3(-2, 0, 1).normalized, push.Direction);
            TestMethods.AreEqualIsh(expecedPushMagnitude, push.Magnitude, POINT_MAX_DIFF);

        }
    }
}