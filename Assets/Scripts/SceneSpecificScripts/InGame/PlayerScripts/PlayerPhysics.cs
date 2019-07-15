using Jerre.JPhysics;
using UnityEngine;

namespace Jerre
{
    public class PlayerPhysics : MonoBehaviour
    {
        public Vector3 MovementDirection;
        public float Speed;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void SetSpeed(float speed)
        {
            if (speed > PhysicsManager.AbsoluteMaxSpeed)
            {
                Speed = PhysicsManager.AbsoluteMaxSpeed;
            }
            else
            {
                Speed = speed;
            }
        }
    }
}
