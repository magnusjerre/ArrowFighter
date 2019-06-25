using UnityEngine;

namespace Jerre.JPhysics
{
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
            if (isVertical && Mathf.Abs(point.x - b) == 0f)
            {
                return true;
            }
            if (Mathf.Abs(CalculateY(point.x) - point.z) == 0f)
            {
                return true;
            }
            return false;
        }

        public float CalculateIntersection(Slope otherSlope)
        {
            return (otherSlope.b - b) / (a - otherSlope.a);
        }

        public float CalculateIntersectionWithHorizontalLine(float z)
        {
            return (z - b) / a;
        }
    }
}
