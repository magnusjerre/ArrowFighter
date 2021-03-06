﻿using Jerre.JPhysics;
using Jerre.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.JColliders
{
    public class JColliderManager : MonoBehaviour
    {
        public static QueueRerunnable<CollisionMap> collisionMapQueue = new QueueRerunnable<CollisionMap>();
        public static List<JCollisionPushPair> pushPairs = new List<JCollisionPushPair>();

        private EnterStayExitManager enterStayExitManager = new EnterStayExitManager();

        void LateUpdate()
        {
            pushPairs.Clear();
            collisionMapQueue.Clear();

            var allActiveJColliders = JColliderContainer.INSTANCE.ActiveColliders();
            var lengthAllActiveJColliders = allActiveJColliders.Count;
            if (lengthAllActiveJColliders == 0) return;

            // Update bounds, vertices and normals for each collider based on their transform
            for (int i = 0; i < lengthAllActiveJColliders; i++)
            {
                var collider = allActiveJColliders[i];
                collider.meshFrame = JMesh.FromMeshAndTransform(collider.meshIdentity, collider.meshFilter.transform.localToWorldMatrix);
            }

            var completedCollisionsThisFrame = new HashSet<JCollisionPair>();
            var collisionMap = CollisionMap.GenerateMapFor(allActiveJColliders, 4, CameraHelper.GetCameraWorldCoordinateBounds());
            collisionMapQueue.Add(collisionMap);
            while (collisionMapQueue.HasNext())
            {
                var currentCollisionMap = collisionMapQueue.Next();
                if (currentCollisionMap.bodies != null)
                {
                    DoCollisionChecks(currentCollisionMap.bodies, completedCollisionsThisFrame);
                }
                else
                {
                    collisionMapQueue.Add(currentCollisionMap.subMaps);
                }
            }

            enterStayExitManager.CompleteFrame();
        }

        private void DoCollisionChecks(List<JCollider> colliders, HashSet<JCollisionPair> completedCollisions)
        {
            var endOuter = colliders.Count - 1;
            var endInner = colliders.Count;
            for (int i = 0; i < endOuter; i++)
            {
                var colliderA = colliders[i];
                for (int j = i + 1; j < endInner; j++)
                {
                    var colliderB = colliders[j];

                    if (ShouldSkipCollisionCheck(colliderA, colliderB, completedCollisions))
                    {
                        continue;
                    }

                    enterStayExitManager.Add(new JCollisionPair(colliderA, colliderB));

                    if (colliderA.IsTrigger || colliderB.IsTrigger)
                    {
                        continue;
                    }

                    if (colliderA.IsStationary && colliderB.IsStationary)
                    {
                        continue;
                    }

                    var pushResult = JMeshOverlapPushUtil.CalculateMinimumPush(colliderA.meshFrame, colliderB.meshFrame);
                    HandlePushResult(pushResult, colliderA, colliderB);
                }
            }
        }

        private void HandlePushResult(Push pushResult, JCollider colliderA, JCollider colliderB)
        {
            if (colliderA.IsStationary || colliderB.IsStationary)
            {
                var pushable = colliderA.IsStationary ? colliderB : colliderA;
                var nonPushable = colliderA.IsStationary ? colliderA : colliderB;
                Push(pushable, nonPushable, pushResult.Direction, pushResult.Magnitude);
            }
            else
            {
                var halfMagnitude = pushResult.Magnitude / 2f;

                Push(colliderA, colliderB, pushResult.Direction, halfMagnitude);
                Push(colliderB, colliderA, pushResult.Direction, halfMagnitude);
            }
        }

        private void Push(JCollider pushable, JCollider pushingFrom, Vector3 pushDirection, float magnitude)
        {
            var centerDirection = pushable.meshFrame.AABB.center - pushingFrom.meshFrame.AABB.center;
            var pushScalarDirection = Vector3.Dot(pushDirection, centerDirection);
            pushScalarDirection /= Mathf.Abs(pushScalarDirection);

            var actualPushDirection = pushDirection * pushScalarDirection;
            var pushVector = actualPushDirection * magnitude;
            pushable.transform.Translate(pushVector, Space.World);
            pushPairs.Add(new JCollisionPushPair(pushable, pushingFrom, new Push(actualPushDirection, magnitude)));
        }

        // Modifies completedCollisions by always trying to add the new collision pair to the list
        private bool ShouldSkipCollisionCheck(JCollider colliderA, JCollider colliderB, HashSet<JCollisionPair> completedCollisions)
        {
            if (!JLayerMaskUtil.MaskCheck(JLayerMaskUtil.GetLayerMask(colliderA.jLayer), colliderB.jLayer))
            {
                return true;
            }

            var collisionPair = new JCollisionPair(colliderA, colliderB);
            if (!completedCollisions.Add(collisionPair))    // This is where the methods mutates completedCollisions
            {
                return true;
            }

            var boundsA = colliderA.meshFrame.AABB;
            var boundsB = colliderB.meshFrame.AABB;

            if (!JMeshOverlap.AABBOverlap(boundsA, boundsB))
            {
                return true;
            }

            var meshA = colliderA.meshFrame;
            var meshB = colliderB.meshFrame;
            if (!JMeshOverlap.MeshesOverlap(meshA, meshB))
            {
                return true;
            }
            return false;
        }

        public List<JCollider> FindOverlaps(JCollider colliderA)
        {
            var output = new List<JCollider>(10);
            var boundsA = colliderA.meshFrame.AABB;
            var meshA = colliderA.meshFrame;

            collisionMapQueue.ResetQueueIteration();
            while (collisionMapQueue.HasNext())
            {
                var map = collisionMapQueue.Next();
                if (map.bodies != null && JMeshOverlap.AABBOverlap(boundsA, map.bounds))
                {
                    for (var i = 0; i < map.bodies.Count; i++)
                    {
                        var colliderB = map.bodies[i];
                        if (output.Contains(colliderB)) continue;

                        if (JLayerMaskUtil.MaskCheck(JLayerMaskUtil.GetLayerMask(colliderA.jLayer), colliderB.jLayer) &&
                            JMeshOverlap.AABBOverlap(boundsA, colliderB.meshFrame.AABB) &&
                                JMeshOverlap.MeshesOverlap(meshA, colliderB.meshFrame))
                        {
                            output.Add(colliderB);
                        }
                    }
                }
            }

            return output;
        }
    }
}
