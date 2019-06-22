using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class TestMethods
    {
        public const float VECTOR_DIFF = 0.00001f;

        public static void AreEqualIsh(Vector3 expected, Vector3 actual, float maxDiff, string errorPrefix)
        {
            var failed = false;
            string errorString = errorPrefix != null ? errorPrefix + "\n" : "";
            if (Mathf.Abs(expected.x - actual.x) > maxDiff)
            {
                failed = true;
                errorString += "exp.x: " + expected.x + ", act.x: " + actual.x + "\n";
            }
            if (Mathf.Abs(expected.y - actual.y) > maxDiff)
            {
                failed = true;
                errorString += "exp.y: " + expected.y + ", act.y: " + actual.y + "\n";
            }
            if (Mathf.Abs(expected.z - actual.z) > maxDiff)
            {
                failed = true;
                errorString += "exp.z: " + expected.z + ", act.z: " + actual.z + "\n";
            }
            if (failed)
            {
                throw new AssertionException(errorString);
            }
        }

        public static void AreEqualIsh(Vector3 expected, Vector3 actual)
        {
            AreEqualIsh(expected, actual, VECTOR_DIFF, "");
        }

        public static void AreEqualIsh(Vector3 expected, Vector3 actual, string errorPrefix)
        {
            AreEqualIsh(expected, actual, VECTOR_DIFF, errorPrefix);
        }

        public static void AreEqualIsh(float expected, float actual, float maxDiff)
        {
            if (Mathf.Abs(expected - actual) > maxDiff)
            {
                throw new AssertionException("Expected: " + expected + ", actual: " + actual);
            }
        }

        public static void AreEqualIsh(float[] expected, float[] actual, float maxDiff)
        {
            if (expected.Length != actual.Length)
            {
                throw new AssertionException("Different array lengths");
            }

            for (var i = 0; i < expected.Length; i++) {

                if (Mathf.Abs(expected[i] - actual[i]) > maxDiff)
                {
                    throw new AssertionException("expected[" + i + "]: " + expected[i] + ", actual[" + i + "]: " + actual[i] + ". Maxdiff: " + maxDiff);
                }
            }
        }
    }
}
