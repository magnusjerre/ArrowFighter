using Jerre.JColliders;
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

        void Start()
        {
            //for (var i = 0; i < DebugView.childCount; i++)
            //{
            //    DebugView.GetChild(i).gameObject.SetActive(false);
            //}
        }

        void LateUpdate()
        {
            var pushPairs = JColliderManager.pushPairs;
            var pushPairsCount = pushPairs.Count;

            for (int i = 0; i < pushPairsCount; i++)
            {
                var pushable = pushPairs[i].pushable;
                var pushingFrom = pushPairs[i].pushingFrom;
                var push = pushPairs[i].push;

                var pushingFromPhysicsBody = pushingFrom.GetComponent<JPhysicsBody>();

                var playerPhysics = pushable.GetComponent<PlayerPhysics>();
                if (playerPhysics != null && pushingFromPhysicsBody != null)
                {
                    BouncePlayer(playerPhysics, push.Direction, pushingFromPhysicsBody.SurfaceBounceFactor);
                }

                var bulletPhysics = pushable.GetComponent<BulletMover>();
                if (bulletPhysics != null && pushingFromPhysicsBody != null)
                {
                    BounceBullet(bulletPhysics, push.Direction, pushingFromPhysicsBody.SurfaceBounceFactor);
                }
            }
            //var allPhysicsBodies = GameObject.FindObjectsOfType<JPhysicsBody>();
            //if (allPhysicsBodies.Length < 1)
            //{
            //    return;
            //}

            //var bodies = new List<JPhysicsBody>(allPhysicsBodies.Length);
            //var allPBLength = allPhysicsBodies.Length;
            //for (var i = 0; i < allPBLength; i++)
            //{
            //    var body = allPhysicsBodies[i];
            //    if (body.enabled)
            //    {   // Update physics body, and add it to the list
            //        body.jMeshFrameInstance = JMesh.FromMeshAndTransform(body.jMeshIdentity, body.meshFilter.transform.localToWorldMatrix);
            //        bodies.Add(body);
            //    }
            //}

            //if (Debugbounds)
            //{
            //    for (var i = 0; i < DebugView.childCount; i++)
            //    {
            //        DebugView.GetChild(i).gameObject.SetActive(false);
            //    }
            //}

            //var completedCollisionsThisFrame = new HashSet<CollisionPair>();
            //var collisionMap = CollisionMap.GenerateMapFor(bodies, 2, CameraHelper.GetCameraWorldCoordinateBounds());
            //var processQueue = new List<CollisionMap>();
            //processQueue.Add(collisionMap);
            //var index = 0;
            //while (processQueue.Count > 0)
            //{
            //    var current = processQueue[0];
            //    processQueue.RemoveAt(0);

            //    if (current.bodies != null)
            //    {
            //        HandlePhysics(current.bodies.ToArray(), completedCollisionsThisFrame);
            //    }
            //    else
            //    {
            //        processQueue.AddRange(current.subMaps);
            //    }
            //    if (Debugbounds && index < DebugView.childCount)
            //    {
            //        var rect = DebugView.GetChild(index).GetComponent<RectTransform>();

            //        CameraHelper.PaintBoundsOnScreen(current.bounds, rect);
            //    }
            //    index++;
            //}
        }

        struct CollisionPair
        {
            public JPhysicsBody body1;
            public JPhysicsBody body2;

            public CollisionPair(JPhysicsBody body1, JPhysicsBody body2)
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
                return EqualityComparer<JPhysicsBody>.Default.GetHashCode(body1) + EqualityComparer<JPhysicsBody>.Default.GetHashCode(body2);
            }
        }

        void LogForPlayerCollisionsOnly(string message, JPhysicsBody body1, JPhysicsBody body2)
        {
            if (body1.jLayer == JLayer.PLAYER || body2.jLayer == JLayer.PLAYER)
            {
                Debug.Log(message);
            }
        }

        void HandlePhysics(JPhysicsBody[] allPhysicsBodies, HashSet<CollisionPair> completedCollisions)
        {
            for (var i = 0; i < allPhysicsBodies.Length - 1; i++)
            {
                for (var j = i + 1; j < allPhysicsBodies.Length; j++)
                {
                    var physicsObjA = allPhysicsBodies[i];
                    var physicsObjB = allPhysicsBodies[j];

                    if (!JLayerMaskUtil.MaskCheck(JLayerMaskUtil.GetLayerMask(physicsObjA.jLayer), physicsObjB.jLayer))
                    {
                        LogForPlayerCollisionsOnly("Shouldn't handle collisions between these objects: " + physicsObjA.gameObject.name + " : " + physicsObjB.gameObject.name, physicsObjA, physicsObjB);
                        continue;
                    }

                    var collisionPair = new CollisionPair(physicsObjA, physicsObjB);
                    if (completedCollisions.Contains(collisionPair))
                    {
                        //LogForPlayerCollisionsOnly("This collision has already been processed, moving onto next", physicsObjA, physicsObjB);
                        continue;
                    }
                    completedCollisions.Add(collisionPair);

                    if (physicsObjA.IsStationary && physicsObjB.IsStationary) continue;

                    var boundsA = physicsObjA.jMeshFrameInstance.AABB;
                    var boundsB = physicsObjB.jMeshFrameInstance.AABB;

                    if (!JMeshCollisionUtil.Intersect(boundsA, boundsB)) continue;

                    if (!JMeshOverlap.MeshesOverlap(physicsObjA.jMeshFrameInstance, physicsObjB.jMeshFrameInstance)) continue;

                    var pushResult = JMeshOverlapPushUtil.CalculateMinimumPush(physicsObjA.jMeshFrameInstance, physicsObjB.jMeshFrameInstance);

                    if (physicsObjA.IsStationary)
                    {
                        var centerDirection = physicsObjB.jMeshFrameInstance.AABB.center - physicsObjA.jMeshFrameInstance.AABB.center;
                        var pushScalarDirection = Vector3.Dot(pushResult.Direction, centerDirection);
                        pushScalarDirection /= Mathf.Abs(pushScalarDirection);
                        var pushVector = pushResult.Direction * pushResult.Magnitude * pushScalarDirection;
                        physicsObjB.transform.Translate(pushVector, Space.World);
                        var player = physicsObjB.GetComponent<PlayerPhysics>();
                        if (player != null)
                        {
                            BouncePlayer(player, pushResult.Direction * pushScalarDirection, physicsObjA.SurfaceBounceFactor);
                        }
                        var bulletMover = physicsObjB.GetComponent<BulletMover>();
                        if (bulletMover != null)
                        {
                            BounceBullet(bulletMover, pushResult.Direction * pushScalarDirection, physicsObjA.SurfaceBounceFactor);
                        }
                    }
                    else if (physicsObjB.IsStationary)
                    {
                        var centerDirection = physicsObjA.jMeshFrameInstance.AABB.center - physicsObjB.jMeshFrameInstance.AABB.center;
                        var pushScalarDirection = Vector3.Dot(pushResult.Direction, centerDirection);
                        pushScalarDirection /= Mathf.Abs(pushScalarDirection);
                        var pushVector = pushResult.Direction * pushResult.Magnitude * pushScalarDirection;
                        physicsObjA.transform.Translate(pushVector, Space.World);
                        var player = physicsObjA.GetComponent<PlayerPhysics>();
                        if (player != null)
                        {
                            BouncePlayer(player, pushResult.Direction * pushScalarDirection, physicsObjB.SurfaceBounceFactor);
                        }
                        var bulletMover = physicsObjA.GetComponent<BulletMover>();
                        if (bulletMover != null)
                        {
                            BounceBullet(bulletMover, pushResult.Direction * pushScalarDirection, physicsObjB.SurfaceBounceFactor);
                        }
                    }
                    else
                    {
                        var centerAToB = physicsObjB.jMeshFrameInstance.AABB.center - physicsObjA.jMeshFrameInstance.AABB.center;
                        var pushScalarDirectionAToB = Vector3.Dot(pushResult.Direction, centerAToB);
                        var pushVectorAToB = pushResult.Direction * (pushResult.Magnitude / 2f) * pushScalarDirectionAToB;
                        physicsObjA.transform.Translate(pushVectorAToB, Space.World);
                        
                        var centerBToA = physicsObjA.jMeshFrameInstance.AABB.center - physicsObjB.jMeshFrameInstance.AABB.center;
                        var pushScalarDirectionBToA = Vector3.Dot(pushResult.Direction, centerBToA);
                        var pushVectorBToA = pushResult.Direction * (pushResult.Magnitude / 2f) * pushScalarDirectionBToA;
                        physicsObjB.transform.Translate(pushVectorBToA, Space.World);

                        // Not including bounce at this time, maybe later...
                    }

                    var aJCollision = physicsObjA.GetComponent<JCollider>();
                    var bJCollision = physicsObjB.GetComponent<JCollider>();
                    if (aJCollision != null)
                    {
                        aJCollision.OnJCollsionStay(bJCollision);
                    }
                    if (bJCollision != null)
                    {
                        bJCollision.OnJCollsionStay(aJCollision);
                    }
                    /*
                    var physicsResult = JMeshCollisionUtil.CalculateObjectSeparation(
                        physicsObjA.jMeshFrameInstance, 
                        physicsObjB.jMeshFrameInstance
                    );
                    if (!physicsResult.CanPush) continue;
                    Debug.Log("PD: result.APushB: " + physicsResult.APushB);
                    Debug.Log("PD::i: " + i + ", j: " + j + ", PushDirection: " + physicsResult.push.Direction + ", magnitude: " + physicsResult.push.Magnitude + ", time: " + Time.time);
                    Debug.Log("PD:: name: " + physicsObjA.name + ", position: " + physicsObjA.transform.position + "; name: " + physicsObjB.name + ", posiion: " + physicsObjB.transform.position);
                    Debug.Log("PD:: bodyA edge vertices" + Logging.AsString(physicsObjA.jMeshFrameInstance.EdgeVertices));
                    Debug.Log("PD:: bodyA.triangles. " + Logging.AsString(physicsObjA.mesh.triangles));
                    Debug.Log("PD:: bodyB edge vertices" + Logging.AsString(physicsObjB.jMeshFrameInstance.EdgeVertices));
                    Debug.Log("PD:: bodyB.triangles. " + Logging.AsString(physicsObjB.mesh.triangles));




                    // TODO:  Det er noe veldig rart med visuell størrelse og størrelsen på bounding boxes. Det ser nå ut til at bouning box ikke håndteres riktig. 
                    // Mulig jeg nå også må sørge for å støtte alle tre koordinater så jeg ikke får stusselige feil relatert til at jeg kun bruker X og Z.
                    // Kanskje jeg skal vurdere å endre fra XZ-plane til XY-plane. Tror det er det som gjøres i 2D unity-spill.









                    PhysicsbodyRectangular toPushFrom = physicsResult.APushB ? physicsObjA : physicsObjB;
                    PhysicsbodyRectangular toPush = physicsResult.APushB ? physicsObjB : physicsObjA;
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
                    var bulletMover = toPush.GetComponent<BulletMover>();
                    if (bulletMover != null)
                    {
                        BounceBullet(bulletMover, direction, toPushFrom.SurfaceBounceFactor);
                    }
                    var aJCollision = toPush.GetComponent<JCollision>();
                    if (aJCollision != null)
                    {
                        aJCollision.OnJCollsion(toPushFrom);
                    }
                    var bJCollision = toPushFrom.GetComponent<JCollision>();
                    if (bJCollision != null)
                    {
                        bJCollision.OnJCollsion(toPush);
                    }
                    */
                }
            }
        }

        void BouncePlayer(PlayerPhysics playerPhysics, Vector3 surfaceNormal, float surfaceBounceFactor)
        {
            var dotProduct = Vector3.Dot(surfaceNormal, playerPhysics.MovementDirection);
            var bounceDirection = playerPhysics.MovementDirection - 2 * dotProduct * surfaceNormal;

            var resultingSpeedVector = bounceDirection * playerPhysics.Speed;
            var lengthOfResultingBounceDirecitonOntoNormal = Vector3.Dot(surfaceNormal, resultingSpeedVector);
            var bounceSpeedInDirectionOfNormal = surfaceNormal * lengthOfResultingBounceDirecitonOntoNormal * surfaceBounceFactor;
            var totalSpeedVector = resultingSpeedVector + bounceSpeedInDirectionOfNormal;

            playerPhysics.MovementDirection = totalSpeedVector.normalized;
            var oldSpeed = playerPhysics.Speed;
            playerPhysics.SetSpeed(totalSpeedVector.magnitude);
            playerPhysics.transform.Translate(playerPhysics.MovementDirection * playerPhysics.Speed * Time.deltaTime, Space.World); // HACKY whacky, should probably handle this in some other way?
        }

        void BounceBullet(BulletMover bulletMover, Vector3 surfaceNormal, float surfaceBounceFactor)
        {
            var dotProduct = Vector3.Dot(surfaceNormal, bulletMover.transform.forward);
            var bounceDirection = bulletMover.transform.forward - 2 * dotProduct * surfaceNormal;

            var resultingSpeedVector = bounceDirection * bulletMover.Speed;
            var lengthOfResultingBounceDirectionOntoNormal = Vector3.Dot(surfaceNormal, resultingSpeedVector);
            var bounceSpeedInDirectionOfNormal = surfaceNormal * lengthOfResultingBounceDirectionOntoNormal * surfaceBounceFactor;
            var totalSpeedVector = resultingSpeedVector + bounceSpeedInDirectionOfNormal;

            bulletMover.transform.LookAt(bulletMover.transform.position + totalSpeedVector.normalized);
            bulletMover.Speed = totalSpeedVector.magnitude;
            bulletMover.transform.Translate(Vector3.forward * bulletMover.Speed * Time.deltaTime * 0.5f);   // HACKY whacky, doesn't look very good in game...
        }
    }
}
