using UnityEngine;

namespace Jerre.JPhysics
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

        public enum IntersectionType
        {
            INTERSECT, OVERLAP, NONE
        }
    }
}