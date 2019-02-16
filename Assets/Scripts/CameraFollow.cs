using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof(CameraSettings))]
    public class CameraFollow : MonoBehaviour
    {
        private Vector3 CameraTargetOffset = Vector3.up * 5f;

        private CameraSettings settings;

        [SerializeField]
        private Transform FollowTarget;

        public bool HardFollow = false;

        private void Awake()
        {
            settings = GetComponent<CameraSettings>();
        }

        // Start is called before the first frame update
        void Start()
        {
            AcquireTarget();
        }

        // Update is called once per frame
        void Update()
        {
            if (FollowTarget == null)
            {
                AcquireTarget();
                if (FollowTarget == null)
                {
                    return;
                }
            }

            if (HardFollow)
            {
                transform.position = FollowTarget.position + Vector3.up * 5f;
                return;
            }



            var cameraPosInPlayerPlane = new Vector3(transform.position.x, FollowTarget.position.y, transform.position.z);
            var diff = FollowTarget.position - cameraPosInPlayerPlane;
            var distance = diff.magnitude;
            if (distance <= 0.0001f)
            {
                return;
            }
            if (distance > settings.MaxCenterOffset)
            {
                var traveldistance = distance - settings.MaxCenterOffset;
                transform.Translate(diff.normalized * traveldistance, Space.World);
            } else
            {
                UseCorrectionTime(distance, diff);
            }

        }
        private void UseCorrectionTime(float distance, Vector3 diff)
        {
            var distanceRatio = distance / settings.MaxCenterOffset;
            var timeLeft = distanceRatio * settings.CorrectionTime;
            var newTimeLeft = Mathf.Max(0, timeLeft - Time.deltaTime);
            var deltaDistance = Mathf.Lerp(distance, 0, newTimeLeft / timeLeft);
            var translation = diff.normalized * deltaDistance;
            transform.Translate(translation, Space.World);
        }

        private void AcquireTarget()
        {
            var playerSettings = GameObject.FindObjectsOfType<PlayerSettings>();

            for (var i = 0; i < playerSettings.Length; i++)
            {
                if (playerSettings[i].playerNumber == settings.PlayerNumber)
                {
                    this.FollowTarget = playerSettings[i].transform;
                }
            }

            if (playerSettings == null)
            {
                throw new System.Exception("Camera settings couldn't find a player with player number " + settings.PlayerNumber);
            }
            transform.position = FollowTarget.position + CameraTargetOffset;
        }
    }
}
