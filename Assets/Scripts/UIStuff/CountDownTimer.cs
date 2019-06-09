using Jerre.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Jerre.UIStuff
{
    [RequireComponent(typeof (Text))]
    public class CountDownTimer : MonoBehaviour
    {
        public int TimeInSeconds = 10;
        public bool CountDown = false;
        public string TimerName = "GameModeCountDownTimer";

        private int timeLeft;
        private Text uiText;

        private void Awake()
        {
            uiText = GetComponent<Text>();
        }

        void Start()
        {
            timeLeft = TimeInSeconds;
            UpdateText();
            if (CountDown)
            {
                StartTimer();
            }
        }

        public void Reset()
        {
            timeLeft = TimeInSeconds;
            CountDown = false;
            UpdateText();
        }

        public void StartTimer()
        {
            Debug.Log("Starting timer, TimerName: " + TimerName);
            if (CountDown)
            {
                Debug.Log("Can't start the timer twice, TimerName: " + TimerName);
                return;
            }
            Reset();
            CountDown = true;
            Invoke("DoUpdateTimeLeftMoreThanOneSecondLeft", 1);
        }

        void DoUpdateTimeLeftMoreThanOneSecondLeft()
        {
            timeLeft--;
            UpdateText();
            if (timeLeft <= 1)
            {
                Invoke("DoUpdateTimeLeftOnSecondLeft", 1);
            }
            else
            {
                Invoke("DoUpdateTimeLeftMoreThanOneSecondLeft", 1);
            }
        }

        void UpdateText()
        {
            var minutes = timeLeft / 60;
            var seconds = timeLeft % 60;
            uiText.text = (minutes < 10 ? "0" + minutes : minutes.ToString()) + ":" + (seconds < 10 ? "0" + seconds : seconds.ToString());
        }

        void DoUpdateTimeLeftOnSecondLeft()
        {
            timeLeft--;
            UpdateText();
            Invoke("NotifyCountDownFinished", 1);
        }

        void NotifyCountDownFinished()
        {
            timeLeft = 0;
            UpdateText();
            AFEventManager.INSTANCE.PostEvent(AFEvents.ContDownOver(TimerName));
        }

        public void StopTimer()
        {
            CountDown = false;
            CancelInvoke();
        }
    }
}
