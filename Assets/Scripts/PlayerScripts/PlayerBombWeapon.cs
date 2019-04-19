using UnityEngine;
using Jerre.Events;

namespace Jerre
{
    [RequireComponent(typeof(PlayerSettings)), RequireComponent(typeof(PlayerInputComponent))]
    public class PlayerBombWeapon : MonoBehaviour, IAFEventListener, UsePlayerInput
    {
        public BombSettings bombPrefab;

        private PlayerSettings settings;
        private PlayerInputComponent playerInput;

        private bool BombDropped = false;
        private float minTimeBetweenFire;
        private float timeSinceLastFire;

        private bool UsePlayerInput = true;

        void Start()
        {
            settings = GetComponent<PlayerSettings>();
            playerInput = GetComponent<PlayerInputComponent>();
            minTimeBetweenFire = 1f / settings.Fire2Rate;
            timeSinceLastFire = minTimeBetweenFire;
        }

        void Update()
        {
            timeSinceLastFire += Time.deltaTime;

            var tryToDropBomb = UsePlayerInput ? playerInput.input.Fire2 : false;

            if (BombDropped && tryToDropBomb)
            {
                BombDropped = false;
                timeSinceLastFire = 0f;
                AFEventManager.INSTANCE.PostEvent(AFEvents.BombTrigger(settings.playerNumber, true));
            } else if (playerInput.input.Fire2 && timeSinceLastFire >= minTimeBetweenFire)
            {
                timeSinceLastFire = 0;

                BombDropped = true;
                var newBomb = Instantiate(bombPrefab, transform.position, transform.rotation);
                newBomb.PlayerOwnerNumber = settings.playerNumber;
                newBomb.color = settings.color;
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

        public void SetUsePlayerInput(bool usePlayerInput)
        {
            this.UsePlayerInput = usePlayerInput;
        }
    }
}
