using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerInputComponent)), RequireComponent(typeof (PlayerSettings)), RequireComponent(typeof (PlayerPhysics))]
    public class PlayerMovement : MonoBehaviour
    {

        PlayerInputComponent playerInput;
        PlayerSettings settings;
        PlayerPhysics physics;

        // Start is called before the first frame update
        void Start()
        {
            playerInput = GetComponent<PlayerInputComponent>();
            settings = GetComponent<PlayerSettings>();
            physics = GetComponent<PlayerPhysics>();
        }

        // Update is called once per frame
        void Update()
        {
            var newMoveDirection = playerInput.input.MoveDirection;

            // Speed
            var accelerationSpeed = settings.MaxAcceleration * Time.deltaTime;
            var oldVelocity = physics.MovementDirection * physics.Speed;
            var wantedVelocity = newMoveDirection.normalized * accelerationSpeed * newMoveDirection.magnitude;
            var newVelocity = oldVelocity + (wantedVelocity.sqrMagnitude == 0f ? -accelerationSpeed * physics.MovementDirection : wantedVelocity);
            if (newVelocity.magnitude > settings.MaxSpeed)
            {
                newVelocity = newVelocity.normalized * settings.MaxSpeed;
            }

            physics.Speed = newVelocity.magnitude;
            physics.MovementDirection = newVelocity.normalized;

            transform.Translate(physics.MovementDirection * physics.Speed * Time.deltaTime, Space.World);
            transform.LookAt(transform.position + playerInput.input.MoveDirection);
        }
    }
}
