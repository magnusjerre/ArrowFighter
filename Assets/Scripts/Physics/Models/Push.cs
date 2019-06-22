using UnityEngine;

namespace Jerre.JPhysics
{
    public struct Push
    {
        public Vector3 Direction;
        public float Magnitude;

        public Push(Vector3 direction, float magnitude)
        {
            Direction = direction;
            Magnitude = magnitude;
        }
    }
}
