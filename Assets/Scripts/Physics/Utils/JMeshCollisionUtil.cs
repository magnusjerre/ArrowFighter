using System.Collections.Generic;
using UnityEngine;

namespace Jerre.JPhysics
{
    public class JMeshCollisionUtil
    {
        public const float POINT_MAX_DIFF = 0.00001f;

        public static PushResult CalculateObjectSeparation(JMesh meshA, JMesh meshB)
        {
            if (HasPointInsideOtherMesh(meshB, meshA))
            {
                return new PushResult(true, FindMinimumPushFromAToB(meshA, meshB), true);
            }
            else if (HasPointInsideOtherMesh(meshA, meshB))
            {
                return new PushResult(true, FindMinimumPushFromAToB(meshB, meshA), false);
            }
            else
            {
                Debug.Log("PD:: no pushing");
                return new PushResult(false, new Push(Vector3.zero, 0f), false);
            }
        }

        public static bool HasPointInsideOtherMesh(JMesh mesh, JMesh otherMesh)
        {
            var vertices = mesh.EdgeVertices;
            var length = vertices.Length;
            for (var i = 0; i < length; i++)
            {
                if (IsPointInsideMesh(vertices[i], otherMesh)) return true;
            }
            return false;
        }

        public static bool IsPointInsideMesh(Vector3 point, JMesh meshTransformed)
        {
            // Assumes that the point is withing the AABB
            var edgesCrossed = 0;
            var vertices = meshTransformed.EdgeVertices;
            var end = vertices.Length - 1;

            var edgeCount = 0;
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
                    Debug.Log("Point is exactly on an existing point, should return true");
                    return true;
                }

                

                if ((pointA.z <= point.z && point.z <= pointB.z) || (pointB.z <= point.z && point.z <= pointA.z))
                {
                    // Can cross vertically
                    if (pointA.x <= point.x || pointB.x <= point.x)
                    {
                        /** The edge starts or ends to the left of our point, 
                         * meaning we cross the edge with our horizontal leftwards raycast 
                         */

                        /**
                         * Check if point is on an edge. This must be done before the next type of check
                         * that checks for a single point being added twice to edgesCrossed.
                         **/
                        var slope = CalculateSlope(pointA, pointB);
                        if (slope.IsPointOnLineInXZPlane(point, POINT_MAX_DIFF))
                        {
                            return true;
                        }

                        /**
                         * Need to perform an additional check so that we don't include points crossed twice.
                         * If our point is on the exact same "axis" (x or z position) as our end-point, don't
                         * count it again since its been crossed when it was on pointA. 
                         * We only count point-crossing for pointA to avoid double-counting.
                         */
                        if (pointB.x == point.x || pointB.z == point.z)
                        {
                            continue;
                        }

                        var xIntersection = slope.CalculateIntersectionWithHorizontalLine(point.z);
                        var minX = Mathf.Min(pointA.x, pointB.x);
                        if (minX <= xIntersection && xIntersection <= point.x)
                        {
                            edgesCrossed++;
                        }

                        //Debug.Log("pointA: " + pointA);
                        //Debug.Log("PointB: " + pointB);
                        //Debug.Log("point: " + point);
                        //edgesCrossed++;
                    }
                }
                edgeCount++;
            }

            //Debug.Log("edges crossed: " + edgesCrossed);
            return edgesCrossed % 2 != 0;
        }

        public static Slope CalculateSlope(Vector3 pointA, Vector3 pointB)
        {
            var numerator = pointB.z - pointA.z;
            var denominator = pointB.x - pointA.x;
            if (denominator == 0f)
            {
                return new Slope(true, pointA.x);
            }

            var slopeGradient = numerator / denominator;
            var slopeOffset = pointA.z - slopeGradient * pointA.x;
            return new Slope(slopeGradient, slopeOffset);
        }

        public static bool IsPointInsideAABB(Vector3 point, Bounds bound)
        {
            // Ignores y-axis
            return (bound.min.x <= point.x && point.x <= bound.max.x) && bound.min.z <= point.z && point.z <= bound.max.z;
        }

        public static Push FindMinimumPushFromAToB(JMesh meshA, JMesh meshB)
        {
            var meshBVertices = meshB.EdgeVertices;
            var meshBLength = meshBVertices.Length;

            var pushDistances = new List<float[]>(meshBLength);
            for (var i = 0; i < meshBLength; i++)
            {
                var vertex = meshBVertices[i];
                if (!IsPointInsideAABB(vertex, meshA.AABB) || !IsPointInsideMesh(vertex, meshA))
                {
                    continue;
                }

                /**
                 * Calculate the push distances in each normal direction
                 */
                pushDistances.Add(CalculatePushLengthsForPoint(meshA, vertex));
            }

            var maxDistances = FindMaxesForEachValue(pushDistances);
            var indexOfSmallest = IndexOfSmallestValue(maxDistances);
            return new Push(meshA.EdgeOutwardNormals[indexOfSmallest], maxDistances[indexOfSmallest]);
        }

        public static float[] CalculatePushLengthsForPoint(JMesh meshA, Vector3 vertex)
        {
            var aVertices = meshA.EdgeVertices;
            var aNormals = meshA.EdgeOutwardNormals;
            var aNormalsLength = aNormals.Length;
            var pushLengths = new float[aNormalsLength];
            for (var a = 0; a < aNormalsLength; a++)
            {
                var PAVector = (aVertices[a] - vertex);
                var normal = aNormals[a];
                var distance = Mathf.Abs(Vector3.Dot(normal, PAVector));
                pushLengths[a] = distance;
                //Debug.Log("vertex: " + vertex + ", PAVector: " + PAVector + ", normal: " + normal + ", distance: " + distance);
            }

            return pushLengths;
        }

        public static float[] FindMaxesForEachValue(List<float[]> directionMagnitudes)
        {
            float[] currentMaxes = new float[directionMagnitudes[0].Length];
            for (var i = 0; i < currentMaxes.Length; i++)
            {
                currentMaxes[i] = float.MinValue;
            }

            for (var i = 0; i < directionMagnitudes.Count; i++)
            {
                var magnitudes = directionMagnitudes[i];
                for (var j = 0; j < magnitudes.Length; j++)
                {
                    if (magnitudes[j] > currentMaxes[j])
                    {
                        currentMaxes[j] = magnitudes[j];
                    }
                }
            }

            return currentMaxes;
        }

        public static int IndexOfSmallestValue(float[] values)
        {
            var indexOfSmallest = 0;
            var smallestValue = values[indexOfSmallest];
            for (var i = 1; i < values.Length; i++)
            {
                if (values[i] < smallestValue)
                {
                    smallestValue = values[i];
                    indexOfSmallest = i;
                }
            }

            return indexOfSmallest;
        }

        public static bool Intersect(Bounds a, Bounds b)
        {
            return HasBPointInsideA(a, b) || HasBPointInsideA(b, a) || BoundsOverlapWithoutPointsInside(a, b) || BoundsOverlapWithoutPointsInside(b, a);
        }

        public static bool BoundsOverlapWithoutPointsInside(Bounds a, Bounds b)
        {
            var aMin = a.min;
            var aMax = a.max;
            var bMin = b.min;
            var bMax = b.max;

            return (
                    (bMin.x < aMin.x && aMax.x < bMax.x) &&     // Is B wider than A?
                    (aMin.z <= bMin.z && bMin.z < aMax.z) &&    // Is B bottom line inside A?
                    (aMin.z <= bMax.z && bMax.z < aMax.z)       // Is B top line inside A?
                );
        }

        public static bool HasBPointInsideA(Bounds a, Bounds b)   //Ignore y-axis, only checks that B is inside A, not the other way around
        {
            var aMin = a.min;
            var aMax = a.max;
            var bMin = b.min;
            var bMax = b.max;

            return
                ((aMin.x <= bMin.x && bMin.x <= aMax.x) && (aMin.z <= bMin.z && bMin.z <= aMax.z)) ||   //B's lower left corner inside A?
                ((aMin.x <= bMax.x && bMax.x <= aMax.x) && (aMin.z <= bMin.z && bMin.z <= aMax.z)) ||   //B's lower right corner inside A?
                ((aMin.x <= bMax.x && bMax.x <= aMax.x) && (aMin.z <= bMax.z && bMax.z <= aMax.z)) ||   //B's upper right corner inside A?
                ((aMin.x <= bMin.x && bMin.x <= aMax.x) && (aMin.z <= bMax.z && bMax.z <= aMax.z));    //B's upper left corner inside A?
        }
    }
}
