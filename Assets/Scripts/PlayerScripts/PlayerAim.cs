using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerInputComponent)), RequireComponent(typeof (PlayerSettings))]
    public class PlayerAim : MonoBehaviour, UsePlayerInput
    {
        PlayerInputComponent playerInput;
        PlayerSettings settings;
        PlayerBoost boost;
        private bool UsePlayerInput = true;
        
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
            var lookDirection = UsePlayerInput ? playerInput.input.LookDirection : Vector3.zero;
            var moveDirection = UsePlayerInput ? playerInput.input.MoveDirection : Vector3.zero;
            var oldLookDirection = transform.forward.normalized;
            var targetLookDirection = (lookDirection.sqrMagnitude == 0f || boost.boosting) ? moveDirection.normalized : lookDirection.normalized;
            var angleRotation = Mathf.Min(Vector3.Angle(oldLookDirection, targetLookDirection), settings.MaxLookRotationSpeedDegs * Time.deltaTime);
            var targetDirection = Vector3.RotateTowards(oldLookDirection, targetLookDirection, angleRotation * Mathf.Deg2Rad, 0f);
            transform.LookAt(transform.position + targetDirection);
        }

        public void SetUsePlayerInput(bool usePlayerInput)
        {
            this.UsePlayerInput = usePlayerInput;
        }
    }
}
