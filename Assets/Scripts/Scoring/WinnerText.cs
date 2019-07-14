using Jerre.Events;
using Jerre.GameSettings;
using Jerre.GC;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.Scoring
{
    public class WinnerText : MonoBehaviour, IAFEventListener
    {
        public Image image;
        public Text ScoreText;
        public Text TitleText;
        public Color PlayerWinnerColor;
        public int WinningScore = 0;
        public ScaleFrom Scaler;

        void Awake()
        {
            AFEventManager.INSTANCE.AddListener(this);
        }

        void Start()
        {
            gameObject.SetActive(false);
        }

        public bool HandleEvent(AFEvent afEvent)
        {
            switch (afEvent.type)
            {
                case AFEventType.ROUND_OVER:
                    {
                        var payload = (RoundOverPayload)afEvent.payload;
                        TitleText.text = "WINNER OF ROUND " + GameSettingsState.INSTANCE.RoundState.CurrentRoundNumber;
                        image.color = payload.playerColor;
                        ScoreText.text = payload.roundScore + " Points";
                        Show();
                        break;
                    }
                case AFEventType.GAME_OVER:
                    {
                        var payload = (GameOverPayload)afEvent.payload;
                        TitleText.text = "WINNER";
                        image.color = payload.playerColor;
                        ScoreText.text = payload.score + " Kills";
                        Show();
                        break;
                    }
            }
            return false;
        }

        private void Show()
        {
            gameObject.SetActive(true);
            var playersContainer = GameObject.FindGameObjectWithTag("PlayersContainer");
            ScaleFrom playersScaler = playersContainer.AddComponent(typeof(ScaleFrom)) as ScaleFrom;
            playersScaler.UseInitialScaleAsFrom = true;
            playersScaler.To = Vector3.zero;
            playersScaler.Do();
            if (Scaler != null)
            {
                Scaler.Do();
            }
        }
    }
}
