using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerInputComponent)), RequireComponent(typeof (PlayerSettings)), RequireComponent(typeof (PlayerPhysics))]
    public class PlayerMovement : MonoBehaviour, UsePlayerInput
    {

        PlayerInputComponent playerInput;
        PlayerSettings settings;
        PlayerPhysics physics;
        PlayerBoost boost;
        private bool UsePlayerInput = true;


        // Start is called before the first frame update
        void Start()
        {
            playerInput = GetComponent<PlayerInputComponent>();
            settings = GetComponent<PlayerSettings>();
            physics = GetComponent<PlayerPhysics>();
            boost = GetComponent<PlayerBoost>();
        }

        // Update is called once per frame
        void Update()
        {
            var newMoveDirection = UsePlayerInput ? Vector3.zero : playerInput.input.MoveDirection;

            // Speed
            var acceleration = (boost != null && boost.boosting) ? settings.BoostAcceleration : settings.MaxAcceleration;
            var accelerationSpeed = acceleration * Time.deltaTime;
            var oldVelocity = physics.MovementDirection * physics.Speed;
            var wantedVelocity = newMoveDirection.normalized * accelerationSpeed * newMoveDirection.magnitude;
            var newVelocity = oldVelocity + (wantedVelocity.sqrMagnitude == 0f ? -accelerationSpeed * physics.MovementDirection : wantedVelocity);
            var maxSpeed = (boost != null && boost.boosting) ? settings.BoostSpeed : settings.MaxSpeed;
            if (newVelocity.magnitude > maxSpeed)
            {
                if (boost.boosting)
                {
                    newVelocity = newVelocity.normalized * maxSpeed;
                } else
                {
                    var newSpeed = Mathf.Max(newVelocity.magnitude - settings.BoostAcceleration * Time.deltaTime, settings.MaxSpeed);
                    newVelocity = newVelocity.normalized * newSpeed;
                }
            }

            physics.Speed = newVelocity.magnitude;
            physics.MovementDirection = newVelocity.normalized;

            transform.Translate(physics.MovementDirection * physics.Speed * Time.deltaTime, Space.World);
        }

        public void SetUsePlayerInput(bool usePlayerInput)
        {
            this.UsePlayerInput = usePlayerInput;
        }
    }
}
