using UnityEngine;

namespace Jerre
{
    public class FixedDirection : MonoBehaviour
    {

        public Vector3 LookDirection = Vector3.forward;

        void Start()
        {
            transform.LookAt(transform.position + LookDirection);
        }

        void LateUpdate()
        {
            transform.LookAt(transform.position + LookDirection);
        }
    }
}
