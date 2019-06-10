using UnityEngine;

namespace Jerre.JPhysics
{

    public class PhysicsHelper : MonoBehaviour
    {
        public static bool IsPointInsideAABB(Vector3 point, Bounds bound, bool ignoreY)
        {
            bool yAxis = true;
            if (!ignoreY)
            {
                yAxis = bound.min.y <= point.y && point.y <= bound.max.y;
            }
            return (bound.min.x <= point.x && point.x <= bound.max.x) && yAxis && bound.min.z <= point.z && point.z <= bound.max.z;
        }

        public static float[] FindMinimumPushDistancesForPointInXZPlane(Vector3 point, Bounds bound)
        {
            return new float[]
            {
                point.z - bound.min.z,
                bound.max.x - point.x,
                bound.max.z - point.z,
                point.x - bound.min.x
            };
        }

        public static BoxDistances FindPushDistancesForPointInXZPlane(Vector3 point, Bounds bound)
        {
            return new BoxDistances(
                Mathf.Abs(point.z - bound.min.z),
                Mathf.Abs(bound.max.x - point.x),
                Mathf.Abs(bound.max.z - point.z),
                Mathf.Abs(point.x - bound.min.x)
            );
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
                ((aMin.x <= bMin.x && bMin.x <= aMax.x) && (aMin.z <= bMax.z && bMax.z <= aMax.z)) ;    //B's upper left corner inside A?
        }


        public static PushResult CalculateObjectSeparation(Bounds a, Bounds b)
        {
            if (HasBPointInsideA(a, b))
            {
                return new PushResult(true, CalculatePushDirectionFromAToB(a, b), a);
            } else if (HasBPointInsideA(b, a))
            {
                return new PushResult(true, CalculatePushDirectionFromAToB(b, a), b);
            }
            return new PushResult(false, new Push(Vector3.zero, 0f), a);
        }


        public static Push CalculatePushDirectionFromAToB(Bounds boxA, Bounds boxB)
        {
            var bMin = boxB.min;
            var bMax = boxB.max;

            //Starting with lower left corner
            Vector3[] boxBVertices = new Vector3[]
            {
                new Vector3(bMin.x, bMin.y, bMin.z),
                new Vector3(bMax.x, bMin.y, bMin.z),
                new Vector3(bMax.x, bMin.y, bMax.z),
                new Vector3(bMin.x, bMin.y, bMax.z)
            };

            BoxDistances[] distances = new BoxDistances[boxBVertices.Length];
            for (var i = 0; i < boxBVertices.Length; i++)
            {
                if (IsPointInsideAABB(boxBVertices[i], boxA, true))
                {
                    distances[i] = FindPushDistancesForPointInXZPlane(boxBVertices[i], boxA);
                }
                else
                {
                    distances[i] = new BoxDistances(-1, -1, -1, -1);
                }
            }

            var maxDistances = new float[] {
                MaxABDistance(distances),
                MaxBCDistance(distances),
                MaxCDDistance(distances),
                MaxDADistance(distances)
            };

            var indexOfSmallest = FindIndexOfSmallest(maxDistances);

            if (indexOfSmallest == 0)   // AB
            {
                return new Push(Vector3.back, maxDistances[indexOfSmallest]);
            } else if (indexOfSmallest == 1) // BC
            {
                return new Push(Vector3.right, maxDistances[indexOfSmallest]);
            } else if (indexOfSmallest == 2) // CD
            {
                return new Push(Vector3.forward, maxDistances[indexOfSmallest]);
            } else
            {
                return new Push(Vector3.left, maxDistances[indexOfSmallest]);
            }
        }

        public struct Push
        {
            public Vector3 Direction;
            public float Magnitude;

            public Push(Vector3 pushVector, float pushMagnitude)
            {
                Direction = pushVector;
                Magnitude = pushMagnitude;
            }

            public override string ToString()
            {
                return "{direction: " + logVector3(Direction) + ", magnitude: " + Magnitude + "}";
            }
        }

        public struct PushResult
        {
            public bool CanPush;
            public Push push;
            public Bounds PushSource;

            public PushResult(bool canPush, Push push, Bounds pushSource)
            {
                CanPush = canPush;
                this.push = push;
                PushSource = pushSource;
            }

            public override string ToString()
            {
                return "canPush: " + CanPush + ", push: " + push + ", pushSource: " + logVector3(PushSource.center);
            }
        }

        public static string arrToString(Vector3[] arr)
        {
            string concat = "[";
            for (var i = 0; i < arr.Length; i++)
            {
                if (i < arr.Length - 1)
                {
                    concat += logVector3(arr[i]) + ";";
                }
                else
                {
                    concat += logVector3(arr[i]);
                }
            }
            return concat + "]";
        }

        public static string logVector3(Vector3 vector)
        {
            return "{x: " + vector.x + ", y: " + vector.y + ", z: " + vector.z + "}";
        }

        public static string arrToString(float[] arr)
        {
            string concat = "[";
            for (var i = 0; i < arr.Length; i++)
            {
                if (i < arr.Length - 1)
                {
                    concat += arr[i] + ";";
                }
                else
                {
                    concat += arr[i];
                }
            }
            return concat + "]";
        }

        public static string arrToString(BoxDistances[] arr)
        {
            string concat = "[";
            for (var i = 0; i < arr.Length; i++)
            {
                if (i < arr.Length - 1)
                {
                    concat += arr[i] + ";";
                }
                else
                {
                    concat += arr[i];
                }
            }
            return concat + "]";
        }

        public struct BoxDistances
        {
            public float ab, bc, cd, da;

            public BoxDistances(float ab, float bc, float cd, float da)
            {
                this.ab = ab;
                this.bc = bc;
                this.cd = cd;
                this.da = da;
            }

            public override string ToString()
            {
                return "ab: " + ab + ", bc: " + bc + ", cd: " + cd + ", da: " + da;
            }
        }

        private static float MaxABDistance(BoxDistances[] boxDistances)
        {
            float currentMax = -1f;
            for (var i = 0; i < boxDistances.Length; i++)
            {
                if (boxDistances[i].ab > currentMax)
                {
                    currentMax = boxDistances[i].ab;
                }
            }
            return currentMax;
        }

        private static float MaxBCDistance(BoxDistances[] boxDistances)
        {
            float currentMax = -1f;
            for (var i = 0; i < boxDistances.Length; i++)
            {
                if (boxDistances[i].bc > currentMax)
                {
                    currentMax = boxDistances[i].bc;
                }
            }
            return currentMax;
        }

        private static float MaxCDDistance(BoxDistances[] boxDistances)
        {
            float currentMax = -1f;
            for (var i = 0; i < boxDistances.Length; i++)
            {
                if (boxDistances[i].cd > currentMax)
                {
                    currentMax = boxDistances[i].cd;
                }
            }
            return currentMax;
        }

        private static float MaxDADistance(BoxDistances[] boxDistances)
        {
            float currentMax = -1f;
            for (var i = 0; i < boxDistances.Length; i++)
            {
                if (boxDistances[i].da > currentMax)
                {
                    currentMax = boxDistances[i].da;
                }
            }
            return currentMax;
        }

        private static int FindIndexOfSmallest(float[] otherValues)
        {
            var smallest = otherValues[0];
            var indexOfSmallest = 0;
            for (var i = 1; i < otherValues.Length; i++)
            {
                if (otherValues[i] < smallest)
                {
                    smallest = otherValues[i];
                    indexOfSmallest = i;
                }
            }
            return indexOfSmallest;
        }

        private static int FindIndexOfLargest(float[] array)
        {
            var largest = array[0];
            var index = 0;
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i] > largest)
                {
                    index = i;
                    largest = array[i];
                }
            }
            return index;
        }
    }

}
