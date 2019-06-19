using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestMethods
    {
        public const float VECTOR_DIFF = 0.00001f;

        public static void AreEqualIsh(Vector3 expected, Vector3 actual, float maxDiff)
        {
            Assert.IsTrue(Mathf.Abs(expected.x - actual.x) < maxDiff);
            Assert.IsTrue(Mathf.Abs(expected.y - actual.y) < maxDiff);
            Assert.IsTrue(Mathf.Abs(expected.z - actual.z) < maxDiff);
        }

        public static void AreEqualIsh(Vector3 expected, Vector3 actual)
        {
            AreEqualIsh(expected, actual, VECTOR_DIFF);
        }
    }
}
