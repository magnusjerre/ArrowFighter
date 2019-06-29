using UnityEngine;
using NUnit.Framework;
using Jerre.JPhysics;
using Jerre.JColliders;

namespace Tests
{
    public class JMeshOverlapPushUtilTest
    {

        [Test]
        public void Calculate_minmumPushDistance_should_push_b_to_the_right()
        {
            var meshA = JMeshPhysicsMeshes.squareMeshIdentity;
            var meshB = JMesh.FromMeshAndTransform(JMeshPhysicsMeshes.triangleMeshIdentity, Matrix4x4.Translate(new Vector3(0.75f, 0, 0.5f)));

            var pushResult = JMeshOverlapPushUtil.CalculateMinimumPush(meshA, meshB);
            TestMethods.AreEqualIshOrOppositeIsh(new Vector3(1, 0, 0), pushResult.Direction);
            TestMethods.AreEqualIsh(0.25f, pushResult.Magnitude, TestMethods.VECTOR_DIFF);
        }

        [Test]
        public void Calculate_minmumPushDistance_should_push_b_up()
        {
            var meshA = JMeshPhysicsMeshes.squareMeshIdentity;
            var meshB = JMesh.FromMeshAndTransform(JMeshPhysicsMeshes.triangleMeshIdentity, Matrix4x4.Translate(new Vector3(0, 0, 0.55f)));

            var pushResult = JMeshOverlapPushUtil.CalculateMinimumPush(meshA, meshB);
            TestMethods.AreEqualIshOrOppositeIsh(new Vector3(0, 0, 1), pushResult.Direction);
            TestMethods.AreEqualIsh(0.45f, pushResult.Magnitude, TestMethods.VECTOR_DIFF);
        }

        [Test]
        public void Calculate_minmumPushDistance_should_push_b_left()
        {
            var meshA = JMeshPhysicsMeshes.squareMeshIdentity;
            var meshB = JMesh.FromMeshAndTransform(JMeshPhysicsMeshes.triangleMeshIdentity, Matrix4x4.Translate(new Vector3(-0.8f, 0, 0.25f)));

            var pushResult = JMeshOverlapPushUtil.CalculateMinimumPush(meshA, meshB);
            TestMethods.AreEqualIshOrOppositeIsh(new Vector3(-1, 0, 0), pushResult.Direction);
            TestMethods.AreEqualIsh(0.2f, pushResult.Magnitude, TestMethods.VECTOR_DIFF);
        }

        [Test]
        public void Calculate_minmumPushDistance_should_push_b_down()
        {
            var meshA = JMeshPhysicsMeshes.squareMeshIdentity;
            var meshB = JMesh.FromMeshAndTransform(JMeshPhysicsMeshes.triangleMeshIdentity, Matrix4x4.Translate(new Vector3(0, 0, -0.5f)));

            // Should push the square op to left, at 45 deg

            var pushResult = JMeshOverlapPushUtil.CalculateMinimumPush(meshA, meshB);
            TestMethods.AreEqualIshOrOppositeIsh(new Vector3(-1, 0, 1).normalized, pushResult.Direction);
            TestMethods.AreEqualIsh(Mathf.Sin(45 * Mathf.Deg2Rad) / 2f, pushResult.Magnitude, TestMethods.VECTOR_DIFF);
        }

        [Test]
        public void Calculate_minmumPushDistance_should_push_b_down_even_when_argument_order_is_reversed()
        {
            var meshA = JMeshPhysicsMeshes.squareMeshIdentity;
            var meshB = JMesh.FromMeshAndTransform(JMeshPhysicsMeshes.triangleMeshIdentity, Matrix4x4.Translate(new Vector3(0, 0, -0.5f)));

            var pushResult = JMeshOverlapPushUtil.CalculateMinimumPush(meshB, meshA);
            TestMethods.AreEqualIshOrOppositeIsh(new Vector3(-1, 0, 1).normalized, pushResult.Direction);
            TestMethods.AreEqualIsh(Mathf.Sin(45 * Mathf.Deg2Rad) / 2f, pushResult.Magnitude, TestMethods.VECTOR_DIFF);
        }

    }
}
