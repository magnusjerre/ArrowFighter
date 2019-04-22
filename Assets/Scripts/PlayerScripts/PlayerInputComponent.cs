using UnityEngine;

namespace Jerre
{
    [RequireComponent(typeof (PlayerSettings))]
    public class PlayerInputComponent : MonoBehaviour
    {
        public PlayerInput input;
        public bool InputIsFresh = false;
        private PlayerSettings settings;
        
        // Start is called before the first frame update
        void Start()
        {
            settings = GetComponent<PlayerSettings>();
        }

        // Update is called once per frame
        void Update()
        {
            var playerNumber = settings.playerNumber;
            input = new PlayerInput(
                Input.GetAxis(PlayerInputTags.MOVE_X + playerNumber),
                Input.GetAxis(PlayerInputTags.MOVE_Y + playerNumber),
                Input.GetAxis(PlayerInputTags.LOOK_X + playerNumber),
                Input.GetAxis(PlayerInputTags.LOOK_Y + playerNumber),
                Input.GetAxis(PlayerInputTags.FIRE + playerNumber) > 0.5f,
                Input.GetButtonDown(PlayerInputTags.FIRE2 + playerNumber),
                Input.GetButtonDown(PlayerInputTags.DODGE_RIGHT + playerNumber),
                Input.GetButtonDown(PlayerInputTags.DODGE_LEFT + playerNumber),
                Input.GetAxis(PlayerInputTags.BOOST + playerNumber) > 0.5f
            );
            InputIsFresh = true;
        }
    }
}
