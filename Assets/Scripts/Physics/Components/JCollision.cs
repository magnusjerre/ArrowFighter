using UnityEngine;

namespace Jerre.JPhysics
{
    [RequireComponent(typeof (PhysicsbodyRectangular))]
    public class JCollision : MonoBehaviour
    {
        PhysicsbodyRectangular thisBody;

        public delegate void CollisionHandler(PhysicsbodyRectangular thisBody, PhysicsbodyRectangular otherBody);
        private CollisionHandler handler;

        void Start()
        {
            thisBody = GetComponent<PhysicsbodyRectangular>();
        }

        public void SetHandler(CollisionHandler handler)
        {
            this.handler = handler;
        }

        public void OnJCollsion(PhysicsbodyRectangular otherBody)
        {
            handler?.Invoke(thisBody, otherBody);
        }
    }
}
