using UnityEngine;
using System.Collections;

namespace Jerre
{
    [RequireComponent(typeof (PlayerSettings)), RequireComponent(typeof (PlayerInputComponent))]
    public class PlayerBoost : MonoBehaviour
    {
        private PlayerInputComponent playerInput;
        private PlayerSettings settings;

        private float elapsedBoostTime;
        private float timeSinceLastBoost;
        public bool boosting;

        // Use this for initialization
        void Start()
        {
            settings = GetComponent<PlayerSettings>();
            playerInput = GetComponent<PlayerInputComponent>();
        }

        // Update is called once per frame
        void Update()
        {
            var input = playerInput.input;

            if (boosting)
            {
                elapsedBoostTime += Time.deltaTime;
                if (!input.Boost || elapsedBoostTime >= settings.BoostDuration)
                {
                    boosting = false;
                    timeSinceLastBoost = 0f;
                }
            }
            else
            {
                timeSinceLastBoost += Time.deltaTime;
                if (input.Boost && timeSinceLastBoost > settings.BoostPauseDuration)
                {
                    boosting = true;
                    elapsedBoostTime = 0f;
                }
            }
        }
    }
}
