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
            AFEventManager.INSTANCE.AddListener(this);
            settings = GetComponent<PlayerSettings>();
            playerInput = GetComponent<PlayerInputComponent>();
            minTimeBetweenFire = settings.BombPauseTime;
            timeSinceLastFire = minTimeBetweenFire;
        }

        void Update()
        {
            if (!playerInput.InputIsFresh) return;

            timeSinceLastFire += Time.deltaTime;

            var tryToDropBomb = UsePlayerInput ? playerInput.input.Fire2 : false;

            if (BombDropped && tryToDropBomb)
            {
                AFEventManager.INSTANCE.PostEvent(AFEvents.BombTrigger(settings.playerNumber, true));
            } else if (tryToDropBomb && timeSinceLastFire >= minTimeBetweenFire)
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
