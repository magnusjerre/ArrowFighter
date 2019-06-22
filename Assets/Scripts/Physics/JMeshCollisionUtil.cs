using UnityEngine;

namespace Jerre.JPhysics
{
    public class JMeshCollisionUtil : MonoBehaviour
    {
        public const float POINT_MAX_DIFF = 0.00001f;
        public static bool IsPointInsideMesh(Vector3 point, JMesh meshTransformed)
        {
            // Assumes that the point is withing the AABB
            var edgesCrossed = 0;
            var vertices = meshTransformed.EdgeVertices;
            var end = vertices.Length - 1;
            Debug.Log("end: " + end);

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
                        if (CalculateSlope(pointA, pointB).IsPointOnLineInXZPlane(point, POINT_MAX_DIFF))
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

                        edgesCrossed++;
                    }
                }
                edgeCount++;
            }

            Debug.Log("IsPointInsideMesh? edgeCount: " + edgeCount + ", edgesCrossed: " + edgesCrossed);
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

        public struct Slope
        {
            public float a;
            public float b;

            public bool isVertical;

            public Slope(float a, float b)
            {
                this.a = a;
                this.b = b;
                isVertical = false;
            }

            public Slope(bool isVertical, float x)
            {
                this.isVertical = isVertical;
                this.b = x;
                this.a = Mathf.Infinity;
            }

            public float CalculateY(float x)
            {
                return a * x + b;
            }

            // Ignores Y-axis of Vector3
            public bool IsPointOnLineInXZPlane(Vector3 point, float maxDiff)
            {
                if (isVertical && Mathf.Abs(point.x - b) < maxDiff)
                {
                    return true;
                }
                if (Mathf.Abs(CalculateY(point.x) - point.z) < maxDiff)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
