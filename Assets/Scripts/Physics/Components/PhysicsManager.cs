using Jerre.JColliders;
using UnityEngine;

namespace Jerre.JPhysics
{
    public class PhysicsManager : MonoBehaviour
    {
        public static float AbsoluteMaxSpeed = 2500;

        void LateUpdate()
        {
            var pushPairs = JColliderManager.pushPairs;
            var pushPairsCount = pushPairs.Count;

            for (int i = 0; i < pushPairsCount; i++)
            {
                var pushable = pushPairs[i].pushable;
                var pushingFrom = pushPairs[i].pushingFrom;
                if (pushingFrom == null || pushable == null) continue;
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
        }

        void BouncePlayer(PlayerPhysics playerPhysics, Vector3 surfaceNormal, float surfaceBounceFactor)
        {
            var reflectionVector = ReflectIncomingVector(playerPhysics.MovementDirection * playerPhysics.Speed, surfaceNormal);
            var scaledOutgoingVector = ScaleVectorInNormalDirection(reflectionVector, surfaceNormal, surfaceBounceFactor);

            playerPhysics.MovementDirection = scaledOutgoingVector.normalized;
            playerPhysics.SetSpeed(scaledOutgoingVector.magnitude);
            playerPhysics.transform.Translate(scaledOutgoingVector * Time.deltaTime, Space.World);
        }

        void BounceBullet(BulletMover bulletMover, Vector3 surfaceNormal, float surfaceBounceFactor)
        {
            var reflectionVector = ReflectIncomingVector(bulletMover.transform.forward * bulletMover.Speed, surfaceNormal);
            var scaledOutgoingVector = ScaleVectorInNormalDirection(reflectionVector, surfaceNormal, surfaceBounceFactor);

            bulletMover.transform.LookAt(bulletMover.transform.position + scaledOutgoingVector);
            bulletMover.Speed = scaledOutgoingVector.magnitude;
            bulletMover.transform.Translate(Vector3.forward * bulletMover.Speed * Time.deltaTime);
        }

        // Returns the reflected vector of magnitude equal to the input movementdirection
        public static Vector3 ReflectIncomingVector(Vector3 incomingVector, Vector3 surfaceNormal)
        {
            var dotProduct = Vector3.Dot(surfaceNormal, incomingVector);
            return incomingVector - 2 * dotProduct * surfaceNormal;
        }

        // Scales the vector in the normaldirection. Calculated by finding how much the normal-direction increases and then adding the normaldirection * increase to the vector
        public static Vector3 ScaleVectorInNormalDirection(Vector3 vector, Vector3 normalDirection, float scale)
        {
            var lengthOfVectorOnNormal = Vector3.Dot(normalDirection, vector);
            var scaledLengthOfVectorOnNormal = lengthOfVectorOnNormal * scale;
            return vector + normalDirection * (scaledLengthOfVectorOnNormal - lengthOfVectorOnNormal);
        }
    }
}
