using System.Collections;
using System.Collections.Generic;
using Jerre.Events;
using Jerre.UIStuff;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.Scoring
{
    public class WinnerText : MonoBehaviour, IAFEventListener
    {
        public Image image;
        public Text WinningScoreText;
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
            switch(afEvent.type) {
                case AFEventType.GAME_OVER: {
                    var payload = (GameOverPayload)afEvent.payload;
                    image.color = payload.playerColor;
                    WinningScoreText.text = payload.score + " Kills";
                    gameObject.SetActive(true);
                    if (Scaler != null) {
                        Scaler.Do();
                    }
                    break;
                }
            }
            return false;
        }
    }
}
