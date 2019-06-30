using Jerre.JPhysics;
using System.Collections.Generic;
using UnityEngine;

namespace Jerre.JColliders
{
    public class CollisionMap
    {
        public List<JCollider> bodies;
        public Bounds bounds;
        public List<CollisionMap> subMaps;

        public static float MinBoundsWidth = 128f;

        private CollisionMap()
        {
            
        }

        public static CollisionMap GenerateMapFor(List<JCollider> bodies, int maxNumberOfElements, Bounds bounds)
        {
            var rootCollisionMap = new CollisionMap();

            if (bodies.Count <= maxNumberOfElements || bounds.size.x <= MinBoundsWidth)
            {
                rootCollisionMap.bodies = new List<JCollider>(bodies);
                rootCollisionMap.bounds = bounds;
            }
            else
            {
                var newXSize = bounds.size.x / 2;

                var boundsChildA = new Bounds(
                    new Vector3(newXSize / 2 + bounds.min.x, 0, bounds.center.z), 
                    new Vector3(newXSize, 0, bounds.size.z));

                var boundsChildB = new Bounds(
                    new Vector3(newXSize / 2 + bounds.center.x, 0, bounds.center.z),
                    new Vector3(newXSize, 0, bounds.size.z));

                List<JCollider> childABodies = new List<JCollider>();
                List<JCollider> childBBodies = new List<JCollider>();

                for (var i = 0; i < bodies.Count; i++)
                {
                    var body = bodies[i];
                    if (JMeshOverlap.AABBOverlap(boundsChildA, body.meshFrame.AABB))
                    {
                        childABodies.Add(body);
                    }
                    if (JMeshOverlap.AABBOverlap(boundsChildB, body.meshFrame.AABB))
                    {
                        childBBodies.Add(body);
                    }
                }

                rootCollisionMap.subMaps = new List<CollisionMap>();
                rootCollisionMap.subMaps.Add(GenerateMapFor(childABodies, maxNumberOfElements, boundsChildA));
                rootCollisionMap.subMaps.Add(GenerateMapFor(childBBodies, maxNumberOfElements, boundsChildB));
            }

            return rootCollisionMap;
        }

        public override string ToString()
        {
            return "[bounds.min.x: " + bounds.min.x + ", bounds.max.x: " + bounds.max.x + "]";
        }

        public int GetTotalCollisionMapsCount()
        {
            if (bodies != null) return 1;
            else
            {
                var temp = 0;
                for (var i = 0; i < subMaps.Count; i++)
                {
                    temp += subMaps[i].GetTotalCollisionMapsCount();
                }
                return temp;
            }
        }
    }

}
