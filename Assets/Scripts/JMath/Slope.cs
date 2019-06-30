using UnityEngine;

namespace Jerre.JMath
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

        public float CalculateXIntersection(Slope otherSlope)
        {
            return (otherSlope.b - b) / (a - otherSlope.a);
        }

        public float CalculateIntersectionWithHorizontalLine(float z)
        {
            return (z - b) / a;
        }

        public static Slope FromPoints(Vector3 start, Vector3 end)
        {
            var numerator = end.z - start.z;
            var denominator = end.x - start.x;
            if (denominator == 0f)
            {
                return new Slope(true, start.x);
            }

            var slopeGradient = numerator / denominator;
            var slopeOffset = start.z - slopeGradient * start.x;
            return new Slope(slopeGradient, slopeOffset);
        }

        public Intersection CalculateIntersection(Slope otherSlope)
        {
            if (isVertical && otherSlope.isVertical)
            {
                return new Intersection(b == otherSlope.b ? IntersectionType.OVERLAP : IntersectionType.NONE, Vector3.zero);
            }

            if (isVertical || otherSlope.isVertical)
            {
                var vertical = isVertical ? this : otherSlope;
                var notVertical = isVertical ? otherSlope : this;

                return new Intersection(IntersectionType.INTERSECT, new Vector3(vertical.b, 0, notVertical.CalculateY(vertical.b)));
            }

            if (a == 0f && otherSlope.a == 0f)
            {
                return new Intersection(b == otherSlope.b ? IntersectionType.OVERLAP : IntersectionType.NONE, Vector3.zero);
            }

            if (a == 0f || otherSlope.a == 0f)
            {
                var horizontal = a == 0 ? this : otherSlope;
                var notHorizontal = a == 0 ? otherSlope : this;

                return new Intersection(IntersectionType.INTERSECT, new Vector3(notHorizontal.CalculateIntersectionWithHorizontalLine(horizontal.b), 0, horizontal.b));
            }

            if (a == otherSlope.a && b == otherSlope.b)
            {
                return new Intersection(IntersectionType.OVERLAP, Vector3.zero);
            }

            if (a == otherSlope.a && b != otherSlope.b)
            {
                return new Intersection(IntersectionType.NONE, Vector3.zero);
            }

            var x = CalculateXIntersection(otherSlope);
            var z = CalculateY(x);
            return new Intersection(IntersectionType.INTERSECT, new Vector3(x, 0, z));
        }
    }
}
