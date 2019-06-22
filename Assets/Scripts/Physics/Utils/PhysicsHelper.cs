using UnityEngine;

namespace Jerre.JPhysics
{

    public class PhysicsHelper
    {
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
                ((aMin.x <= bMin.x && bMin.x <= aMax.x) && (aMin.z <= bMax.z && bMax.z <= aMax.z)) ;    //B's upper left corner inside A?
        }
    }
}
