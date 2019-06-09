using UnityEngine;

namespace Jerre.JPhysics
{
    public class PhysicsManager : MonoBehaviour
    {
        public static float AbsoluteMaxSpeed = 5000;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            var allPhysicsBodies = GameObject.FindObjectsOfType<PhysicsbodyRectangular>();
            if (allPhysicsBodies.Length < 2)
            {
                return;
            }

            for (var i = 0; i < allPhysicsBodies.Length - 1; i++) {
                for (var j = i + 1; j < allPhysicsBodies.Length; j++)
                {
                    var physicsObjA = allPhysicsBodies[i];
                    var physicsObjB = allPhysicsBodies[j];
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

            playerPhysics.MovementDirection = bounceDirection;
            playerPhysics.SetSpeed(playerPhysics.Speed * surfaceBounceFactor);
        }
    }
}
