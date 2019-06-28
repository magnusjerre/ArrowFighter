using UnityEngine;
using System.Collections;
using static Jerre.JPhysics.Intersection;

namespace Jerre.JPhysics
{
    public class JMeshOverlap
    {
        public const float POINT_MAX_DIFF = 0.00001f;

        public static bool MeshesOverlap(JMesh meshA, JMesh meshB)
        {
            if (!AABBOverlap(meshA.AABB, meshB.AABB)) return false;

            var aVertices = meshA.EdgeVertices;
            var aLength = meshA.EdgeVertices.Length - 1;
            var bVertices = meshB.EdgeVertices;
            var bLength = meshB.EdgeVertices.Length - 1;

            // Check for any edges crossing
            for (var i = 0; i < aLength; i++)
            {
                var aStart = aVertices[i];
                var aEnd = aVertices[i + 1];

                for (var j = 0; j < bLength; j++)
                {
                    var bStart = bVertices[j];
                    var bEnd = bVertices[j + 1];

                    //Debug.Log("i: " + i + ", j: " + j + ", aStart: " + aStart + ", aEnd: " + aEnd + ", bStart: " + bStart + ", bEnd: " + bEnd);
                    if (LinesCross(aStart, aEnd, bStart, bEnd))
                    {
                        return true;
                    }
                }
            }

            //Debug.Log("No lines crossed, looking for points inside each mesh");

            // Check if any vertices from meshA is inside meshB
            for (var i = 0; i < aLength; i++)
            {
                var pointA = aVertices[i];
                if (IsPointInsideMesh(pointA, meshB)) return true;
            }

            //Debug.Log("No points from meshA inside meshB");

            for (var i = 0; i < bLength; i++)
            {
                var pointB = bVertices[i];
                if (IsPointInsideMesh(pointB, meshA)) return true;
            }

            //Debug.Log("No points from meshB inside meshA");
            //Debug.Log("No mesh overlap");

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

        public static bool IsPointInsideMesh(Vector3 point, JMesh meshTransformed)
        {
            // Assumes that the point is withing the AABB
            var edgesCrossed = 0;
            var vertices = meshTransformed.EdgeVertices;
            var end = vertices.Length - 1;

            var horizontalPointSlope = Slope.FromPoints(point, new Vector3(point.x - 1, 0, point.z));
            var endOfRayCast = new Vector3(meshTransformed.AABB.min.x - 1f, 0, point.z);

            for (var i = 0; i < end; i++)
            {
                var pointA = vertices[i];
                var pointB = vertices[i + 1];

                /**
                 * Quick check to see if the point is exactly on a vertex.
                 * If it is, the point counts as being inside the mesh.
                 **/
                if (pointA.x == point.x && pointA.z == point.z)
                {
                    //Debug.Log("Point is exactly on an existing point, should return true");
                    return true;
                }

                var edgeSlope = Slope.FromPoints(pointA, pointB);
                if (edgeSlope.IsPointOnLineInXZPlane(point, POINT_MAX_DIFF))
                {
                    //Debug.Log("Point is on line in XZ plane, edge: " + (pointB - pointA));
                    return true;
                }

                var intersection = edgeSlope.CalculateIntersection(horizontalPointSlope);
                if (LinesCross(point, endOfRayCast, pointA, pointB))
                {
                    //Debug.Log("Crossed edge! " + (pointB - pointA));
                    edgesCrossed++;
                }


                //if ((pointA.z <= point.z && point.z <= pointB.z) || (pointB.z <= point.z && point.z <= pointA.z))
                //{
                //    // Can cross vertically
                //    if (pointA.x <= point.x || pointB.x <= point.x)
                //    {
                //        /** The edge starts or ends to the left of our point, 
                //         * meaning we cross the edge with our horizontal leftwards raycast 
                //         */

                //        /**
                //         * Check if point is on an edge. This must be done before the next type of check
                //         * that checks for a single point being added twice to edgesCrossed.
                //         **/
                //        var slope = Slope.FromPoints(pointA, pointB);
                //        if (slope.IsPointOnLineInXZPlane(point, POINT_MAX_DIFF))
                //        {
                //            return true;
                //        }

                //        /**
                //         * Need to perform an additional check so that we don't include points crossed twice.
                //         * If our point is on the exact same "axis" (x or z position) as our end-point, don't
                //         * count it again since its been crossed when it was on pointA. 
                //         * We only count point-crossing for pointA to avoid double-counting.
                //         */
                //        if (pointB.x == point.x && pointB.z == point.z)
                //        {
                //            continue;
                //        }

                //        var xIntersection = slope.CalculateIntersectionWithHorizontalLine(point.z);
                //        var minX = Mathf.Min(pointA.x, pointB.x);
                //        if (minX <= xIntersection && xIntersection <= point.x)
                //        {
                //            edgesCrossed++;
                //        }
                //    }
                //}
            }
            return edgesCrossed % 2 != 0;
        }

    }
}
