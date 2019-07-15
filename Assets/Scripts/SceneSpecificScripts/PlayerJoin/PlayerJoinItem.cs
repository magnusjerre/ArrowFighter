using UnityEngine;
using UnityEngine.UI;

namespace Jerre
{
    public class PlayerJoinItem : MonoBehaviour, UsePlayerInput
    {
        public int PlayerNumber;
        public Color PlayerColor = Color.white;
        public ViewState viewState = ViewState.NOT_JOINED;

        public Text ReadyGame;
        public Image PlayerImage;
        public GameObject NotJoined;

        private ColorManager colorManager;
        private bool UsePlayerInput;

        public void SetUsePlayerInput(bool UsePlayerInput)
        {
            this.UsePlayerInput = UsePlayerInput;
        }

        void Start()
        {
            colorManager = GameObject.FindObjectOfType<ColorManager>();
            UsePlayerInput = true;
            UpdateViewFromViewState();
        }

        void Update()
        {
            if (!UsePlayerInput) return;

            var joinKeyName = PlayerInputTags.JOIN_LEAVE + PlayerNumber;
            var leaveKeyName = PlayerInputTags.FIRE2 + PlayerNumber;
            var changeColorKeyName = PlayerInputTags.DODGE_RIGHT + PlayerNumber;
            var readyKeyName = PlayerInputTags.ACCEPT + PlayerNumber;
            if (viewState == ViewState.NOT_JOINED)
            {
                if (Input.GetButtonDown(joinKeyName))
                {
                    viewState = ViewState.JOINED_NOT_READY;
                    PlayerColor = colorManager.ExtractNextColor();
                    PlayerImage.color = PlayerColor;
                    UpdateViewFromViewState();
                }
            }
            else if (viewState == ViewState.JOINED_NOT_READY)
            {
                if (Input.GetButtonDown(changeColorKeyName))
                {
                    PlayerColor = colorManager.SwapForNextColor(PlayerColor);
                    PlayerImage.color = PlayerColor;
                    UpdateViewFromViewState();
                }
                else if (Input.GetButtonDown(readyKeyName))
                {
                    viewState = ViewState.JOINED_READY;
                    UpdateViewFromViewState();
                }
                else if (Input.GetButtonDown(leaveKeyName))
                {
                    viewState = ViewState.NOT_JOINED;
                    colorManager.ReturnColor(PlayerColor);
                    PlayerColor = Color.white;
                    PlayerImage.color = PlayerColor;
                    UpdateViewFromViewState();
                }
            }
            else if (viewState == ViewState.JOINED_READY)
            {
                if (Input.GetButtonDown(leaveKeyName))
                {
                    viewState = ViewState.JOINED_NOT_READY;
                    UpdateViewFromViewState();
                }
            }
        }

        private void UpdateViewFromViewState()
        {
            if (viewState == ViewState.NOT_JOINED)
            {
                ReadyGame.gameObject.SetActive(false);
                PlayerImage.gameObject.SetActive(false);
                NotJoined.gameObject.SetActive(true);
            }
            else if (viewState == ViewState.JOINED_NOT_READY)
            {
                ReadyGame.gameObject.SetActive(false);
                PlayerImage.gameObject.SetActive(true);
                NotJoined.gameObject.SetActive(false);
            }
            else if (viewState == ViewState.JOINED_READY)
            {
                ReadyGame.gameObject.SetActive(true);
                PlayerImage.gameObject.SetActive(true);
                NotJoined.gameObject.SetActive(false);
            }
        }

        public enum ViewState
        {
            NOT_JOINED, JOINED_NOT_READY, JOINED_READY
        }
    }
}
