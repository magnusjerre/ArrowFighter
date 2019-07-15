using UnityEngine;

namespace Jerre.JMath
{
    public struct Intersection
    {
        public IntersectionType Type;
        public Vector3 Point;

        public Intersection(IntersectionType type, Vector3 point)
        {
            Type = type;
            Point = point;
        }
    }
}