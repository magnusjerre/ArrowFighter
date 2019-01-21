using UnityEngine;
using Jerre.Events;

namespace Jerre
{
    [RequireComponent(typeof(PlayerSettings)), RequireComponent(typeof(PlayerInputComponent))]
    public class PlayerBombWeapon : MonoBehaviour, IAFEventListener
    {
        public BombSettings bombPrefab;

        private PlayerSettings settings;
        private PlayerInputComponent playerInput;
        private AFEventManager eventManager;

        private bool BombDropped = false;
        private float minTimeBetweenFire;
        private float timeSinceLastFire;

        void Start()
        {
            settings = GetComponent<PlayerSettings>();
            playerInput = GetComponent<PlayerInputComponent>();
            eventManager = GameObject.FindObjectOfType<AFEventManager>();
            minTimeBetweenFire = 1f / settings.Fire2Rate;
            timeSinceLastFire = minTimeBetweenFire;
        }

        void Update()
        {
            timeSinceLastFire += Time.deltaTime;

            if (BombDropped && playerInput.input.Fire2)
            {
                BombDropped = false;
                timeSinceLastFire = 0f;
                eventManager.PostEvent(AFEvents.BombTrigger(settings.playerNumber, true));
            } else if (playerInput.input.Fire2 && timeSinceLastFire >= minTimeBetweenFire)
            {
                timeSinceLastFire = 0;

                BombDropped = true;
                var newBomb = Instantiate(bombPrefab, transform.position, transform.rotation);
                newBomb.PlayerOwnerNumber = settings.playerNumber;
            }
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            if (afEvent.type == AFEventType.BOMB_TRIGGER)
            {
                var payload = (BombTriggerPayload)afEvent.payload;
                if (payload.OwnerPlayerNumber == settings.playerNumber)
                {
                    BombDropped = false;
                    timeSinceLastFire = 0f;
                }
            }
            return false;
        }
    }
}
