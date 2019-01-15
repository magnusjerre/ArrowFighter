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
            var tempVelocity = newMoveDirection * settings.MaxSpeed;
            var velocity = tempVelocity * (1f - settings.VelocityStickyNess) + physics.Velocity * settings.VelocityStickyNess;
            if (velocity.sqrMagnitude > settings.MaxSpeed * settings.MaxSpeed)
            {
                velocity = velocity.normalized * settings.MaxSpeed;
            }
            physics.Velocity = velocity;

            transform.Translate(velocity * Time.deltaTime, Space.World);
            transform.LookAt(transform.position + playerInput.input.MoveDirection);
        }
    }
}
