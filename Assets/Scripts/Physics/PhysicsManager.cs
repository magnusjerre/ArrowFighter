using Jerre.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.JPhysics
{
    public class PhysicsManager : MonoBehaviour
    {
        public static float AbsoluteMaxSpeed = 2500;
        public bool Debugbounds = false;
        public RectTransform DebugView;
        public Renderer themesh;

        // Use this for initialization
        void Start()
        {
            for (var i = 0; i < DebugView.childCount; i++)
            {
                DebugView.GetChild(i).gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            var allPhysicsBodies = GameObject.FindObjectsOfType<PhysicsbodyRectangular>();
            if (allPhysicsBodies.Length < 2)
            {
                return;
            }

            var bodies = new List<PhysicsbodyRectangular>();
            bodies.AddRange(allPhysicsBodies);

            if (Debugbounds)
            {
                for (var i = 0; i < DebugView.childCount; i++)
                {
                    DebugView.GetChild(i).gameObject.SetActive(false);
                }
            }

            var completedCollisionsThisFrame = new HashSet<CollisionPair>();
            var collisionMap = CollisionMap.GenerateMapFor(bodies, 2, CameraHelper.GetCameraWorldCoordinateBounds());
            var processQueue = new List<CollisionMap>();
            processQueue.Add(collisionMap);
            var index = 0;
            while (processQueue.Count > 0)
            {
                var current = processQueue[0];
                processQueue.RemoveAt(0);

                if (current.bodies != null)
                {
                    HandlePhysics(current.bodies.ToArray(), completedCollisionsThisFrame);
                } else
                {
                    processQueue.AddRange(current.subMaps);
                }
                if (Debugbounds && index < DebugView.childCount)
                {
                    var rect = DebugView.GetChild(index).GetComponent<RectTransform>();

                    CameraHelper.PaintBoundsOnScreen(current.bounds, rect);
                }
                index++;
            }
        }

        struct CollisionPair
        {
            public PhysicsbodyRectangular body1;
            public PhysicsbodyRectangular body2;

            public CollisionPair(PhysicsbodyRectangular body1, PhysicsbodyRectangular body2)
            {
                this.body1 = body1;
                this.body2 = body2;
            }

            public override bool Equals(object obj)
            {
                var pair = (CollisionPair)obj;
                return ((pair.body1 == body1 && pair.body2 == body2) || (pair.body1 == body2 && pair.body2 == body1));
            }

            public override int GetHashCode()
            {
                return EqualityComparer<PhysicsbodyRectangular>.Default.GetHashCode(body1) + EqualityComparer<PhysicsbodyRectangular>.Default.GetHashCode(body2);
            }
        }

        void HandlePhysics(PhysicsbodyRectangular[] allPhysicsBodies, HashSet<CollisionPair> completedCollisions)
        {
            for (var i = 0; i < allPhysicsBodies.Length - 1; i++)
            {
                for (var j = i + 1; j < allPhysicsBodies.Length; j++)
                {
                    var physicsObjA = allPhysicsBodies[i];
                    var physicsObjB = allPhysicsBodies[j];
                    var collisionPair = new CollisionPair(physicsObjA, physicsObjB);
                    if (completedCollisions.Contains(collisionPair))
                    {
                        Debug.Log("This collision has already been processed, moving onto next");
                        continue;
                    }
                    completedCollisions.Add(collisionPair);

                    if (physicsObjA.IsStationary && physicsObjB.IsStationary) continue;

                    var boundsA = physicsObjA.MeshRenderer.bounds;
                    var boundsB = physicsObjB.MeshRenderer.bounds;

                    var physicsResult = PhysicsHelper.CalculateObjectSeparation(boundsA, boundsB);
                    if (!physicsResult.CanPush) continue;

                    PhysicsbodyRectangular toPushFrom = physicsResult.PushSource == boundsA ? physicsObjA : physicsObjB;
                    PhysicsbodyRectangular toPush = physicsResult.PushSource == boundsA ? physicsObjB : physicsObjA;
                    Vector3 direction = physicsResult.push.Direction;

                    if (toPush == physicsObjA)    // Going to try to push A
                    {
                        if (physicsObjA.IsStationary)   //Must push A instead
                        {
                            toPushFrom = physicsObjA;
                            toPush = physicsObjB;
                            direction *= -1;
                        }
                    }
                    else // Going to try to push B
                    {
                        if (physicsObjB.IsStationary)   // Must push A instead
                        {
                            toPushFrom = physicsObjB;
                            toPush = physicsObjA;
                            direction *= -1;
                        }
                    }

                    toPush.transform.Translate(direction * physicsResult.push.Magnitude, Space.World);

                    var playerPhysics = toPush.GetComponent<PlayerPhysics>();
                    if (playerPhysics != null)
                    {
                        BouncePlayer(playerPhysics, direction, toPushFrom.SurfaceBounceFactor);
                    }
                }
            }
        }

        void BouncePlayer(PlayerPhysics playerPhysics, Vector3 surfaceNormal, float surfaceBounceFactor)
        {
            var dotProduct = Vector3.Dot(surfaceNormal, playerPhysics.MovementDirection);
            var bounceDirection = playerPhysics.MovementDirection - 2 * dotProduct * surfaceNormal;

            var resultingSpeedVector = bounceDirection * playerPhysics.Speed;
            var lengthOfResultingBoucneDirecitonOntoNormal = Vector3.Dot(surfaceNormal, resultingSpeedVector);
            var bounceSpeedInDirectionOfNormal = surfaceNormal * lengthOfResultingBoucneDirecitonOntoNormal * surfaceBounceFactor;
            var totalSpeedVector = resultingSpeedVector + bounceSpeedInDirectionOfNormal;

            playerPhysics.MovementDirection = totalSpeedVector.normalized;
            var oldSpeed = playerPhysics.Speed;
            playerPhysics.SetSpeed(totalSpeedVector.magnitude);
        }
    }
}
