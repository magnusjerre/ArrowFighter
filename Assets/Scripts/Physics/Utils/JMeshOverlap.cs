using UnityEngine;
using System.Collections;
using static Jerre.JPhysics.Intersection;

namespace Jerre.JPhysics
{
    public class JMeshOverlap
    {
        public static bool MeshesOverlap(JMesh meshA, JMesh meshB)
        {
            if (!AABBOverlap(meshA.AABB, meshB.AABB)) return false;

            var aVertices = meshA.EdgeVertices;
            var aLength = meshA.EdgeVertices.Length - 1;
            var bVertices = meshB.EdgeVertices;
            var bLength = meshB.EdgeVertices.Length - 1;
            for (var i = 0; i < aLength; i++)
            {
                var aStart = aVertices[i];
                var aEnd = aVertices[i + 1];

                for (var j = 0; j < bLength; j++)
                {
                    var bStart = bVertices[j];
                    var bEnd = bVertices[j + 1];

                    Debug.Log("i: " + i + ", j: " + j + ", aStart: " + aStart + ", aEnd: " + aEnd + ", bStart: " + bStart + ", bEnd: " + bEnd);
                    if (LinesCross(aStart, aEnd, bStart, bEnd))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool AABBOverlap(Bounds a, Bounds b)
        {
            var aMin = a.min;
            var aMax = a.max;
            var bMin = b.min;
            var bMax = b.max;


            return !(
                    (aMax.x < bMin.x || bMax.x < aMin.x) ||     // Other is to the right
                    (aMax.z < bMin.z || bMax.z < aMin.z)     // Other is above
                );
        }

        public static bool LinesCross(Vector3 aStart, Vector3 aEnd, Vector3 bStart, Vector3 bEnd)
        {
            var slopeA = Slope.FromPoints(aStart, aEnd);
            var slopeB = Slope.FromPoints(bStart, bEnd);
            var intersection = slopeA.CalculateIntersection(slopeB);

            if (intersection.Type == IntersectionType.NONE) return false;
            if (intersection.Type == IntersectionType.OVERLAP) return true;

            var p = intersection.Point;
            var minAX = Mathf.Min(aStart.x, aEnd.x);
            var maxAX = Mathf.Max(aStart.x, aEnd.x);
            var minAZ = Mathf.Min(aStart.z, aEnd.z);
            var maxAZ = Mathf.Max(aStart.z, aEnd.z);

            if (p.x < minAX || p.x > maxAX || p.z < minAZ || p.z > maxAZ) return false;

            var minBX = Mathf.Min(bStart.x, bEnd.x);
            var maxBX = Mathf.Max(bStart.x, bEnd.x);
            var minBZ = Mathf.Min(bStart.z, bEnd.z);
            var maxBZ = Mathf.Max(bStart.z, bEnd.z);
            return !(p.x < minBX || p.x > maxBX || p.z < minBZ || p.z > maxBZ);
        }

    }
}
