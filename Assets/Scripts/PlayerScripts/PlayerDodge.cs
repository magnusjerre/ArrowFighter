using UnityEngine;
using System.Collections;

namespace Jerre
{
    [RequireComponent(typeof (PlayerSettings)), RequireComponent(typeof (PlayerInputComponent))]
    public class PlayerDodge : MonoBehaviour
    {
        private PlayerSettings settings;
        private PlayerInputComponent playerInput;

        private float timeSinceLastDodge;
        private float elapsedDodgeTime;
        private float dodgeDirection;

        // Use this for initialization
        void Start()
        {
            settings = GetComponent<PlayerSettings>();
            playerInput = GetComponent<PlayerInputComponent>();
            elapsedDodgeTime = settings.DodgeDuration;
        }

        // Update is called once per frame
        void Update()
        {
            var input = playerInput.input;
            timeSinceLastDodge += Time.deltaTime;
            elapsedDodgeTime += Time.deltaTime;

            if (elapsedDodgeTime < settings.DodgeDuration || ((input.DodgeRight || input.DodgeLeft) && timeSinceLastDodge >= settings.DodgePauseDuration))
            {
                if (elapsedDodgeTime > settings.DodgeDuration) //Hasn't dodged yet
                {
                    dodgeDirection = input.DodgeRight ? 1f : -1f;
                    elapsedDodgeTime = 0f;
                    timeSinceLastDodge = 0f;
                }

                transform.Translate(Vector3.right * dodgeDirection * settings.DodgeSpeed * Time.deltaTime);
            }
        }

        private void ResetDodgeTime()
        {
            timeSinceLastDodge = settings.DodgePauseDuration;
        }
    }
}
