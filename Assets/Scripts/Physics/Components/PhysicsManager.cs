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
