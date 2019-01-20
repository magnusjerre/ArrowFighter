using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerInputComponent)), RequireComponent(typeof (PlayerSettings))]
    public class PlayerAim : MonoBehaviour
    {
        PlayerInputComponent playerInput;
        PlayerSettings settings;
        PlayerBoost boost;
        
        // Start is called before the first frame update
        void Start()
        {
            playerInput = GetComponent<PlayerInputComponent>();
            settings = GetComponent<PlayerSettings>();
            boost = GetComponent<PlayerBoost>();
        }

        // Update is called once per frame
        void Update()
        {
            var input = playerInput.input;
            var oldLookDirection = transform.forward.normalized;
            var targetLookDirection = (input.LookDirection.sqrMagnitude == 0f || boost.boosting) ? input.MoveDirection.normalized : input.LookDirection.normalized;
            var angleRotation = Mathf.Min(Vector3.Angle(oldLookDirection, targetLookDirection), settings.MaxLookRotationSpeedDegs * Time.deltaTime);
            var targetDirection = Vector3.RotateTowards(oldLookDirection, targetLookDirection, angleRotation * Mathf.Deg2Rad, 0f);
            transform.LookAt(transform.position + targetDirection);
        }
    }
}
