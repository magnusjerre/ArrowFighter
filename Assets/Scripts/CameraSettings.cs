using UnityEngine;

namespace Jerre
{
    public class CameraSettings : MonoBehaviour
    {
        public int TotalNumberOfCameras;
        public int PlayerNumber;
        public int CameraNumber;

        //public Transform FollowTarget;
        public float MaxCenterOffset = 10f;
        public float MaxCorrectionSpeed = 1f;
        public float CorrectionAcceleration = 10f;
        public float CorrectionTime = 1f;

        void Awake()
        {
        }

        void Start()
        {

        }
    }
}
